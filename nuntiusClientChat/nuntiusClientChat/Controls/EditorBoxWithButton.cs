using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

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

            sendButton.Clicked += SendButton_Clicked;
        }

        private void SendButton_Clicked(object sender, EventArgs e)
        {
           
        }
    }
}
