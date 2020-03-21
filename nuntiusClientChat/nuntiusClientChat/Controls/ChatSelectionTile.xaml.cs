using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat.Controls
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[Serializable]
	public partial class ChatSelectionTile : ContentView
	{
		private ChatPage chatPage;
		public ChatSelectionTile()
		{
			InitializeComponent();
			UnReadMsgs = 30;
			PartnerAlias = "   TestUser";
			ConfigGrid();
		}
		public ChatSelectionTile(ChatPage chat)
		{
			InitializeComponent();
			UnReadMsgs = 0;
			chatPage = chat;
			PartnerAlias = chatPage.Chat.Partner;
			PartnerAlias = "   " + PartnerAlias;

			Device.BeginInvokeOnMainThread(() =>
			{
				ConfigGrid();
			});
		}

		private void ConfigGrid()
		{
			//Adding the User Item
			Grid.Children.Add(new Image(), 1, 1);
			//Adding the User Alias
			Grid.Children.Add(new Label { Text = PartnerAlias, FontAttributes = FontAttributes.Bold, VerticalTextAlignment = TextAlignment.Center }, 2, 1);
			//Displaing the Unred Messeges
			Grid.Children.Add(new Label { Text = UnReadMsgs.ToString() }, 3, 1);

			Grid.Children.Remove(OpenChatButten);
			Grid.Children.Add(OpenChatButten);
		}
		/// <summary>
		/// Opens the underlying Caht Instanc
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void OpenChatButten_Clicked(object sender, EventArgs e)
		{
			if (chatPage == null)
			{
				return;
			}
			else
			{
				await Navigation.PushAsync(chatPage, true);
				await chatPage.ChatScrollView.ScrollToAsync(chatPage.ChatStackLayout, ScrollToPosition.End, false);
			}

		}

		public Image UserImg { get; set; }
		public string PartnerAlias { get; set; }
		public int UnReadMsgs { get; set; }
		public ChatPage ChatPage { get => chatPage; set => chatPage = value; }
	}
}