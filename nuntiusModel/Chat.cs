using System;
using System.Collections.Generic;
using System.Text;

namespace nuntiusModel
{	
	[Serializable]
	public class Chat
	{
		public List<Message> ChatMessages { get; set; }
		public string Owner { get; set; }
		public string Partner { get; set; }

		public Chat()
		{
			ChatMessages = new List<Message>();
		}
	}
}
