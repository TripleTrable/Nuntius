using nuntiusClientChat.Controller;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

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
			LoginTyp.Text = typSwitch.IsToggled == false ? "Login" : "Registrieren";
		}

		private async void continueButton_ClickedAsync(object sender, EventArgs e)
		{
			
			if (AliasEntry.Text == null || PasswordEntry.Text == null || AliasEntry.Text == "" || PasswordEntry.Text == "")
			{
				return;
			}
			else
			{
				//Login
				if (!typSwitch.IsToggled)
				{

					await Task.WhenAny(NetworkController.SendLoginRequestAsync(AliasEntry.Text, PasswordEntry.Text));

					if (UserController.LogedInUser != null && UserController.CurrentTocken != "")
					{  
						//Open the Chat selection
						App.Current.MainPage = new NavigationPage(new ChatSelectionPage());
					}
					else
					{
						if (!await NetworkController.SendPing())
						{
							await DisplayAlert("Error", "Sie Haben keine verbindung zum Internet", "Ok");
						}
						else
						{
							await DisplayAlert("Error", "Überprüfen Sie Ihre eingabe", "Ok");
						}
						return;
					}
				}
				//Regiter
				else
				{
					await Task.WhenAll(NetworkController.SendRegisterRequestAsync(AliasEntry.Text, PasswordEntry.Text));

					if (UserController.LogedInUser != null && UserController.CurrentTocken != "")
					{   //Open the Chat selection
						App.Current.MainPage = new NavigationPage(new ChatSelectionPage());
					}
					else
					{
						if (!await NetworkController.SendPing())
						{
							await DisplayAlert("Error", "Sie Haben keine verbindung zum Internet", "Ok");
						}
						else
						{
							await DisplayAlert("Error", "Überprüfen Sie Ihre eingabe", "Ok");
						}
					
						return;
					}
				}
			

			}

		}


	}
}