using ArcGIS.Desktop.Framework.Dialogs;
using ArcGIS.Desktop.Internal.Catalog.PropertyPages.NetworkDataset;
using OpenAI_API;
using OpenAI_API.Chat;
using PAMChatGPT.Enums;
using PAMChatGPT.Extensions;
using PAMChatGPT.Properties;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PAMChatGPT
{
    public class Bot
    {
        private Conversation chat = null;

        public ObservableCollection<Message> Messages { get; } = new ObservableCollection<Message>();

        private static Bot instance;
        private static readonly object locker = new();
        public static Bot GetBot()
        {
            if (instance == null)
            {
                lock (locker)
                {
                    instance ??= new Bot();
                }
            }

            return instance;
        }

        protected Bot()
        {
            if (Settings.Default.UseAPI.ToLowerInvariant() == UseAPI.OpenAI.GetDescription().ToLowerInvariant())
            {
                Api = new()
                {
                    Auth = new APIAuthentication(Settings.Default.OpenAI_api_key)
                };

                if (!string.IsNullOrWhiteSpace(Settings.Default.OpenAI_Organization))
                {
                    Api.Auth.OpenAIOrganization = Settings.Default.OpenAI_Organization;
                }
            }
            else if (Settings.Default.UseAPI.ToLowerInvariant() == UseAPI.OpenAI_Azure.GetDescription().ToLowerInvariant())
            {
                Api = OpenAIAPI.ForAzure(Settings.Default.OpenAIAzure_ResourceName, Settings.Default.OpenAIAzure_DeploymentId, Settings.Default.OpenAIAzure_api_key);
                Api.ApiVersion = Settings.Default.OpenAIAzure_APIVersion;
            }

        }

        public OpenAIAPI Api
        {
            get;set; 
        }


        public async Task SendActivityAsync(string text, MessageFrom messageFrom = MessageFrom.User, string assistantText = null, string userText = null)
        {
            try
            {
                CreateConversationIfNotExistAsync();
                if (!string.IsNullOrWhiteSpace(userText))
                {
                    Messages.Add(new Message
                    {
                        MessageFrom = MessageFrom.UserCode,
                        Text = userText,
                    });

                    chat.AppendUserInput(userText);
                }
                else if (!string.IsNullOrWhiteSpace(assistantText))
                {
                    Messages.Add(new Message
                    {
                        MessageFrom = MessageFrom.AssistantCode,
                        Text = assistantText,
                    });

                    chat.AppendUserInput(assistantText);
                }

                Messages.Add(new Message
                {
                    MessageFrom = messageFrom,
                    Text = text,
                });
                
                chat.AppendUserInput(text);
                string response = await chat.GetResponseFromChatbotAsync();

                Message message = new()
                {
                    MessageFrom = MessageFrom.Bot,
                    Text = response
                };

                Messages.Add(message);

                //await foreach (var res in chat.StreamResponseEnumerableFromChatbotAsync())
                //{
                //    message.Text += res;
                //}

            }
            catch (System.Security.Authentication.AuthenticationException)
            {
                ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show("Invalid OpenAI api key set in Options -> ChatGPT!",
                    "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                Messages.Add(new Message
                {
                    MessageFrom = MessageFrom.System,
                    Text = ex.Message
                });
            }

        }

        public void ClearChat()
        {
            Messages.Clear();
            chat = null;
        }

        private void CreateConversationIfNotExistAsync()
        {
            if (chat != null)
            {
                return;
            }

            
            
            

            chat = Api.Chat.CreateConversation();


            if (Settings.Default.UseAPI.ToLowerInvariant() == UseAPI.OpenAI.GetDescription().ToLowerInvariant())
            {
                string modelName = Settings.Default.OpenAI_Model;

                if (string.IsNullOrWhiteSpace(modelName))
                {
                    modelName = OpenAI_API.Models.Model.DefaultModel.ModelID;
                }
                chat.Model = new OpenAI_API.Models.Model(modelName);
            }
        }

    }
}
