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
		static ChatSelectionController chatSelectionController;
		public event EventHandler<ChatEventArgs> ChatAdded;
		private List<List<Message>> sotedChatMessages;
		private List<Chat> currentChats;

		public ChatSelectionController()
		{
			sotedChatMessages = new List<List<Message>>();
			currentChats = new List<Chat>();

		}

		public static ChatSelectionController GetSelectionController()
		{
			return chatSelectionController == null ? new ChatSelectionController() : chatSelectionController;
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

		public void SortMeseges(List<Message> recievedMsg)
		{

			List<Message> test = new List<Message>();
			test.Add(new Message { From = "Hans", To = "Peter", Sent = DateTime.Now, Text = "Test Nachricht" });
			sotedChatMessages.Add(test);

			if (sotedChatMessages == null)
			{
				Task.Run(() => SortLeftoverMsg(recievedMsg));
			}

			foreach (List<Message> ListMessages in sotedChatMessages)
			{
				List<Message> messageQerry = (from Message in recievedMsg
											  where (Message.From) == ListMessages[0].From
											  select Message).ToList();

				foreach (Message message in messageQerry)
				{
					ListMessages.Add(message);
					//Clears the recieved Messages
					recievedMsg.Remove(message);

				}

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
					Chat chat = new Chat { /*Owner = UserController.LogedInUser.Alias*/Owner = message.To, Partner = message.From, ChatMessages = new List<Message> { message } };
					OnChatAdded(chat);
				}
				NetworkController.NagTimerRun = true;
			}
		}
	}
	public class ChatEventArgs : EventArgs
	{
		public Chat Chat { get; set; }
	}
}


