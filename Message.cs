using System.ComponentModel;
using System.Runtime.CompilerServices;
using PAMChatGPT.Extensions;
namespace PAMChatGPT
{
    public class Message : INotifyPropertyChanged
    {
        
        public MessageFrom MessageFrom { get; set; }

        string text = null;
        public string Text {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChanged();
            }
        }
       
        public string Result {

            get 
            {
                string c = "red";
                if (MessageFrom == MessageFrom.Bot)
                {
                    c = "blue";
                }
                else if ((MessageFrom == MessageFrom.User) || (MessageFrom == MessageFrom.UserCode) || (MessageFrom == MessageFrom.Assistant) || (MessageFrom == MessageFrom.AssistantCode))
                {
                    c = "green";
                }
                
                
                return $"%{{color:{c}}}***{MessageFrom.GetDescription<MessageFrom>()}***%: {Text}"; 
            
            }
                    
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    public enum MessageFrom
    {
        [Description("User")]
        User,
        [Description("Bot")]
        Bot,
        [Description("System")]
        System,
        [Description("Assistant")]
        Assistant,
        [Description("User (code)")]
        UserCode,
        [Description("Assistant (code)")]
        AssistantCode
    }
}
