using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;
using nuntiusModel;
using System;
using System.Collections;
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
				//List<ChatPage> chatPages = (from tile in chatSelectionStack.Children.OfType<ChatSelectionTile>()
				//							select tile.ChatPage).ToList();

				List<ChatSelectionTile> chatSeletion = CurrentChatSelectionTiles();

				foreach (var updatedChat in e.ChatList)
				{

					//var chats = (from page in chatPages
					//			 where page.Chat.Partner == updatedChat.Partner
					//			 select page).ToList();

					var tile = (from t in chatSeletion
								where t.ChatPage.Chat.Partner == updatedChat.Partner
								select t).ToList();

					//Add Messeges to the List of the Model
					//chats[0].Chat.ChatMessages.AddRange(updatedChat.ChatMessages);

					tile[0].ChatPage.Chat.ChatMessages.AddRange(updatedChat.ChatMessages);

					//Add Messeges to the ChatView 
					foreach (Message message in updatedChat.ChatMessages)
					{
						//chats[0].ChatStackLayout.Children.Add(new MessageControll(false, message));
						//chats[0].ChatScrollView.ScrollToAsync(chats[0].ChatStackLayout, ScrollToPosition.End, false);

						tile[0].ChatPage.ChatStackLayout.Children.Add(new MessageControll(false, message));
						tile[0].ChatPage.ChatScrollView.ScrollToAsync(tile[0].ChatPage.ChatStackLayout, ScrollToPosition.End, false);
					}
					if (updatedChat.ChatMessages.Count != 0)
					{
						OrderMostRecentChat(tile[0]);
					}

				}

			});
		}

		private List<ChatSelectionTile> CurrentChatSelectionTiles()
		{
			return (from tile in chatSelectionStack.Children.OfType<ChatSelectionTile>()
					select tile).ToList();
		}

		private void Chat_Added(object source, ChatEventArgs args)
		{
			ChatPage chatPage = new ChatPage(args.Chat);
			ChatSelectionTile chatSelectionTile = new ChatSelectionTile(chatPage);


			Device.BeginInvokeOnMainThread(() =>
			{
				chatSelectionStack.Children.Add(chatSelectionTile);
				chatSelctScroll.ScrollToAsync(chatSelectionStack, ScrollToPosition.Start, false);
			});

			OrderMostRecentChat(chatSelectionTile);

		}

		public void OrderMostRecentChat(ChatSelectionTile tile)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				List<ChatSelectionTile> selectionTiles = CurrentChatSelectionTiles();

				selectionTiles.Remove(tile);

				selectionTiles.Reverse();

				selectionTiles.Add(tile);

				selectionTiles.Reverse();

				//Added the new Order of Chat Seletion Tiles
				foreach (var item in selectionTiles)
				{
					chatSelectionStack.Children.Remove(item);
					chatSelectionStack.Children.Add(item);
				}
			
			});

		}
	}
}