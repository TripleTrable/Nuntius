using System;

namespace nuntiusModel
{
	[Serializable]
	public class Message
	{
		private string from;
		private string to;
		private DateTime sent;
		private string text;

		#region Properties

		public string From
		{
			get { return from; }
			set { from = value; }
		}
		public string To
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