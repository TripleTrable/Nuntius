using nuntiusClientChat.Controller;
using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatPage : ContentPage
	{

		public ChatPage()
		{
			InitializeComponent();
			chatScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
			chatScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Never;
		}

		public ChatPage(Chat chat)
		{
			InitializeComponent();
			chatScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
			chatScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

			if (chat == null)
			{
				return;
			}

			this.Chat = chat;

			foreach (Message m in chat.ChatMessages)
			{
				//Sedn
				if (m.From == UserController.LogedInUser.Alias)
				{
					ChatStackLayout.Children.Add(new MessageControll(true, m));
				}
				//Receved
				else if (m.From != UserController.LogedInUser.Alias)
				{
					ChatStackLayout.Children.Add(new MessageControll(false, m));
				}
			}

		}




		private async void MsgSend_Clicked(object sender, EventArgs e)
		{
			if (Chat.Partner == null || MsgEditor.Text == null || MsgEditor.Text == "")
			{
				return;
			}

			//Send Msg via Networkkontroller
			//TODO: Settings Trim On/Off
			await Task.Run(() => NetworkController.SendMsgRequest(Chat.Partner, DateTime.Now, MsgEditor.Text.Trim()));

			//Add Msg to View
			Message message = new Message { From = UserController.LogedInUser.Alias, To = Chat.Partner, Sent = DateTime.Now, Text = MsgEditor.Text };
			MsgChatStack.Children.Add(new MessageControll(true, message));
			Chat.ChatMessages.Add(message);
			MsgEditor.Text = null;

			await chatScroll.ScrollToAsync(ChatStackLayout, ScrollToPosition.End, false);


		}

		public Chat Chat { get; set; }
		public StackLayout ChatStackLayout
		{
			get { return MsgChatStack; }
			set { MsgChatStack = value; }
		}

		public ScrollView ChatScrollView
		{
			get { return chatScroll; }
			set { chatScroll = value; }
		}

	}
}