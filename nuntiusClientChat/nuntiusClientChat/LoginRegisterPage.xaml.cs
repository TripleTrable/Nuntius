using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using nuntiusModel;
using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginRegisterPage : ContentPage
	{
		public LoginRegisterPage()
		{
			InitializeComponent();
			AliasEntry.Text = null; PasswordEntry.Text = null;

		}


		private void Entry_Completed(object sender, EventArgs e)
		{
			PasswordEntry.Focus();
		}
		private void PasswordEntry_Completed(object sender, EventArgs e)
		{

		}

		private void Switch_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			LoginTyp.Text = LoginTyp.Text == "Existierender benutzer." ? "Neuer benutzer." : "Existierender benutzer.";
		}

		private async void continueButton_ClickedAsync(object sender, EventArgs e)
		{
			//App.Current.MainPage = new NavigationPage(new ChatSelectionPage());
			//NetworkController.sendNaggRequstAsync();
			if (AliasEntry.Text == null || PasswordEntry.Text == null || AliasEntry.Text == "" || PasswordEntry.Text == "")
			{
				return;
			}
			else
			{
				//Login
				if (LoginTyp.Text == "Existierender benutzer.")
				{
					await Task.WhenAll(NetworkController.SendLoginRequestAsync(AliasEntry.Text, PasswordEntry.Text));
				}
				//Regiter
				else if (LoginTyp.Text == "Neuer benutzer.")
				{
					await Task.WhenAll(NetworkController.SendRegisterRequestAsync(AliasEntry.Text, PasswordEntry.Text));
				}
				//Open the Chat selection
				App.Current.MainPage = new NavigationPage(new ChatSelectionPage());
		
			}

		}


	}
}