using Npgsql;
using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace NuntiusServer
{
	public static class DbController 
	{
		private static string connectionString = "Server=localhost;Port=5432;Database=nuntius;Uid=nuntiusserver;Pwd=;";
		//private static string connectionString = "Driver={PostgreSQL Unicode};Server=localhost;Port=5432;Database=nuntius;Uid=nuntiusserver;Pwd=;";
		//private static string connectionString = "Driver={MySQL ODBC 5.2 UNICODE Driver};Server=localhost;Database=nuntius;User=nuntiusserver;Password=;Option=3;";

		/// <summary>
		/// Check the login data
		/// </summary>
		public static bool LogInUser(string alias, string password)
		{
			//ToDo: Use sql parameter
			NpgsqlCommand command = new NpgsqlCommand($"SELECT 1 FROM users WHERE alias = '{alias}' AND pwd_md5 = '{password}'");

			DataTable data = SelectDataTable(command);

			//ToDo: Send Unknown error
			if (data == null)
				return false;
			else if (data.Rows.Count == 1)
				return true;

			return false;
		}

		/// <summary>
		/// Check if a alias already exists
		/// </summary>
		public static bool CheckUsersAliasAvalible(string alias)
		{
			NpgsqlCommand command = new NpgsqlCommand($"SELECT 1 FROM users WHERE alias = '{alias}'");
			DataTable data = SelectDataTable(command);

			//ToDo: Send Unknown error
			if (data == null)
				return false;
			else if (data.Rows.Count == 0)
				return true;

			return false;
		}

		/// <summary>
		/// Register a new unique user
		/// </summary>
		public static bool RegisterUser(string alias, string password, string publicKey)
		{
			if (!CheckUsersAliasAvalible(alias))
				return false;

			string sql = $"INSERT INTO users(alias, pwd_md5, publicKey) VALUES('{alias}','{password}', '{publicKey}');";

			//ToDo: Send Unknown error
			ExecuteNonQuerry(new NpgsqlCommand(sql));
			return true;
		}

		/// <summary>
		/// Get the alias of a user from the token 
		/// </summary>
		public static string GetAliasFromToken(string token)
		{
			NpgsqlCommand command = new NpgsqlCommand($"SELECT u.alias FROM users u JOIN token t ON u.id = t.userID WHERE t.token = '{token}';");
			DataTable data = SelectDataTable(command);

			if (data == null)
				return null;
			else if (data.Rows.Count == 0)
				return "";

			return data.Rows[0].ItemArray[0].ToString();
		}

		/// <summary>
		/// Check if a user already has a token
		/// </summary>
		public static string HasUserAToken(string alias)
		{
			//ToDo: sql Prameter
			string sql = $@"SELECT token FROM token
						     WHERE userID = (SELECT id FROM users WHERE alias = '{alias}');";

			NpgsqlCommand command = new NpgsqlCommand(sql);
			DataTable data = SelectDataTable(command);

			//ToDo: unknown exeption
			if (data == null)
				return "";

			if (data.Rows.Count == 0)
				return "";

			//Extract the token
			string token = data.Rows[0].ItemArray[0].ToString();

			return token;
		}

		/// <summary>
		/// Check if a token is alrady used
		/// </summary>
		/// <param name="token">token to check</param>
		/// <returns>true if the token is not used</returns>
		public static bool IsTokenFree(string token)
		{
			NpgsqlCommand command = new NpgsqlCommand($"SELECT 1 FROM token WHERE token = '{token}'");
			DataTable data = SelectDataTable(command);

			//ToDo: Send unknown error
			if (data == null)
				return false;
			else if (data.Rows.Count == 1)
				return false;

			return true;
		}


		/// <summary>
		/// Inset a new message in the table
		/// </summary>
		/// <param name="fromAlias">alias from the user who sent the message</param>
		/// <param name="toAlias">alias of the user which get the message</param>
		/// <param name="send">date when the message was sent</param>
		/// <param name="message">message</param>
		/// <returns>1 when the message was inseted sucessfuly else 0</returns>
		public static int InsertNewMessage(string fromAlias, string toAlias, DateTime send, string message)
		{
			//ToDo: add parameter
			string sql = $@"INSERT INTO messages(from_user, to_user, send, content) 
							VALUES((SELECT id FROM users WHERE alias = '{fromAlias}'), 
							(SELECT id FROM users WHERE alias = '{toAlias}'), 
							'{send.ToString()}', 
							'{message}')";
			NpgsqlCommand command = new NpgsqlCommand(sql);

			return ExecuteNonQuerry(command);
		}

		/// <summary>
		/// Get all unread messages of a user
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public static List<Message> SelectUnreadMessages(string token)
		{
			string sql = $@"SELECT uf.alias, ut.alias, m.send, m.content, m.id
						     FROM messages m
							 JOIN users uf
							   ON m.from_user = uf.id
							 JOIN users ut
							   ON m.to_user = ut.id
						    WHERE m.to_user = (SELECT userID FROM token WHERE token = '{token}')
							  AND m.unread = true;";

			NpgsqlCommand command = new NpgsqlCommand(sql);

			DataTable data = SelectDataTable(command);

			//Convet data table to messages
			List<Message> newMessages = new List<Message>();
			foreach (DataRow row in data.Rows)
			{
				Message message = new Message()
				{
					From = row.ItemArray[0].ToString(),
					To = row.ItemArray[1].ToString(),
					Sent = Convert.ToDateTime(row.ItemArray[2].ToString()),
					Text = row.ItemArray[3].ToString()
				};
				newMessages.Add(message);

				//Set the message to read
				NpgsqlCommand updateCommand = new NpgsqlCommand($"UPDATE messages SET unread = false WHERE id = {row.ItemArray[4]};");
				ExecuteNonQuerry(updateCommand);
			}

			return newMessages;
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

			NpgsqlCommand command = new NpgsqlCommand(sql);

			//ToDo: unknown exeption
			ExecuteNonQuerry(command);
		}

		/// <summary>
		/// Select the public key of an user
		/// </summary>
		/// <param name="alias">user alias</param>
		/// <returns>public key as a xml string</returns>
		public static string GetUserPublicKey(string alias)
		{
			string sql = $"SELECT u.publickey FROM token t JOIN users u ON t.userID = u.id WHERE u.alias = '{alias}'" ;
			NpgsqlCommand command = new NpgsqlCommand(sql);
			DataTable data = SelectDataTable(command);

			//ToDo: Send unknown error
			if (data == null)
				return null;

			return data.Rows[0].ItemArray[0].ToString();
		}

		/// <summary>
		/// Select a Query and return a DataTable
		/// </summary>
		private static DataTable SelectDataTable(NpgsqlCommand cmd)
		{
			DataTable dt;

			//Connect
			using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
			{
				cmd.Connection = con;
				try
				{
					con.Open();

					//Execute SQL Statement
					using (NpgsqlDataAdapter a = new NpgsqlDataAdapter(cmd))
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
		private static int ExecuteNonQuerry(NpgsqlCommand command)
		{
			int result;

			// create SqlConnection object
			using (NpgsqlConnection con = new NpgsqlConnection(connectionString))
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