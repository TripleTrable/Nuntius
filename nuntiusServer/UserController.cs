using System;
using System.Collections.Generic;
using nuntiusModel;

namespace NuntiusServer
{
	public static class UserController
	{
		private static LinkedList<User> loggedinUser = new LinkedList<User>();

		/// <summary>
		/// Register a new user if the alis is free
		/// </summary>
		public static string RegisterNewUser(string alias, string password)
		{
			User user = new User(alias, password);

			//ToDo: Database insert and check if user already exits

			loggedinUser.AddLast(user);

			return "";
		}

	}
}