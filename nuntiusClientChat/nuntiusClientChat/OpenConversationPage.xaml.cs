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
	public partial class OpenConversationPage : ContentPage
	{
		ChatSelectionController selectionController = NetworkController.selectionController;

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