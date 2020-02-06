using System;
using System.Collections.Generic;
using nuntiusModel;

namespace NuntiusServer
{
	public static class UserController
	{
		/// <summary>
		/// try to log a user in and assingn a token
		/// </summary>
		public static string Login(string alias, string password)
		{
			if (!DbController.LogInUser(alias, password))
				return null;

			string token = GenerateToken(alias);
			DbController.AssignToken(alias, token);

			return token;
		}

		/// <summary>
		/// Register a new user if the alias is free and assing a token
		/// </summary>
		public static string RegisterNewUser(string alias, string password)
		{
			if (!DbController.RegisterUser(alias, password))
				return null;

			return GenerateToken(alias);
		}

		/// <summary>
		/// Grenrate a Token
		/// </summary>
		private static string GenerateToken(string alias, int length = 32)
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