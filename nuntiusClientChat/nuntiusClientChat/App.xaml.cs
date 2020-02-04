﻿using Xamarin.Forms;

namespace nuntiusClientChat
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

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
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
