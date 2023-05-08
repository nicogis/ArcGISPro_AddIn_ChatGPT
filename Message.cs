using System.ComponentModel;
using System.Runtime.CompilerServices;

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
                else if (MessageFrom == MessageFrom.User)
                {
                    c = "green";
                }
                
                
                return $"%{{color:{c}}}***{MessageFrom}***%: {Text}"; 
            
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
        User,
        Bot,
        System
    }
}
