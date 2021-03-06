﻿using nuntiusModel;
using System;
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

				Text += message.Text.Trim();

				DateTime d = message.Sent.ToLocalTime();

				Text = "\n" + Text + "\t\n\t";

				TextColor = Color.FromHex("fdfdfd");

				VerticalTextAlignment = TextAlignment.Center;
				FontSize = 17;

				Margin = new Thickness(1, 0, 1, 0);

				if (send)
				{
					BackgroundColor = Color.FromHex("397cb3");
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
