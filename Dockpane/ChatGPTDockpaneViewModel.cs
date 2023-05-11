using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using System.Collections.ObjectModel;

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
                await ModuleChatGPT.Bot.SendActivityAsync(InputText);

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