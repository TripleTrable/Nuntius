using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using nuntiusClientChat.Controls;
using nuntiusModel;
using System.Threading.Tasks;

using System.ComponentModel;

namespace nuntiusClientChat.Controller
{
	class ChatSelectionController
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
		protected virtual void OnSavedChatAdded(Chat chat)
		{
			SavedChatAdded?.Invoke(this, new ChatEventArgs { Chat = chat });
		}

		protected virtual void OnMessagesAdded(List<Chat> chats)
		{
			MessagesAdded?.Invoke(this, new ChatEventArgs { ChatList = chats });
		}

		public void SortMessages(List<Message> recievedMsg)
		{
			if (recievedMsg == null)
			{
				return;
			}

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
				c.ChatMessages.AddRange(messageQuery);

				foreach (Message message in messageQuery)
				{
					recievedMsg.Remove(message);
				}
				newMesseges.Add(c);


			}

			if (newMesseges.Count != 0)
			{
				OnMessagesAdded(newMesseges);
			}

			//if not all messeges are Sorted new Chats are Created for those leftover Messeges and the Sorting Function is Called agin
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
				SortMessages(recievedMsg);
			}
			else
			{
				NetworkController.NagTimerRun = true;
			}

		}

		
		public void AddSavedChat(Chat chat)
		{
				currentChats.Add(chat);
				OnSavedChatAdded(chat);
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
		public List<Chat> ChatList {get ; set; }
	}

}


