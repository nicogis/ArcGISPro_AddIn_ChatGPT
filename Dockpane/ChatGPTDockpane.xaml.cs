using System.Windows.Controls;

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

        private void cmbQuestion_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = e.AddedItems[0] as string;
            if (text.Contains("..."))
            {
                txtQuestion.Text = text;
            }
            else
            {
                txtQuestion.Text = null;

            }
        }
    }
}
