using System;
using nuntiusModel;

namespace NuntiusServer
{
	//ToDo: implement
	public static class DbController
	{
		/// <summary>
		/// </summary>
		public static bool CheckToken(string token)
		{
			return true;
		}

		/// <summary>
		/// temporörer platzhalter
		/// </summary>
		public static User LogInUser(string alias, string password)
		{
			return new User("test", "1234");
		}

		/// <summary>
		/// temporörer platzhalter
		/// </summary>
		public static User SelectUser(string alias)
		{
			return new User(alias, "1234");
		}

		/// <summary>
		/// temporörer platzhalter
		/// </summary>
		public static bool RegisterUser(string alias, string password)
		{
			return true;
		}

		/// <summary>
		/// temporörer platzhalter
		/// </summary>
		public static void AssignToken(string alias, string token)
		{

		}
	}
}