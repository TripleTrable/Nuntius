using Xamarin.Forms;
using nuntiusClientChat.Controller;
using LocalNotifications;

namespace nuntiusClientChat
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			DependencyService.Get<INotificationManager>().Initialize();


			if (Controller.UserController.LogedInUser != null)
			{
				MainPage = new NavigationPage(new ChatSelectionPage());
		
			}
			else
			{
				MainPage = new LoginRegisterPage();
			}
			
		}

		protected override void OnStart()
	{
			if (UserController.LogedInUser != null && UserController.CurrentTocken != "")
			{
				StorageController.LoadeData();
			}

		}

		protected override void OnSleep()
		{
			StorageController.SaveData();
		}

		protected override void OnResume()
		{
			if (UserController.LogedInUser != null && UserController.CurrentTocken != "")
			{
				StorageController.LoadeData();
			}
		}


	}
}
