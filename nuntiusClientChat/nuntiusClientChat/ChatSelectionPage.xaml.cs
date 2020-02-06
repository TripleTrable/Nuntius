using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using nuntiusModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using nuntiusClientChat.Controls;

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatSelectionPage : ContentPage
	{
		public ChatSelectionPage()
		{
			InitializeComponent();
			//ChatSelectionStack.Children.Add(new Controls.ChatSelectionTile());
		}

		private List<Chat> chats;
		private List<Message> allRecevedMsgs;

		public void SortMeseges(List<Message> recievedMsg)
		{
			foreach (Chat chat in chats)
			{
				List<Message> messageQerry =
					(from message in allRecevedMsgs
					 where (message.From) == chat.Partner.Alias
					 select message).ToList();

				foreach (Message messages in messageQerry)
				{
					chat.ConversationController.AddMessage(messages);
				}
			}
		}

		public List<Message> AllRecevedMsgs
		{
			get { return allRecevedMsgs; }
			set { allRecevedMsgs = value; }
		}

		public List<Chat> Chats
		{
			get { return chats; }
			set { chats = value; }
		}

		private void addNewChat_Clicked(object sender, EventArgs e)
		{
			Chat chat = new Chat();
			chat.Partner = new User { Alias = "Fynn"};
			ChatSelectionStack.Children.Add(new ChatSelectionTile(chat));
		}
	}
}