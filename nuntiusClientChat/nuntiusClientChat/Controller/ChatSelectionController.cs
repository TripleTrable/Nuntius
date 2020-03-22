using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace nuntiusClientChat.Controller
{
	internal class ChatSelectionController
	{
		public event EventHandler<ChatEventArgs> ChatAdded;
		public event EventHandler<ChatEventArgs> MessagesAdded;
		public event EventHandler<ChatEventArgs> SavedChatAdded;
		private List<Chat> currentChats;


		public ChatSelectionController()
		{
			currentChats = new List<Chat>();
		}

		protected virtual void OnChatAdded(Chat chat)
		{
			ChatAdded?.Invoke(this, new ChatEventArgs { Chat = chat });
		}

		protected virtual void OnSavedChatAdded(List<Chat> chats)
		{
			SavedChatAdded?.Invoke(this, new ChatEventArgs { ChatList = chats });
		}

		protected virtual void OnMessagesAdded(List<Chat> chats)
		{
			MessagesAdded?.Invoke(this, new ChatEventArgs { ChatList = chats });
		}
		/// <summary>
		/// Sorts the Incoming Messages 
		/// </summary>
		/// <param name="recievedMsg"></param>
		public void SortMessages(List<Message> recievedMsg)
		{
			if (recievedMsg == null)
					return;
			
			List<Chat> newMesseges = new List<Chat>();

			
			foreach (Chat chat in currentChats)
			{
				List<Message> messageQuery = (from Message in recievedMsg
											  where (Message.From) == chat.Partner
											  select Message).ToList();
				Chat c = new Chat
				{
					Owner = chat.Owner,
					Partner = chat.Partner
				};
				//adds the list of messages to a temp chat 
				c.ChatMessages.AddRange(messageQuery);

				//removes the sortet Messages
				foreach (Message message in messageQuery)
				{
					recievedMsg.Remove(message);
				}
				//adds the temp chat to a List of Chats
				newMesseges.Add(c);

			}
			//When the incoming messages are all sorted, an event is called and the list of chats is added to the user interface. 
			if (newMesseges.Count != 0)
			{
				OnMessagesAdded(newMesseges);
			}

			//If not all messages are sorted, new chats are created for the remaining messages and the sorting function is called again.
			if (recievedMsg.Count != 0)
			{
				var chats = (from c in recievedMsg
							 group c by c.From into cc
							 select cc).ToList();

				foreach (var item in chats)
				{
					Chat c = new Chat { Partner = item.Key.ToString(), Owner = UserController.LogedInUser.Alias };

					AddChat(c);

				}

				//Calls the  metode again
				SortMessages(recievedMsg);
			}
			else
			{
				NetworkController.NagTimerRun = true;
			}

		}

		public void AddSavedChat(List<Chat> chats)
		{
			foreach (Chat chat in chats)
			{
				currentChats.Add(chat);
			}
			OnSavedChatAdded(chats);
		}

		public void AddChat(Chat chat)
		{
			currentChats.Add(chat);
			OnChatAdded(chat);
		}

		public List<Chat> CurrentChats
		{
			get { return currentChats; }
			set { currentChats = value; }
		}

	}

	public class ChatEventArgs : EventArgs
	{
		public Chat Chat { get; set; }
		public List<Chat> ChatList { get; set; }
	}

}


