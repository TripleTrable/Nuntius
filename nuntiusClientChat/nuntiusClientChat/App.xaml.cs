using nuntiusClientChat.Controller;
using nuntiusModel;
using System.Collections.Generic;
using Xamarin.Forms;

namespace nuntiusClientChat
{
	public partial class App : Application
	{
		private int appOpend = 0;
		public App()
		{
			InitializeComponent();
			NetworkController.NagTimerRun = false;

			if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
			{
				DependencyService.Get<INotificationManager>().Initialize();
			}

			if (UserController.LogedInUser != null)
			{
				MainPage = new NavigationPage(new ChatSelectionPage());

			}
			else
			{
				MainPage = new NavigationPage(new LoginRegisterPage());
			}
		
		}

		protected override void OnStart()
		{
			appOpend++;
			if (appOpend == 1)
			{
				StorageController.Loade = true;
				StorageController.LoadeData();
			}
			else
			{
				StorageController.Loade = false;
			}
		}

		protected override void OnSleep()
		{
			if (MainPage.Navigation.NavigationStack.Count >= 0)
			{
				try
				{
					MainPage.Navigation.PopToRootAsync();
				}
				catch (System.Exception)
				{
									
				}
				
			}
			StorageController.SaveData(saveData);
		}

		protected override void OnResume()
		{
		
		}

		private static List<Chat> saveData;

		public static List<Chat> SaveData
		{
			get { return saveData; }
			set { saveData = value; }
		}


	}
}
