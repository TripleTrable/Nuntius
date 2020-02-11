using nuntiusModel;
using Xamarin.Forms;


namespace nuntiusClientChat
{
    class MessageControll : Label
    {
        public MessageControll(bool send, Message message)
        {
            Device.BeginInvokeOnMainThread(() => { 
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
            };});
        }
    }
}
