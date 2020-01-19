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

			return GenerateToken();
		}
		
		/// <summary>
		/// Grenrate a Token
		/// </summary>
		private static string GenerateToken(int length = 32)
		{
			string chars = "ABCDEFGHJKLMNPRSTUVWXYZabcdefghijkmnpqrstuvwxyz23456789@€%!#§$&";
			char[] token = new char[length];
			var random = new Random();

			for (int i = 0; i < length; i++)
			{
				token[i] = chars[random.Next(chars.Length)];
			}

			//ToDo: Check if the token ist used

			return new String(token);
		}

	}
}