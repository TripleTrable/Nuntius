using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat.DebugPages
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DebugPage : ContentPage
	{
		private List<Exception> exceptions;
		private DateTime appLaunch;
		private static DebugPage debugPage;
		public DebugPage()
		{
			InitializeComponent();
		}

		public static DebugPage GetDebugPage()
		{
			if (debugPage == null)
			{
				return debugPage = new DebugPage();
			}
			else
			{
				return debugPage;
			}
		}

		private void saveIPButton_Clicked(object sender, EventArgs e)
		{
			if (ServerIPEnty.Text != "" || ServerIPEnty.Text != null)
			{
				//Set the new Ip
			}
		}

		private void clearEntyButton_Clicked(object sender, EventArgs e)
		{
			ServerIPEnty.Text = "";
		}

		public DateTime AppLaunch
		{
			get { return appLaunch; }
			set { appLaunch = value; }
		}

		public List<Exception> Exceptions
		{
			get { return exceptions; }
			set { exceptions = value; }
		}


	}
}