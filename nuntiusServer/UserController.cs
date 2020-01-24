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
			if (!DbController.RegisterUser(alias, password))
				return null;

			loggedinUser.AddLast(new User(alias, password));

			return GenerateToken();
		}

		/// <summary>
		/// Grenrate a Token
		/// </summary>
		private static string GenerateToken(int length = 32)
		{
			string chars = "ABCDEFGHJKLMNOPRSTUVWXYZabcdefghijkmnopqrstuvwxyz1234567890@€%!#§$&";
			char[] token = new char[length];
			var random = new Random();

			do
			{

				for (int i = 0; i < length; i++)
				{
					token[i] = chars[random.Next(chars.Length)];
				}

			} while (!DbController.CheckToken(new String(token)));

			return new String(token);
		}
	}
}