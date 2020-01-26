using System;
using nuntiusModel;
using System.Data.Odbc;

namespace NuntiusServer
{
	//ToDo: implement
	public static class DbController
	{
		private static string connectionString = "Driver={PostgreSQL};Server=localhost;Port=5432;Database=nuntius;Uid=nuntiusserver;Pwd=;";

		/// <summary>
		/// temporörer platzhalter
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

		/// <summary>
		///  Connect to the database and execute a non querry
		/// </summary>
		private static void ExecuteNonQuerry(OdbcCommand command)
		{
			int result;

			// create SqlConnection object
			using (OdbcConnection con = new OdbcConnection(connectionString))
			{
				try
				{
					// open connection to database
					con.Open();
					command.Connection = con;

					//Execute the commmand
					result = command.ExecuteNonQuery();
				}
				catch (Exception e)
				{
					throw e;
				}
				finally
				{
					// close connection to database
					con.Close();
				}
			}

		}
	}
}