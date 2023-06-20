using ArcGIS.Desktop.Framework.Contracts;
using OpenAI_API;
using OpenAI_API.Models;
using PAMChatGPT.Enums;
using PAMChatGPT.Extensions;
using PAMChatGPT.Properties;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;


namespace PAMChatGPT
{
    internal class ApplicationSettingsViewModel : Page
    {
        private string previousOpenAI_api_key;
        private string previousOpenAI_Organization;
        private string previousOpenAI_Model;

        private string previousOpenAIAzure_api_key;
        private string previousOpenAIAzure_ResourceName;
        private string previousOpenAIAzure_DeploymentId;
        private string previousOpenAPIVersion;

        private string previousUseAPI;


        

        private ObservableCollection<string> useAPIs;
        public ObservableCollection<string> UseAPIs
        {
            get { return useAPIs; }
            set
            {
                if (SetProperty(ref useAPIs, value, () => UseAPIs))
                    IsModified = true;
            }
        }

        private string selectedUseAPI;
        public string SelectedUseAPI
        {
            get { return selectedUseAPI; }
            set
            {
                if (SetProperty(ref selectedUseAPI, value, () => SelectedUseAPI))
                    IsModified = true;
            }
        }



        #region OpenAI
        private string openAI_api_key;
        public string OpenAI_api_key
        {
            get { return openAI_api_key; }
            set
            {
                if (SetProperty(ref openAI_api_key, value, () => OpenAI_api_key))
                    base.IsModified = true;
            }
        }

        private string openAI_Organization;
        public string OpenAI_Organization
        {
            get { return openAI_Organization; }
            set
            {
                if (SetProperty(ref openAI_Organization, value, () => OpenAI_Organization))
                    base.IsModified = true;
            }
        }




        private ObservableCollection<Model> models;
        public ObservableCollection<Model> Models
        {
            get { return models; }
            set
            {
                if (SetProperty(ref models, value, () => Models))
                    IsModified = true;
            }
        }

        private Model selectedModel;
        public Model SelectedModel
        {
            get { return selectedModel; }
            set
            {
                if (SetProperty(ref selectedModel, value, () => SelectedModel))
                    IsModified = true;
            }
        }

        
        #endregion

        #region OpenAIAzure
        private string openAIAzure_api_key;
        public string OpenAIAzure_api_key
        {
            get { return openAIAzure_api_key; }
            set
            {
                if (SetProperty(ref openAIAzure_api_key, value, () => OpenAIAzure_api_key))
                    base.IsModified = true;
            }
        }

        private string openAIAzure_ResourceName;
        public string OpenAIAzure_ResourceName
        {
            get { return openAIAzure_ResourceName; }
            set
            {
                if (SetProperty(ref openAIAzure_ResourceName, value, () => OpenAIAzure_ResourceName))
                    base.IsModified = true;
            }
        }

        private string openAIAzure_DeploymentId;
        public string OpenAIAzure_DeploymentId
        {
            get { return openAIAzure_DeploymentId; }
            set
            {
                if (SetProperty(ref openAIAzure_DeploymentId, value, () => OpenAIAzure_DeploymentId))
                    base.IsModified = true;
            }
        }



        private ObservableCollection<string> apiVersions;
        public ObservableCollection<string> APIVersions
        {
            get { return apiVersions; }
            set
            {
                if (SetProperty(ref apiVersions, value, () => APIVersions))
                    IsModified = true;
            }
        }

        private string selectedAPIVersion;
        public string SelectedAPIVersion
        {
            get { return selectedAPIVersion; }
            set
            {
                if (SetProperty(ref selectedAPIVersion, value, () => SelectedAPIVersion))
                    IsModified = true;
            }
        }

        #endregion



        private static bool isAuthenticationExpanded = true;
        public bool IsAuthenticationExpanded
        {
            get { return isAuthenticationExpanded; }
            set { SetProperty(ref isAuthenticationExpanded, value, () => IsAuthenticationExpanded); }
        }

        private static bool isParametersExpanded = true;
        public bool IsParametersExpanded
        {
            get { return isParametersExpanded; }
            set { SetProperty(ref isParametersExpanded, value, () => IsParametersExpanded); }
        }


        private bool IsDirty()
        {
            if (previousOpenAI_api_key != OpenAI_api_key)
            {
                return true;
            }

            if (previousOpenAI_Organization != OpenAI_Organization)
            {
                return true;
            }

            if (previousOpenAI_Model != SelectedModel.ModelID)
            {
                return true;
            }

            if (previousOpenAIAzure_api_key != OpenAIAzure_api_key)
            {
                return true;
            }

            if (previousOpenAIAzure_ResourceName != OpenAIAzure_ResourceName)
            {
                return true;
            }

            if (previousOpenAIAzure_DeploymentId != OpenAIAzure_DeploymentId)
            {
                return true;
            }

            if (previousOpenAPIVersion != SelectedAPIVersion)
            {
                return true;
            }

            if (previousUseAPI != SelectedUseAPI)
            {
                return true;
            }

            return false;
        }

        

        /// <summary>
        /// Invoked when the OK or apply button on the property sheet has been clicked.
        /// </summary>
        /// <returns>A task that represents the work queued to execute in the ThreadPool.</returns>
        /// <remarks>This function is only called if the page has set its IsModified flag to true.</remarks>
        protected override Task CommitAsync()
        {
            if (IsDirty())
            {
                // save the new settings
                Settings settings = Settings.Default;

                settings.OpenAI_api_key = OpenAI_api_key;
                settings.OpenAI_Organization = OpenAI_Organization;
                settings.OpenAI_Model = SelectedModel.ModelID;

                settings.OpenAIAzure_api_key= OpenAIAzure_api_key;
                settings.OpenAIAzure_ResourceName = OpenAIAzure_ResourceName;
                settings.OpenAIAzure_DeploymentId = OpenAIAzure_DeploymentId;
                settings.OpenAIAzure_APIVersion = SelectedAPIVersion;

                settings.UseAPI = SelectedUseAPI;
                settings.Save();

                ModuleChatGPT.Bot.Api = new OpenAIAPI();

                if (SelectedUseAPI.ToLowerInvariant() == UseAPI.OpenAI.GetDescription().ToLowerInvariant())
                {
                    ModuleChatGPT.Bot.Api.Auth = new APIAuthentication(null);

                    if (!string.IsNullOrWhiteSpace(OpenAI_api_key))
                    {
                        ModuleChatGPT.Bot.Api.Auth.ApiKey = OpenAI_api_key;
                        if (!string.IsNullOrWhiteSpace(OpenAI_Organization))
                        {
                            ModuleChatGPT.Bot.Api.Auth.OpenAIOrganization = OpenAI_Organization;
                        }
                    }
                }
                else if (SelectedUseAPI.ToLowerInvariant() == UseAPI.OpenAI_Azure.GetDescription().ToLowerInvariant())
                {
                    ModuleChatGPT.Bot.Api = OpenAIAPI.ForAzure(OpenAIAzure_ResourceName,OpenAIAzure_DeploymentId, OpenAIAzure_api_key);
                    ModuleChatGPT.Bot.Api.ApiVersion = selectedAPIVersion;
                }

                ModuleChatGPT.Bot.ClearChat();
            }

            

            return Task.FromResult(0);
        }


        /// <summary>
        /// Called when the page loads because it has become visible.
        /// </summary>
        /// <returns>A task that represents the work queued to execute in the ThreadPool.</returns>
        protected override Task InitializeAsync()
        {
            // get the default settings
            Settings settings = Settings.Default;

            useAPIs = new ObservableCollection<string>();

            foreach (var k in Enum.GetValues(typeof(UseAPI)))
            {
                useAPIs.Add(k.GetDescription());
            };

            

            selectedUseAPI = useAPIs[0];
            if (!string.IsNullOrWhiteSpace(settings.UseAPI))
            {
                foreach (var m in useAPIs)
                {
                    if (m.ToLowerInvariant() == settings.UseAPI.ToLowerInvariant())
                    {
                        selectedUseAPI = m;
                        break;
                    }
                }
            }


            // assign to the values binding to the controls
            openAI_api_key = settings.OpenAI_api_key;
            openAI_Organization = settings.OpenAI_Organization;





            // https://platform.openai.com/docs/models/how-we-use-your-data 
            // /v1/chat/completions

            models = new ObservableCollection<Model>
            {
                OpenAI_API.Models.Model.ChatGPTTurbo,
                PAMChatGPT.Models.GPT3_5_0613,
                PAMChatGPT.Models.GPT3_5_16k,
                PAMChatGPT.Models.GPT3_5_16k_0613,
                PAMChatGPT.Models.GPT4_0613,
                PAMChatGPT.Models.GPT4_32k_0613,
                OpenAI_API.Models.Model.GPT4,
                OpenAI_API.Models.Model.GPT4_32k_Context,
                OpenAI_API.Models.Model.ChatGPTTurbo0301
                

            };

            selectedModel = OpenAI_API.Models.Model.ChatGPTTurbo;
            if (!string.IsNullOrWhiteSpace(settings.OpenAI_Model))
            {
                foreach (var m in models)
                {
                    if (m.ModelID.ToLowerInvariant() == settings.OpenAI_Model.ToLowerInvariant())
                    {
                        selectedModel = m;
                        break;
                    }
                }
            }


            openAIAzure_api_key = settings.OpenAIAzure_api_key;
            openAIAzure_ResourceName= settings.OpenAIAzure_ResourceName;
            openAIAzure_DeploymentId = settings.OpenAIAzure_DeploymentId;

            apiVersions = new ObservableCollection<string>
            {
               "2023-03-15-preview",
               "2022-12-01"
            };

            selectedAPIVersion = apiVersions[0];
            if (!string.IsNullOrWhiteSpace(settings.OpenAIAzure_APIVersion))
            {
                foreach (var m in apiVersions)
                {
                    if (m.ToLowerInvariant() == settings.OpenAIAzure_APIVersion.ToLowerInvariant())
                    {
                        selectedAPIVersion = m;
                        break;
                    }
                }
            }
            



            // keep track of the original values (used for comparison when saving)
            previousOpenAI_api_key = OpenAI_api_key;
            previousOpenAI_Organization = OpenAI_Organization;
            previousOpenAI_Model = SelectedModel.ModelID;

            previousOpenAIAzure_api_key = OpenAIAzure_api_key;
            previousOpenAIAzure_ResourceName = OpenAIAzure_ResourceName;
            previousOpenAIAzure_DeploymentId = OpenAIAzure_DeploymentId;
            previousOpenAPIVersion = SelectedAPIVersion;

            previousUseAPI = SelectedUseAPI;

            return Task.FromResult(0);
        }

        

        
    }

}
