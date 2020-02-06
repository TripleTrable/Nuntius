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
		/// temporörer platzhalter
		/// </summary>
		public static bool CheckToken(string token)
		{
			return true;
		}

		/// <summary>
		/// temporörer platzhalter
		/// </summary>
		public static bool LogInUser(string alias, string password)
		{
			//ToDo: Use sql parameter
			OdbcCommand command = new OdbcCommand($"SELECT 1 FROM users WHERE alias = '{alias}' AND pwd_md5 = '{password}'");

			DataTable data = SelectDataTable(command);

			//ToDo: Send Unknown error
			if(data == null)
				return false;
			else if (data.Rows.Count == 1)
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
			DataTable dataTable = SelectDataTable(command);

			//ToDo: Send Unknown error
			if(dataTable == null)
				return false;
			else if(dataTable.Rows.Count == 0)
				return true;
			
			return false;
		}

		/// <summary>
		/// Register a new unique user
		/// </summary>
		public static bool RegisterUser(string alias, string password)
		{
			if(!CheckUsersAliasAvalible(alias))
				return false;

			string sql = $"INSERT INTO users(alias, pwd_md5) VALUES('{alias}','{password}');";

			ExecuteNonQuerry(new OdbcCommand(sql));
			return true;
		}

		/// <summary>
		/// temporörer platzhalter
		/// </summary>
		public static void AssignToken(string alias, string token)
		{

		}

		/// <summary>
		/// Select a Query and return a DataTable
		/// </summary>
		private static DataTable SelectDataTable(OdbcCommand cmd)
		{
			DataTable dt;

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
						dt = new DataTable();
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