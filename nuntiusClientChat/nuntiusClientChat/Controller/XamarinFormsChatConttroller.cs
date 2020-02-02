using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using nuntiusModel;
using nuntiusClientChat.Controls;

namespace nuntiusClientChat.Controller
{
    class XamarinFormsChatConttroller : IChatViewInterface
    {
        public StackLayout chatStack;

        public XamarinFormsChatConttroller()
        {
            chatStack = new StackLayout();
            chatStack.Spacing = 2;

        }

        public void AddMesages(List<Message> messages)
        {
            messages.Add(new Message { Text = "Hallo gehts?" });
            messages.Add(new Message { Text = "Hallo?" });
            messages.Add(new Message { Text = "TEST:::::::.." });

            foreach (var item in messages)
            {
                chatStack.Children.Add(new MessageControll(true, item));
            }
        }

        public void EditMessages()
        {

        }

        public void RemoveMessages()
        {

        }
    }
}
