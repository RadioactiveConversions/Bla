using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2033
{
    public class ChatHistory
    {
        public ChatHistory()
        {
            this.Text = "";
        }

        public ChatHistory(string load)
        {
            this.Text = load;
        }

        public string Text
        {
            get;
            set;
        }

        public void add(string text)
        {
            this.Text += text + System.Environment.NewLine;
        }
    }
}
