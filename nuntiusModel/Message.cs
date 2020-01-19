using System;

namespace nuntiusModel
{
	public class Message
	{
		private User from;
		private User to;
		private DateTime sent;
		private string text;

		#region Properties

		public User From
		{
			get { return from; }
			set { from = value; }
		}
		public User To
		{
			get { return to; }
			set { to = value; }
		}
		public DateTime Sent
		{
			get { return sent; }
			set { sent = value; }
		}
		public string Text
		{
			get { return text; }
			set { text = value; }
		}

		#endregion
	}
}