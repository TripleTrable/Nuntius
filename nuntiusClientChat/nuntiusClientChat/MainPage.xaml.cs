using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using nuntiusModel;
using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;

namespace nuntiusClientChat
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        Entry msgEntry;
        StackLayout ChatStack;

        public MainPage()
        {
            InitializeComponent();
            msgEntry = new Entry(); msgEntry.Placeholder = "";
            ChatStack = new StackLayout { Spacing = 2 };
            ChatGridLayout();
        }

        private void sendButtonClicked(object sender, EventArgs e)
        {
            messagegSend();
        }

        private void messagegSend()
        {
            Message newMessage = new Message { Text = msgEntry.Text };
            ChatStack.Children.Add(new MessageControll(true, newMessage));
            msgEntry.Text = "";
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

            grid.Children.Add(new EditorBoxWithButton(), 1, 2, 2, 3);      

            grid.Children.Add(sendButton, 1, 2, 0, 1);

            // Accomodate iPhone status bar.
            // this.Padding = new Thickness(5, Device.OnPlatform(20, 0, 0), 5, 5);

            // Build the page.
            this.Content = grid;


        }
    }
}
