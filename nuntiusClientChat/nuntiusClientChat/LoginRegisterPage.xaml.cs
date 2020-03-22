using nuntiusClientChat.Controller;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
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
			VersionLabel.Text = "alpha_1.0.3  " + NetworkController.ServerAddres;
			VersionLabel.TextColor = Color.FromHex("ffffff");

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

		private async void ContinueButton_ClickedAsync(object sender, EventArgs e)
		{
			//Check if the user has made an Input
			if (AliasEntry.Text == null || PasswordEntry.Text == null || AliasEntry.Text == "" || PasswordEntry.Text == "")
			{
				return;
			}
			else
			{
				//Login
				if (!typSwitch.IsToggled)
				{
					if (await SendPingAsync())
					{
						await Task.WhenAny(NetworkController.SendLoginRequestAsync(AliasEntry.Text, PasswordEntry.Text));
					}

					if (UserController.LogedInUser != null && UserController.CurrentTocken != "")
					{
						//Open the Chat selection
						App.Current.MainPage = new NavigationPage(new ChatSelectionPage());
						if (Navigation.NavigationStack.Count == 1)
						{   //Loaded the staved Chats
							StorageController.Loade = true;
							StorageController.LoadeData();
						}
					}
					else
					{
						if (!await SendPingAsync())
						{
							//await DisplayAlert("Error", "Sie Haben keine verbindung zum Nuntius Server", "Ok");
						}

						return;
					}
				}
				//Regiter
				else
				{

					if (await SendPingAsync())
					{
						await Task.WhenAll(NetworkController.SendRegisterRequestAsync(AliasEntry.Text, PasswordEntry.Text));
					}

					if (UserController.LogedInUser != null && UserController.CurrentTocken != "")
					{
						//Open the Chat selection
						App.Current.MainPage = new NavigationPage(new ChatSelectionPage());
					}
					else
					{
						if (!await SendPingAsync())
						{
							//await DisplayAlert("Error", "Sie Haben keine verbindung zum Nuntius Server", "Ok");
						}

						return;
					}
				}
			}
		}
		/// <summary>
		/// Checks if the user chan reach the Internet
		/// </summary>
		/// <returns></returns>
		public static async Task<bool> SendPingAsync()
		{
			var connectivity = CrossConnectivity.Current; bool reachable;

			if (!connectivity.IsConnected)
				return false;

			reachable = await connectivity.IsRemoteReachable("google.de", 80, 3000);

			return reachable;
		}
		/// <summary>
		/// User can change the IP for the Server
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void ChangeServerIP_ClickedAsync(object sender, EventArgs e)
		{
			string serverIP = await DisplayPromptAsync("Server Addresse", "Die Aktuelle Server Adresse lautet:\nBitte geben Sie eine gültige IP Adresse an", "OK", "Cancel", NetworkController.ServerAddres);
			//TODO: Check the ip with ipaddres.pars();
			try
			{
				if (NetworkController.TryParsIp(serverIP))
				{
					await DisplayAlert("Server Addresse","Ihre Eingabe wurde übernommern, die Serveraddresse lautet jetzt:\n"+NetworkController.ServerAddres, "OK");
				}
				else
				{
					await DisplayAlert("Server Addresse", "Ihre Eingebe Konnte nicht verarbeitet werden die Server Addresse ist jetzt:\n" + NetworkController.ServerAddres, "OK");
				}
			}
			catch (Exception ex)
			{
				await DisplayAlert("Exception", ex.Message, "OK");
			}
		}

		private void pwdShowButton_Clicked(object sender, EventArgs e)
		{
			if (PasswordEntry.Text == null)
				return;


			if (PasswordEntry.Text.Length >= 0)
			{
				if (PasswordEntry.IsPassword == true)
				{
					PasswordEntry.IsPassword = false;
				}
				else
				{
					PasswordEntry.IsPassword = true;
				}
			}
		}

	}
}