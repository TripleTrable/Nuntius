using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using nuntiusClientChat.Controls;
using nuntiusModel;

namespace nuntiusClientChat.Controller
{
	class ChatSelectionController
	{
		public delegate void ChatSelectionTileAddedEventHandler(object source, ChatSelectionTileEventArgs args);

		public event ChatSelectionTileAddedEventHandler SelectionTileAdded;

		private List<ChatSelectionTile> chatSelectionTiles;
		private List<List<Message>> sotedChatMessages;

		public ChatSelectionController()
		{

		}
		protected virtual void OnChatSelectionTileAdded(ChatSelectionTile chatSelectionTile)
		{
			if (chatSelectionTile != null)
			{
				SelectionTileAdded(this, new ChatSelectionTileEventArgs { ChatSelectionTile = chatSelectionTile });
			}
		}
		public void AddChatSelectionTile(ChatSelectionTile chatSelectionTile)
		{
			chatSelectionTiles.Add(chatSelectionTile);
			OnChatSelectionTileAdded(chatSelectionTile);
		}

		public void SortMeseges(List<Message> recievedMsg)
		{
			foreach (List<Message> ListMessages in sotedChatMessages)
			{
				List<Message> messageQerry = (from Message in recievedMsg
											  where (Message.From) == ListMessages[0].From
											  select Message).ToList();

				foreach (Message message in messageQerry)
				{
					ListMessages.Add(message);
				}
			}

		}
	} 
	public class ChatSelectionTileEventArgs
	{
		public ChatSelectionTile ChatSelectionTile { get; set; }
	}
}

