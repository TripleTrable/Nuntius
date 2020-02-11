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
		private List<Chat> currentChats;

		public ChatSelectionController()
		{
			currentChats = new List<Chat>();
		}

		protected virtual void OnChatAdded(Chat chat)
		{
			ChatAdded?.Invoke(this, new ChatEventArgs { Chat = chat });
		}
		
		public void AddChat(Chat chat)
		{
			currentChats.Add(chat);
			OnChatAdded(chat);
			
		}
		protected virtual void OnMessagesAdded(List<Chat> chats)
		{
			MessagesAdded?.Invoke(this, new ChatEventArgs { ChatList = chats });
		}


		public void SortMeseges(List<Message> recievedMsg)
		{
			
			if (recievedMsg == null)
			{
				return;
			}
			List<Chat> newMesseges = new List<Chat>();

			foreach (Chat chat in currentChats)
			{
				List<Message> messageQerry = (from Message in recievedMsg
											  where (Message.From) == chat.Partner
											  select Message).ToList();
				Chat n = new Chat();
				n.Owner = chat.Owner;
				n.Partner = chat.Partner;
				n.ChatMessages.AddRange(messageQerry);
				chat.ChatMessages.AddRange(messageQerry);
				
				foreach (Message message in messageQerry)
				{
					recievedMsg.Remove(message);
				}
				newMesseges.Add(n);
			}
			if (newMesseges.Count != 0)
			{
				OnMessagesAdded(newMesseges);
			}
			
			//if ther are messeges left Create new chats and Run agin
			Task.Run(() => SortLeftoverMsg(recievedMsg));
		}
		//TODO: Change so that if more then one Msg from one new P is recived that only one chat is createtd.
		private void SortLeftoverMsg(List<Message> recievedMsg)
		{
			if (recievedMsg.Count == 0)
			{
				//Stat the nag timer.
				NetworkController.NagTimerRun = true;
			}
			else
			{
				//Create new Chats 
				foreach (Message message in recievedMsg)
				{
					Chat chat = new Chat { Owner = message.To, Partner = message.From, ChatMessages = new List<Message> { message } };
					currentChats.Add(chat);
					OnChatAdded(chat);
				}
				NetworkController.NagTimerRun = true;
			}
		}
	}
	public class ChatEventArgs : EventArgs
	{
		public Chat Chat { get; set; }
		public List<Chat> ChatList {get ; set; }
	}

}


