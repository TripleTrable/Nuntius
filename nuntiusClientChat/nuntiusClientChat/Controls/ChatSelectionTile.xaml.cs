using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatSelectionTile : ContentView
	{
		private Chat chat;
		public ChatSelectionTile()
		{
			InitializeComponent();
			UnReadMsgs = 30;
			PartnerAlias = "   TestUser";
			configGrid();
		}
		public ChatSelectionTile(Chat chat)
		{
			InitializeComponent();
			UnReadMsgs = 30;
			PartnerAlias = chat.Partner.Alias;   
			this.chat = chat;
			configGrid();

		}

		public void configGrid()
		{
			//Adding the User Item
			Grid.Children.Add(new Image(), 1, 1);
			//Adding the User Alias
			Grid.Children.Add(new Label { Text = PartnerAlias, FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 2, 1);
			//Displaing the Unred Messeges
			Grid.Children.Add(new Label { Text = UnReadMsgs.ToString() }, 3, 1);

			Grid.Children.Remove(OpenChatButten); Grid.Children.Add(OpenChatButten);
		}
		
		private async void OpenChatButten_Clicked(object sender, EventArgs e)
		{
			if (chat == null)
			{
				return;
			}
			else
			{
				//await Navigation.PushAsync(chat, true);
				await Navigation.PushAsync(new Chat(), true);
			}
			
		}

		public Image UserImg { get; set; }
		public string PartnerAlias { get; set; }
		public int UnReadMsgs { get; set; }


	}
}