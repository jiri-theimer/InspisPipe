using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InspisPipe.Models
{
    public class BaseViewModel
    {
        public List<MyMessage> Messages { get; set; }
        public List<MyMainButton> MainButtons { get; set; }
     
        public void ShowErrorMessasge(string text, string header = null)
        {

            handle_add_message(text, header, "danger");
        }
        public void ShowInfoMessasge(string text, string header = null)
        {
            handle_add_message(text, header, "primary");
        }
        public void ShowWarningMessage(string text, string header = null)
        {
            handle_add_message(text, header, "warning");
        }
        private void handle_add_message(string text, string header, string style)
        {
            if (Messages == null) Messages = new List<MyMessage>();
            Messages.Add(new MyMessage() { MessageText = text, MessageHeader = header, MessageStyle = style });
            

            
        }

        public void AddMainButton(string maintext,string url,string additionaltext=null,bool bolUpperCase=false)
        {
            if (this.MainButtons == null) this.MainButtons = new List<MyMainButton>();
            if (bolUpperCase)
            {
                maintext = maintext.ToUpper();
            }
            this.MainButtons.Add(new MyMainButton() { MainText = maintext, Url = url,AddText=additionaltext });
        }
    }
}