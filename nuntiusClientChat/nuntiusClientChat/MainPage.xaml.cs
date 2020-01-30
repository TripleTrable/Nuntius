using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace nuntiusClientChat
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public static StackLayout ChatStack;
        static EditorBoxWithButton msgEditor = new EditorBoxWithButton();
        Entry pwdEntry;
        Entry aliasEntry_;

        public MainPage()
        {
            InitializeComponent();
            ChatStack = new StackLayout { Spacing = 2 };

            LoginPage();
        }


        public void ChatPage()
        {
            Grid grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.DarkGray,

                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(60, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(40, GridUnitType.Absolute) },

                },
                ColumnDefinitions =
                {
                  new ColumnDefinition { Width = new GridLength(10, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(10, GridUnitType.Absolute) }
                }
            };

            grid.Children.Add(new BoxView { Color = Color.Aqua, HeightRequest = 10 }, 0, 1, 0, 3);

            grid.Children.Add(new BoxView { Color = Color.LightBlue, HeightRequest = 10 }, 1, 2, 0, 2);

            grid.Children.Add(new BoxView { Color = Color.LightCyan, HeightRequest = 10 }, 2, 3, 0, 3);

            grid.Children.Add(new BoxView { Color = Color.DarkBlue, HeightRequest = 40 }, 0, 3, 0, 1);

            grid.Children.Add(ChatStack, 1, 2, 1, 3);

            grid.Children.Add(msgEditor, 1, 2, 2, 3);



            // Accomodate iPhone status bar.

            // this.Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 5);

            // Build the page.
            this.Content = grid;

        }
        public void LoginPage()
        {
            Grid grid, pwdGrid;
            UserCredentialsMask(out grid, out pwdGrid);

            aliasEntry_ = new Entry();

            aliasEntry_.MaxLength = 32;

            pwdEntry = new Entry();

            pwdEntry.IsPassword = true;
            pwdEntry.MaxLength = 32;

            Button loginButton = new Button();

            loginButton.Text = "Log In";
            loginButton.Clicked += LoginButton_Clicked;

            Button registerButton = new Button();
            Label registerLabel = new Label();

            registerButton.Clicked += RegisterButton_Clicked_;

            ImageButton showPwd = new ImageButton { /*Source = @"C:\!nuntiusChat\iah71-messenger-nuntius\nuntiusClientChat\nuntiusClientChat\Show.png"*/ VerticalOptions = LayoutOptions.FillAndExpand };
            showPwd.Clicked += ShowPwd_Clicked;
            //showPwd.Source = @"C:\!nuntiusChat\iah71-messenger-nuntius\nuntiusClientChat\nuntiusClientChat\Show.png";

            registerButton.Opacity = 0;
            registerLabel.Text = "Regesteriren";
            registerLabel.HorizontalTextAlignment = TextAlignment.Center;
            registerLabel.VerticalTextAlignment = TextAlignment.Center;
            registerLabel.FontAttributes = FontAttributes.Bold;
            registerLabel.FontSize = 15;
            registerLabel.TextColor = Color.Black;

            grid.Children.Add(aliasEntry_, 1, 2, 1, 2);
            grid.Children.Add(pwdEntry, 1, 2, 3, 4);
            grid.Children.Add(pwdGrid, 1, 2, 3, 4);
            pwdGrid.Children.Add(pwdEntry, 0, 1, 0, 1);
            pwdGrid.Children.Add(showPwd, 1, 2, 0, 1);


            grid.Children.Add(registerLabel, 1, 2, 5, 6);
            grid.Children.Add(registerButton, 1, 2, 5, 6);

            grid.Children.Add(loginButton, 1, 2, 7, 8);

            this.Content = grid;
        }

        public void RegisterPage1()
        {
            Grid grid, pwdGrid, backGrid;
            UserCredentialsMask(out grid, out pwdGrid);

            backGrid = new Grid
            {
                RowDefinitions =
                {
                     new RowDefinition { Height = new GridLength( 1, GridUnitType.Star)},
                     new RowDefinition { Height = new GridLength(190, GridUnitType.Absolute) },
                }

            };

            aliasEntry_ = new Entry();

            aliasEntry_.MaxLength = 32;
            aliasEntry_.Placeholder = "Benutzer Name";

            pwdEntry = new Entry();

            pwdEntry.IsPassword = true;
            pwdEntry.MaxLength = 32;
            pwdEntry.Placeholder = "Passwort";

            Button registerButton = new Button();
            registerButton.BorderColor = Color.Red;

            Button backButton = new Button();
            backButton.Clicked += BackButton_Clicked;

            registerButton.Text = "Regestrieren";
            registerButton.Clicked += RegisterButton_Clicked;

            ImageButton showPwd = new ImageButton { Source = @"C:\!nuntiusChat\iah71-messenger-nuntius\nuntiusClientChat\nuntiusClientChat\Show.png", VerticalOptions = LayoutOptions.FillAndExpand };
            showPwd.Clicked += ShowPwd_Clicked;

            grid.Children.Add(aliasEntry_, 1, 2, 1, 2);
            grid.Children.Add(pwdEntry, 1, 2, 3, 4);
            grid.Children.Add(pwdGrid, 1, 2, 3, 4);
            pwdGrid.Children.Add(pwdEntry, 0, 1, 0, 1);
            pwdGrid.Children.Add(showPwd, 1, 2, 0, 1);

            //Grid to get back to Login
            grid.Children.Add(backGrid, 0, 1, 0, 1);
            backGrid.Children.Add(backButton, 0, 1, 0, 1);


            grid.Children.Add(registerButton, 1, 2, 7, 8);

            this.Content = grid;
        }

        private void BackButton_Clicked(object sender, EventArgs e)
        {
            LoginPage();
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (pwdEntry.Text == null || aliasEntry_.Text == null)
            {
                return;

            }
            else
            {
                //TODO: Loding icon until the User has a token from the Server 
                bool succses = await NetworkController.SendRegisterRequestAsync(aliasEntry_.Text, pwdEntry.Text);
                aliasEntry_.Text = null;
                pwdEntry.Text = null;

                succses = false;

                if (UserController.LogedInUser != null && UserController.CurrentTocken != null && succses)
                {
                    ChatPage();
                }
                else
                {
                    //Register failed
                    await DisplayAlert(" ", "Der Gewählte Benutzername ist schon vergeben.", "Ok").ConfigureAwait(false);
                    return;
                }
            }

        }





        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            if (aliasEntry_.Text == null || pwdEntry.Text == null)
            {
                return;
            }
            else
            {
                //TODO: Loding icon until the User has a token from the Server 
                bool succses = await NetworkController.SendLoginRequestAsync(aliasEntry_.Text, pwdEntry.Text);
                aliasEntry_.Text = null;
                pwdEntry.Text = null;

                if (UserController.LogedInUser != null && UserController.CurrentTocken != null && succses)
                {
                    ChatPage();
                }
                else
                {
                    //Login failed
                    await DisplayAlert(" ", "Das eingegebene Passwort ist falsch.", "Ok").ConfigureAwait(false);
                    return;
                }
            }
        }

        private void ShowPwd_Clicked(object sender, EventArgs e)
        {
            if (pwdEntry.IsPassword)
            {
                pwdEntry.IsPassword = false;
            }
            else
                pwdEntry.IsPassword = true;

            //pwdEntry.IsPassword ? pwdEntry.IsPassword = false : pwdEntry.IsPassword = true;
        }

        private void RegisterButton_Clicked_(object sender, EventArgs e)
        {
            RegisterPage1();
        }

        private static void UserCredentialsMask(out Grid grid, out Grid pwdGrid)
        {
            grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = Color.Gray,

                RowDefinitions =
                {
                    new RowDefinition { Height = new GridLength(250, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(10, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(150, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                    new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) },
                    new RowDefinition { Height = new GridLength(1,GridUnitType.Star)},
                    new RowDefinition{ Height = new GridLength(1,GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) }
                }
            };
            pwdGrid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,

                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) },
                }

            };
        }

    }
}
