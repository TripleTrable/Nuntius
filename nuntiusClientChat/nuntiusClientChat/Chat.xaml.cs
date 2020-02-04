using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nuntiusModel;
using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Chat : ContentPage
	{
		public ConversationController conversationController;
		public Chat()
		{
			InitializeComponent();
			conversationController = new ConversationController();
			conversationController.MessageAdded += ConversationController_MessageAdded;
		}

		private async void MsgSend_Clicked(object sender, EventArgs e)
		{
			//Send Msg via Networkkontroller
			await Task.Run(() => NetworkController.sendMsgRequest("TestUser", DateTime.Now, MsgEditor.Text));
			
			conversationController.AddMessage(new Message { From = "Test", To = "Test" , Sent = DateTime.Now, Text = MsgEditor.Text});

			MsgEditor.Text = null;
		}

		private void ConversationController_MessageAdded(object source, MessageEventArgs args)
		{
			MsgChatStack.Children.Add(new MessageControll(false, args.Message));
		}
	}
}