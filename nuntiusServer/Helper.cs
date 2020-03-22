using System;
using System.Text;

namespace NuntiusServer
{
	public static class Helpers
	{
		public static void ToConsole(this byte[] b)
		{
			System.Console.WriteLine(Encoding.Unicode.GetString(b));
		}
	}
}