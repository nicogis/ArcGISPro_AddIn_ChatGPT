using System.Windows.Controls;
using System.Windows.Input;

namespace PAMChatGPT
{

    /// <summary>
    /// Interaction logic for BookmarkDockpaneView.xaml
    /// </summary>
    public partial class ChatGPTDockpaneView : UserControl
    {
        public ChatGPTDockpaneView()
        {
            InitializeComponent();
        }

        private ChatGPTDockpaneViewModel ViewModel => (ChatGPTDockpaneViewModel)DataContext;
        

        private void TextBoxInput_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (ViewModel.SendMessageCommand.CanExecute(null))
                {
                    ViewModel.SendMessageCommand.Execute(null);
                }
            }
        }

        //private void Window_Closed(object sender, EventArgs e)
        //{
        //    ViewModel.Closed();
        //}

    }
}
