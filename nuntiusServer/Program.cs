using System;

namespace NuntiusServer
{
	internal class Program
	{
		private static void Main(string[] args)
		{
			SocketListener listener = new SocketListener();
			listener.StartListening();
			Console.Read();
		}
	}
}
