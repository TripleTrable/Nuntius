using nuntiusClientChat.Controller;
using nuntiusModel;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OpenConversationPage : ContentPage
	{
		private ChatSelectionController selectionController = NetworkController.selectionController;

		public OpenConversationPage()
		{
			InitializeComponent();

		}



		private async void SerchButton_Clicked(object sender, EventArgs e)
		{
			if (SerchEntry.Text != null)
			{
				Chat chat = new Chat();

				chat.Partner = SerchEntry.Text;
				chat.Owner = UserController.LogedInUser.Alias;

				selectionController.AddChat(chat);

				await Navigation.PopAsync();
			}

		}
	}
}