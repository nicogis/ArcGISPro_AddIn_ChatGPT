using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace PAMChatGPT
{
    public class ChatGPTDockpaneViewModel : DockPane
    {

        private const string DockPaneId = "Dockpane_ChatGPTDockpane";

        private string inputText;
        public string InputText
        {
            get { return inputText; }
            set
            {
                if (SetProperty(ref inputText, value, () => InputText))
                    SendMessageCommand.RaiseCanExecuteChanged();
            }
        }


        

        private bool codeChecked;
        public bool CodeChecked
        {
            get { return codeChecked; }
            set
            {
                SetProperty(ref codeChecked, value, () => CodeChecked);                    
            }
        }

        private string inputCodeLanguage;
        public string InputCodeLanguage
        {
            get { return inputCodeLanguage; }
            set
            {
                SetProperty(ref inputCodeLanguage, value, () => InputCodeLanguage);

            }
        }

        private ObservableCollection<string> questions;
        public ObservableCollection<string> Questions
        {
            get { return questions; }
            set
            {
                SetProperty(ref questions, value, () => Questions);
                    
            }
        }


        private string selectedQuestion;
        public string SelectedQuestion
        {
            get { return selectedQuestion; }
            set
            {
                SetProperty(ref selectedQuestion, value, () => SelectedQuestion);
                    
            }
        }

        private bool isSendingMessage;

        private bool IsSendingMessage
        {
            get => isSendingMessage;
            set
            {
                isSendingMessage = value;

                SendMessageCommand.RaiseCanExecuteChanged();

            }
        }

        private RelayCommand sendMessageCommand;
        public RelayCommand SendMessageCommand => sendMessageCommand ??= new RelayCommand(SendMessageExecute, SendMessageCanExecute);


        public ReadOnlyObservableCollection<Message> Messages { get; }

        private async void SendMessageExecute()
        {
            IsSendingMessage = true;
            try
            {               
                if (this.CodeChecked)
                {
                    string s = InputText;
                    string l = this.InputCodeLanguage;
                    if (!string.IsNullOrWhiteSpace(l))
                    {
                        l = $"{l} ";
                    }

                    s = $"```{l}\r\n{Regex.Replace(InputText, @"^(?:[\t ]*(?:\r?\n|\r))+", "", RegexOptions.Multiline)}\r\n```";

                    await ModuleChatGPT.Bot.SendActivityAsync(s, MessageFrom.UserCode, SelectedQuestion);
                }
                else
                {
                    await ModuleChatGPT.Bot.SendActivityAsync(InputText);
                }

                InputText = null;

            }
            finally
            {
                IsSendingMessage = false;
            }
        }

        private bool SendMessageCanExecute() => !string.IsNullOrWhiteSpace(InputText) && !IsSendingMessage;


        private RelayCommand clearAllCommand;
        public RelayCommand ClearAllCommand => clearAllCommand ??= new RelayCommand(ClearAllExecute, ClearAllCanExecute);

        private void ClearAllExecute()
        {
            ModuleChatGPT.Bot.ClearChat();
        }

        private bool ClearAllCanExecute() => !IsSendingMessage;

        

        public ChatGPTDockpaneViewModel()
        {
            Messages = new ReadOnlyObservableCollection<Message>(ModuleChatGPT.Bot.Messages);
            Questions = new ObservableCollection<string>() {
                "Clean up this code",
                "Comment this code",
                "Demonstrate how to use this code",
                "Explain this code",
                "Find & fix bugs",
                "Is threadsafe",
                "Is this well-written",
                "Make this run faster"
            };

            SelectedQuestion = Questions[3];
            InputCodeLanguage = "python";


        }



        #region Show dockpane 
        /// <summary>
        /// Show the DockPane.
        /// </summary>
        internal static void Show()
        {
            var pane = FrameworkApplication.DockPaneManager.Find(DockPaneId);
            pane?.Activate();
        }


        #endregion Show dockpane 

    }

    /// <summary>
    /// Button implementation to show the DockPane.
    /// </summary>
    internal class ChatGPTDockpane_ShowButton : Button
    {
        protected override void OnClick()
        {
            ChatGPTDockpaneViewModel.Show();
        }
    }
}