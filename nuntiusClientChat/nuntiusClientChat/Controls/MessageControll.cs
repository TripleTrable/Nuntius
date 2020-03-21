using nuntiusModel;
using Xamarin.Forms;


namespace nuntiusClientChat
{
	internal class MessageControll : Label
	{
		/// <summary>
		/// Custem Controll, Dispays a Message
		/// </summary>
		/// <param name="send">ture: User has send the Message false: User has receved the Message</param>
		/// <param name="message"></param>
		public MessageControll(bool send, Message message)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				LineBreakMode = LineBreakMode.WordWrap;

				Text += message.Text;
				TextColor = Color.FromHex("fdfdfd");
				//HeightRequest = HeightRequest + 30;
				VerticalTextAlignment = TextAlignment.Center;
				FontSize = 16;


				if (send)
				{
					BackgroundColor = Color.FromHex("397cb3");
					//TranslationX = 100;
					HorizontalTextAlignment = TextAlignment.End;
				}
				else
				{
					BackgroundColor = Color.FromHex("1f191b");
				};
			});
		}
	}
}
