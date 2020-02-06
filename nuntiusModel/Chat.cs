using System;
using System.Collections.Generic;
using System.Text;

namespace nuntiusModel
{
	public class Chat
	{
		public List<Message> ChatMessages { get; set; }
		public string Owner { get; set; }
		public string Partner { get; set; }
	}
}
