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
		}

		private List<ChatPage> chats;

		private void addNewChat_Clicked(object sender, EventArgs e)
		{
			//ChatPage chat = new ChatPage(new Chat { Owner = "HansRudi", Partner = "Ödön",ChatMessages = new List<Message> { new Message { Text = "Hallo Welt!", To = "HansRudi", From = "Ödön", Sent = DateTime.Now } } });
			ChatPage chat = new ChatPage(new Chat { Owner = "Ödön", Partner = "HansRudi", ChatMessages = new List<Message> { new Message { Text = "Hallo Welt!", From = "HansRudi", To = "Ödön", Sent = DateTime.Now } } });
			ChatSelectionStack.Children.Add(new ChatSelectionTile(chat));
		}

		private void SlectChat()
		{

		}

		public List<ChatPage> Chats
		{
			get { return chats; }
			set { chats = value; }
		}
	}
}