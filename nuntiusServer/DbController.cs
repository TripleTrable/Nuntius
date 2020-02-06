using System;
using nuntiusModel;
using System.Data;
using System.Data.Odbc;

namespace NuntiusServer
{
	//ToDo: implement
	public static class DbController
	{
		private static string connectionString = "Driver={PostgreSQL Unicode};Server=localhost;Port=5432;Database=nuntius;Uid=nuntiusserver;Pwd=;";
		//private static string connectionString = "Driver={MySQL ODBC 5.2 UNICODE Driver};Server=localhost;Database=nuntius;User=nuntiusserver;Password=;Option=3;";

		/// <summary>
		/// Check the login data
		/// </summary>
		public static bool LogInUser(string alias, string password)
		{
			//ToDo: Use sql parameter
			OdbcCommand command = new OdbcCommand($"SELECT 1 FROM users WHERE alias = '{alias}' AND pwd_md5 = '{password}'");

			DataSet data = SelectDataSet(command);

			//ToDo: Send Unknown error
			if (data == null)
				return false;
			else if (data.Tables[0].Rows.Count == 1)
				return true;

			return false;
		}

		/// <summary>
		/// temporörer platzhalter
		/// </summary>
		public static User SelectUser(string alias)
		{
			return new User(alias, "1234");
		}

		/// <summary>
		/// Check if a alias already exists
		/// </summary>
		public static bool CheckUsersAliasAvalible(string alias)
		{
			OdbcCommand command = new OdbcCommand($"SELECT 1 FROM users WHERE alias = '{alias}'");
			DataSet dataSet = SelectDataSet(command);

			//ToDo: Send Unknown error
			if (dataSet == null)
				return false;
			else if (dataSet.Tables[0].Rows.Count == 0)
				return true;

			return false;
		}

		/// <summary>
		/// Register a new unique user
		/// </summary>
		public static bool RegisterUser(string alias, string password)
		{
			if (!CheckUsersAliasAvalible(alias))
				return false;

			string sql = $"INSERT INTO users(alias, pwd_md5) VALUES('{alias}','{password}');";

			ExecuteNonQuerry(new OdbcCommand(sql));
			return true;
		}

		/// <summary>
		/// Check if a user already has a token
		/// </summary>
		public static string IsTokenAvalible(string alias)
		{
			//ToDo: sql Prameter
			string sql = "SELECT token::text FROM token " +
						$"WHERE userID = (SELECT id FROM users WHERE alias = '{alias}');";

			System.Console.WriteLine(sql);
			OdbcCommand command = new OdbcCommand(sql);
			DataSet data = SelectDataSet(command);

			//ToDo: unknown exeption
			if (data == null)
				return "";

			if (data.Tables[0].Rows.Count == 0)
				return "";

			//Extract the token
			string token = data.Tables[0].Rows[0].ItemArray[0].ToString();

			return token;
		}

		/// <summary>
		/// Check if a token is alrady used
		/// </summary>
		public static bool IsTokenFree(string token)
		{
			return true;
		}

		/// <summary>
		/// Assing a new token to a user
		/// </summary>
		public static void AssignToken(string alias, string token)
		{
			//ToDo: sql parameter
			string sql = $"INSERT INTO token(token, expire, userID)" +
						$"VALUES('{token}', '{DateTime.Now.AddHours(24).ToString()}'," +
						$"(SELECT id FROM users WHERE alias = '{alias}'));";

			System.Console.WriteLine(sql);

			OdbcCommand command = new OdbcCommand(sql);

			//ToDo: unknown exeption
			ExecuteNonQuerry(command);
		}

		/// <summary>
		/// Select a Query and return a DataSet
		/// </summary>
		private static DataSet SelectDataSet(OdbcCommand cmd)
		{
			DataSet dt;

			//Connect
			using (OdbcConnection con = new OdbcConnection(connectionString))
			{
				cmd.Connection = con;
				try
				{
					con.Open();

					//Execute SQL Statement
					using (OdbcDataAdapter a = new OdbcDataAdapter(cmd))
					{
						dt = new DataSet();
						a.Fill(dt);
					}
				}
				catch (Exception e)
				{
					throw e;
				}
				finally
				{
					con.Close();
				}
			}

			return dt;
		}

		/// <summary>
		///  Connect to the database and execute a non querry
		/// </summary>
		private static int ExecuteNonQuerry(OdbcCommand command)
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

			return result;
		}
	}
}