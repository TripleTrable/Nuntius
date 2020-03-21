using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CreditPage : ContentPage
	{
		public CreditPage()
		{
			InitializeComponent();
		}


		private void ArthFinkGitHubButton_Clicked(object sender, EventArgs e)
		{
			Xamarin.Essentials.Browser.OpenAsync("https://github.com/ArthFink", Xamarin.Essentials.BrowserLaunchMode.External);
		}

		private void XohooxGitHubButton_Clicked(object sender, EventArgs e)
		{
			Xamarin.Essentials.Browser.OpenAsync("https://github.com/Xohoox", Xamarin.Essentials.BrowserLaunchMode.External);
		}

		private void NuntiusGitHubButton_Clicked(object sender, EventArgs e)
		{
			Xamarin.Essentials.Browser.OpenAsync("https://github.com/GSO-SW/iah71-messenger-nuntius", Xamarin.Essentials.BrowserLaunchMode.External);
		}
	}
}