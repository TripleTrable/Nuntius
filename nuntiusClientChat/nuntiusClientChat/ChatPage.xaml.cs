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
		}

		public ChatPage(Chat chat)
		{
			InitializeComponent();

			if (chat == null)
			{
				return;
			}

			this.Chat = chat;

			foreach (var messages in chat.ChatMessages)
			{
				MsgChatStack.Children.Add(new MessageControll(false, messages));
			}
		}

		public void AddPrerentResponse(List<Message> messages)
		{
			foreach (var item in messages)
			{
				MsgChatStack.Children.Add(new MessageControll(false, item));
			}
		}
		private async void MsgSend_Clicked(object sender, EventArgs e)
		{
			if (Chat.Partner == null || MsgEditor.Text == null || MsgEditor.Text == "")
			{
				return;
			}

			//Send Msg via Networkkontroller
			await Task.Run(() => NetworkController.sendMsgRequest(Chat.Partner, DateTime.Now, MsgEditor.Text));

			//Add Msg to View
			MsgChatStack.Children.Add(new MessageControll(true, new Message { From = UserController.LogedInUser.Alias, To = Chat.Partner, Sent = DateTime.Now, Text = MsgEditor.Text }));
			MsgEditor.Text = null;
		}

		public Chat Chat { get; set; }
		public StackLayout ChatStackLayout
		{
			get { return MsgChatStack; }
			set { MsgChatStack = value; }
		}
	}
}