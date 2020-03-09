using LocalNotifications;
using nuntiusClientChat.Controller;
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


		public ChatSelectionPage()
		{
			InitializeComponent();
			chatSelection = NetworkController.selectionController;

			chatSelection.ChatAdded += Chat_Added;
			chatSelection.MessagesAdded += ChatSelection_MessagesAdded;
			chatSelection.SavedChatAdded += ChatSelection_SavedChatAdded;

			notificationManager = DependencyService.Get<INotificationManager>();
			notificationManager.NotificationReceived += NotificationManager_NotificationReceived;

		}

		private async void NotificationManager_NotificationReceived(object sender, EventArgs e)
		{
			NetworkController.NagTimerRun = false;
			if (!StorageController.Loaded)
			{
				try
				{
					var evtData = (NotificationEventArgs)e;

					await Task.Run(() => NotificationsOpenedAsync(evtData.Title, evtData.Message));

				}
				catch (Exception)
				{

				}
			}

			else
			{
				try
				{
					var evtData = (NotificationEventArgs)e;

					await Task.Run(() => NotificationsOpenedAsync(evtData.Title, evtData.Message));

				}
				catch (Exception)
				{

				}
			}
			NetworkController.NagTimerRun = true;

		}

		private async Task NotificationsOpenedAsync(string titel, string message)
		{
			//Tital is Partner Chat Name
			List<ChatSelectionTile> selectionTiles = CurrentChatSelectionTiles();

			var ChatPage = (from c in selectionTiles
							where c.ChatPage.Chat.Partner == titel
							select c.ChatPage).ToList();

			await Task.Run(() => Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(ChatPage[0])));
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
							foreach (var msg in updatedChat.ChatMessages)
							{
								notificationManager.ScheduleNotification(updatedChat.Partner, msg.Text);
							}

						}
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
		private List<Button> GetButtons()
		{
			return (from button in chatSelectionStack.Children.OfType<Button>()
					select button).ToList();
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


			//selectionTiles.Remove(tile);

			//selectionTiles.Reverse();

			//selectionTiles.Add(tile);

			//selectionTiles.Reverse();


			Device.BeginInvokeOnMainThread(() =>
			{
				List<ChatSelectionTile> selectionTiles = RemoveDuplicats(CurrentChatSelectionTiles());

				Button tempButton = GetButtons()[0];
				chatSelectionStack.Children.Clear();
				chatSelectionStack.Children.Add(tempButton);
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

		/*public void RemoveDuplicates()
		{
			List<ChatSelectionTile> chatSelectionTiles = CurrentChatSelectionTiles();
				
			List<ChatSelectionTile> distinctItems = chatSelectionTiles.GroupBy( c => c.PartnerAlias).Select(y => y.FirstOrDefault()).ToList();

			//TODO: Check if this may Delete new messages
			if (distinctItems.Count != 0)
			{
				/*Button temp = (Button)(chatSelectionStack.Children[0]);

				chatSelectionStack.Children.Clear();

				chatSelectionStack.Children.Add(temp);

				foreach (var item in distinctItems)
				{
					chatSelectionStack.Children.Remove(item);
					chatSelectionStack.Children.Add(item);
				}
			}

		}*/

		#endregion
	}
}