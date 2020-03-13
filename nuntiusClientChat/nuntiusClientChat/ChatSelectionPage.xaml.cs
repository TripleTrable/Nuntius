﻿using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;
using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace nuntiusClientChat
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ChatSelectionPage : ContentPage
	{
		private readonly ChatSelectionController chatSelection;

		private INotificationManager notificationManager;

		private List<int> notificationIDs;

		public ChatSelectionPage()
		{
			InitializeComponent();
			chatSelection = NetworkController.selectionController;

			chatSelection.ChatAdded += Chat_Added;
			chatSelection.MessagesAdded += ChatSelection_MessagesAdded;
			chatSelection.SavedChatAdded += ChatSelection_SavedChatAdded;


			if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
			{
				notificationIDs = new List<int>();
				notificationManager = DependencyService.Get<INotificationManager>();
				notificationManager.NotificationReceived += async (sender, eventArgs) =>
				{
					var evtData = (NotificationEventArgs)eventArgs;
					await NotificationHandel(evtData.Title, evtData.Message);

				};
			}

			try
			{   //if the date is Loaded incored the Duplicats are removed
				RemoveDuplicats();
			}
			catch (Exception)
			{

			}
		}

		private async Task NotificationHandel(string partner, string m)
		{
			NetworkController.NagTimerRun = false;

			try
			{
				//Opens the Chat 
				List<ChatSelectionTile> selectionTiles = CurrentChatSelectionTiles();

				var ChatPage = (from c in selectionTiles
								where c.ChatPage.Chat.Partner == partner
								select c.ChatPage).ToList();

				await Task.Run(() => Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(ChatPage[0])));
			}
			catch (Exception)
			{

			}

			NetworkController.NagTimerRun = true;
		}

		private void ChatSelection_SavedChatAdded(object sender, ChatEventArgs e)
		{
			List<Chat> savedChats = e.ChatList;
			int curruntNummerOfChatSeletionTiles = CurrentChatSelectionTiles().Count;

			if (savedChats.Count == curruntNummerOfChatSeletionTiles)
			{
				return;
			}
			else if (savedChats.Count >= curruntNummerOfChatSeletionTiles)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					foreach (Chat chat in savedChats)
					{
						ChatPage chatPage = new ChatPage(chat);

						ChatSelectionTile chatSelectionTile = new ChatSelectionTile(chatPage);
						chatSelectionStack.Children.Add(chatSelectionTile);

					}
				});
				return;
			}
			else if (savedChats.Count <= curruntNummerOfChatSeletionTiles)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					foreach (Chat chat in savedChats)
					{

						ChatPage chatPage = new ChatPage(chat);

						ChatSelectionTile chatSelectionTile = new ChatSelectionTile(chatPage);
						chatSelectionStack.Children.Add(chatSelectionTile);

					}
				});
			}
			App.SaveData = GetChatsCurrentChats();
		}

		private async void AddNewChat_Clicked(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new OpenConversationPage(), true);
			App.SaveData = GetChatsCurrentChats();
		}

		private void ChatSelection_MessagesAdded(object sender, ChatEventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{

				List<ChatSelectionTile> chatSeletion = CurrentChatSelectionTiles();

				foreach (var updatedChat in e.ChatList)
				{

					var tile = (from t in chatSeletion
								where t.ChatPage.Chat.Partner == updatedChat.Partner
								select t).ToList();

					if (tile.Count == 0)
						continue;
					try
					{
						tile[0].ChatPage.Chat.ChatMessages.AddRange(updatedChat.ChatMessages);

						//Add Messeges to the ChatView 
						foreach (Message message in updatedChat.ChatMessages)
						{
							tile[0].ChatPage.ChatStackLayout.Children.Add(new MessageControll(false, message));
							tile[0].ChatPage.ChatScrollView.ScrollToAsync(tile[0].ChatPage.ChatStackLayout, ScrollToPosition.End, false);
						}
						if (updatedChat.ChatMessages.Count != 0)
						{
							OrderMostRecentChat(tile[0]);

							//Notification Specific code only exec when Android or IOS
							if (Device.RuntimePlatform == Device.Android || Device.RuntimePlatform == Device.iOS)
							{
								foreach (var msg in updatedChat.ChatMessages)
								{
									IReadOnlyList<Page> pages = App.Current.MainPage.Navigation.NavigationStack;
									//if the User is in a Conversation wher he is receiving new Msg no Notifications are Send for this chat
									if (pages[pages.Count - 1] is ChatPage)
									{
										ChatPage cp = pages[pages.Count - 1] as ChatPage;

										if (cp.Chat.Partner != msg.From)
										{
											notificationIDs.Add(notificationManager.ScheduleNotification(updatedChat.Partner, msg.Text));
										}
									}
									else
									{
										notificationIDs.Add(notificationManager.ScheduleNotification(updatedChat.Partner, msg.Text));
									}


								}
							}
						}
						App.SaveData = GetChatsCurrentChats();
					}
					catch (Exception ex)
					{
						DisplayAlert("", ex.ToString(), "Ok");
					}


				}

			});
		}

		private List<ChatSelectionTile> CurrentChatSelectionTiles()
		{
			return (from tile in chatSelectionStack.Children.OfType<ChatSelectionTile>()
					select tile).ToList();
		}

		private List<Chat> GetChatsCurrentChats()
		{
			List<Chat> temp = new List<Chat>();
			foreach (ChatSelectionTile cst in CurrentChatSelectionTiles())
			{
				temp.Add(cst.ChatPage.Chat);
			}
			return temp;
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

		#region Sorting ChatSelection Tiles

		public void OrderMostRecentChat(ChatSelectionTile tile)
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				List<ChatSelectionTile> selectionTiles = RemoveDuplicats(CurrentChatSelectionTiles());

				selectionTiles.Remove(tile);

				selectionTiles.Reverse();

				selectionTiles.Add(tile);

				selectionTiles.Reverse();

				
				//add´s the new Order of Chat Seletion Tiles and Removes Duplicats
				foreach (var item in selectionTiles)
				{
					chatSelectionStack.Children.Add(item);
				}
			});

		}

		public List<ChatSelectionTile> RemoveDuplicats()
		{
			List<ChatSelectionTile> current = CurrentChatSelectionTiles();

			if (current.Count == 0)
			{
				return current;
			}

			int com(ChatSelectionTile a, ChatSelectionTile b)
			{
				if (a.PartnerAlias == b.PartnerAlias)
				{
					if (a.ChatPage.Chat.ChatMessages.Count > b.ChatPage.Chat.ChatMessages.Count)
					{
						return 1;
					}
					else if (a.ChatPage.Chat.ChatMessages.Count == b.ChatPage.Chat.ChatMessages.Count)
					{
						return 0;
					}
					else
					{
						return 0;
					}
				}
				return 0;
			}
			current.Sort(com);

			List<ChatSelectionTile> distinctItems = current.GroupBy(c => c.PartnerAlias).Select(y => y.First()).ToList();


			return distinctItems;


		}

		public List<ChatSelectionTile> RemoveDuplicats(List<ChatSelectionTile> current)
		{

			if (current.Count == 0)
			{
				return current;
			}

			int com(ChatSelectionTile a, ChatSelectionTile b)
			{
				if (a.PartnerAlias == b.PartnerAlias)
				{
					if (a.ChatPage.Chat.ChatMessages.Count > b.ChatPage.Chat.ChatMessages.Count)
					{
						return 1;
					}
					else if (a.ChatPage.Chat.ChatMessages.Count == b.ChatPage.Chat.ChatMessages.Count)
					{
						return 0;
					}
					else
					{
						return 0;
					}
				}
				return 0;
			}
			current.Sort(com);

			List<ChatSelectionTile> distinctItems = current.GroupBy(c => c.PartnerAlias).Select(y => y.First()).ToList();

			return distinctItems;
		}

		#endregion

		public void PopToRootPage()
		{
			Device.BeginInvokeOnMainThread(async () => { await Navigation.PopToRootAsync(); });
		}

		private void DebugMenuItem_Clicked(object sender, EventArgs e)
		{

		}

		private void UserInfo_Clicked(object sender, EventArgs e)
		{

		}

		private void Credits_Clicked(object sender, EventArgs e)
		{

		}

		private void DebugLoade_Clicked(object sender, EventArgs e)
		{
			StorageController.Loade = true;
			StorageController.LoadeData();
		}

		private void DebugClear_Clicked(object sender, EventArgs e)
		{
			StorageController.SaveData(new List<Chat>());

			Device.BeginInvokeOnMainThread(() =>
			{
				chatSelectionStack.Children.Clear();
			});
		}
	}
}