using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using nuntiusModel;
using nuntiusClientChat.Controller;
using System.Threading.Tasks;

namespace nuntiusClientChat.Controls
{
    class EditorBoxWithButton : Grid
    {
        BoxView Box = new BoxView();
        Button sendButton = new Button();
        Editor msgEditor = new Editor();

        public EditorBoxWithButton()
        {

            sendButton.WidthRequest = 30;
            sendButton.Text = "Send";

            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(75, GridUnitType.Absolute) });

            Children.Add(Box, 0, 2, 0, 1);
            Children.Add(msgEditor, 0, 1, 0, 1);
            Children.Add(sendButton, 1, 2, 0, 1);

            sendButton.Clicked += SendButton_ClickedAsync;
        }

        private async void SendButton_ClickedAsync(object sender, EventArgs e)
        {

          
            //Checks if the editor was used to type a msg 
            //TODO: CHECK if the box may otherwise be empty
            if (msgEditor.Text == " " || msgEditor.Text == null)
            {
                return;
            }
            else
            {
                Message message = new Message();

                message.Text = msgEditor.Text;
                message.Sent = DateTime.Now;
                message.To = new User("tom","rwqe234");
                message.From = new User("fynn","123132fg");

                MainPage.ChatStack.Children.Add(new MessageControll(true, message));
                //Empty the Editor
                msgEditor.Text = null;
               await NetworkController.sendMsgRequest("wqqweqwe", "testUser", message.Sent, message.Text);
                
            }
        }
    }
}
