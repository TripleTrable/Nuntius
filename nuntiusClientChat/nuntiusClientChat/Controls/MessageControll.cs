using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using nuntiusModel;


namespace nuntiusClientChat
{
    class MessageControll : Label
    {
        public MessageControll(bool send, Message message)
        {
            LineBreakMode = LineBreakMode.WordWrap;
        
            Text += message.Text;
                        
            //HeightRequest = HeightRequest + 30;
            VerticalTextAlignment = TextAlignment.Center;
            FontSize = 16;
           

            if (send)
            {
                BackgroundColor = Color.LightSkyBlue;
                //TranslationX = 100;
                HorizontalTextAlignment = TextAlignment.End;
                            
            }
            else
            {
                BackgroundColor = Color.Lavender;
            }

        }
    }
}
