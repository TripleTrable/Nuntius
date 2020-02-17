using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;
using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

//TODO: Make MsgChatStack public

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatSelectionPage : ContentPage
	{
		private readonly ChatSelectionController chatSelection;

		public ChatSelectionPage()
		{
			InitializeComponent();
			chatSelection = NetworkController.selectionController;

			chatSelection.ChatAdded += Chat_Added;
			chatSelection.MessagesAdded += ChatSelection_MessagesAdded;
			chatSelection.SavedChatAdded += ChatSelection_SavedChatAdded;
		}

		private void ChatSelection_SavedChatAdded(object sender, ChatEventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				ChatPage chatPage = new ChatPage(e.Chat);
								
				ChatSelectionTile chatSelectionTile = new ChatSelectionTile(chatPage);
				chatSelectionStack.Children.Add(chatSelectionTile);
								
			});
		}

		private async void AddNewChat_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new OpenConversationPage(), true);
		}

		private void ChatSelection_MessagesAdded(object sender, ChatEventArgs e)
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				List<ChatPage> chatPages = (from tile in chatSelectionStack.Children.OfType<ChatSelectionTile>()
											select tile.ChatPage).ToList();


				foreach (var updatedChat in e.ChatList)
				{

					var chats = (from page in chatPages
								 where page.Chat.Partner == updatedChat.Partner
								 select page).ToList();

					
					//TODO: Crash adding new msg to non existing chat when reciving a msg from a new Chat partner
					//Add Messeges to the List of the Model
					chats[0].Chat.ChatMessages.AddRange(updatedChat.ChatMessages);

					//Add Messeges to the ChatView 
					foreach (Message message in updatedChat.ChatMessages)
					{
						
						chats[0].ChatStackLayout.Children.Add(new MessageControll(false, message));
					}

				}

			});
		}

		private void Chat_Added(object source, ChatEventArgs args)
		{
			ChatPage chatPage = new ChatPage(args.Chat);
			ChatSelectionTile chatSelectionTile = new ChatSelectionTile(chatPage);


			Device.BeginInvokeOnMainThread(() =>
			{
				chatSelectionStack.Children.Add(chatSelectionTile);
			});

		}




	}
}