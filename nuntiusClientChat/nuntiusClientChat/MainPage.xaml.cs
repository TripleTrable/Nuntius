using nuntiusClientChat.Controls;
using nuntiusModel;
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

        public MainPage()
        {
            InitializeComponent();
            ChatStack = new StackLayout { Spacing = 2 };
            LoginPage();
        }


        public void ChatGridLayout()
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
            Grid grid = new Grid
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
                    new  RowDefinition{ Height = new GridLength(1,GridUnitType.Star)},
                    new RowDefinition { Height = new GridLength (20, GridUnitType.Absolute)},

                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) }
                }
            };
            Grid pwdGrid = new Grid
            {                   
                VerticalOptions = LayoutOptions.FillAndExpand,
               
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) }, 
                }

        };

            Entry aliasEntry = new Entry();

            aliasEntry.MaxLength = 256;

            pwdEntry = new Entry();

            pwdEntry.IsPassword = true;

            Button loginButton = new Button();

            loginButton.Text = "Log In";

            Button registerButton = new Button();
            Label registerLabel = new Label();

            ImageButton showPwd = new ImageButton();
            showPwd.Clicked += ShowPwd_Clicked;
            showPwd.Source = "Show.png"; 

            registerButton.Opacity = 0.1;
            registerLabel.Text = "Regesteriren";
            registerLabel.HorizontalTextAlignment = TextAlignment.Center;
            registerLabel.VerticalTextAlignment = TextAlignment.Center;
            registerLabel.FontAttributes = FontAttributes.Bold;
            registerLabel.FontSize = 15;
            registerLabel.TextColor = Color.Black;

            grid.Children.Add(aliasEntry, 1, 2, 1, 2);
            grid.Children.Add(pwdEntry, 1, 2, 3, 4);
            grid.Children.Add(pwdGrid, 1, 2, 3, 4);
            pwdGrid.Children.Add(pwdEntry, 0, 1, 0, 1);
            pwdGrid.Children.Add(showPwd, 1, 2, 0, 1);


            grid.Children.Add(registerLabel, 1, 2, 5, 6);
            grid.Children.Add(registerButton, 1, 2, 5, 6);

            grid.Children.Add(loginButton, 1, 2, 7, 8);




            this.Content = grid;
        }

        private void ShowPwd_Clicked(object sender, EventArgs e)
        {
            if (pwdEntry.IsPassword)
            {
                pwdEntry.IsPassword = false;
            }
            else
                pwdEntry.IsPassword = true;

           // pwdEntry.IsPassword ? pwdEntry.IsPassword = false : pwdEntry.IsPassword = true;
        }

    }
}
