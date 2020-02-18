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
			ChatScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
			ChatScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Never;
			

		}

		public ChatPage(Chat chat)
		{
			InitializeComponent();
			ChatScroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Never;
			ChatScroll.VerticalScrollBarVisibility = ScrollBarVisibility.Never;

			

			ChatStackLayout.ChildAdded += ChatStackLayout_ChildAdded;

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


		//TODO: enabel User Scrolling 
		private void ChatStackLayout_ChildAdded(object sender, ElementEventArgs e)
		{
			ChatScroll.ScrollToAsync(ChatStackLayout, ScrollToPosition.End, true);
		}

		private async void MsgSend_Clicked(object sender, EventArgs e)
		{
			if (Chat.Partner == null || MsgEditor.Text == null || MsgEditor.Text == "")
			{
				return;
			}

			//Send Msg via Networkkontroller
			await Task.Run(() => NetworkController.SendMsgRequest(Chat.Partner, DateTime.Now, MsgEditor.Text));

			//Add Msg to View
			Message message = new Message { From = UserController.LogedInUser.Alias, To = Chat.Partner, Sent = DateTime.Now, Text = MsgEditor.Text };
			MsgChatStack.Children.Add(new MessageControll(true, message));
			Chat.ChatMessages.Add(message);
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