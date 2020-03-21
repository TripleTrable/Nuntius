using System.ComponentModel;
using Npgsql;
using NpgsqlTypes;
using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Data;

namespace NuntiusServer
{
	public class PsqlController : IDbController
	{
		private static PsqlController instance = null;
		private static readonly object padlock = new object();
		public string ConnectionString { get; set; }

		private PsqlController()
		{
			ConnectionString = "Server=localhost;Port=5432;Database=nuntius;Uid=nuntiusserver;Pwd=;";
		}

		/// <summary>
		/// Current DbController object
		/// </summary>
		public static PsqlController Instance
		{
			get
			{
				lock (padlock)
				{
					if (instance == null)
						instance = new PsqlController();

					return instance;
				}
			}
		}

		/// <summary>
		/// Check the login data
		/// </summary>
		public bool LogInUser(string alias, string password)
		{
			//ToDo: Use sql parameter
			NpgsqlCommand command = new NpgsqlCommand("SELECT 1 FROM users WHERE alias = @alias AND pwd_md5 = @pwd");
			command.Parameters.AddWithValue("alias", alias);
			command.Parameters.AddWithValue("pwd", password);

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
		public bool CheckUsersAliasAvalible(string alias)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT 1 FROM users WHERE alias = @alias");
			command.Parameters.AddWithValue("alias", alias);
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
		public bool RegisterUser(string alias, string password, string publicKey)
		{
			if (!CheckUsersAliasAvalible(alias))
				return false;

			string sql = "INSERT INTO users(alias, pwd_md5, publicKey) VALUES(@alias,@pwd, @key);";
			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.Parameters.AddWithValue("alias", alias);
			command.Parameters.AddWithValue("pwd", password);
			command.Parameters.AddWithValue("key", publicKey);

			//ToDo: Send Unknown error
			ExecuteNonQuerry(command);
			return true;
		}

		/// <summary>
		/// Get the alias of a user from the token 
		/// </summary>
		public string GetAliasFromToken(string token)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT u.alias FROM users u JOIN token t ON u.id = t.userID WHERE t.token = @token;");
			command.Parameters.AddWithValue("token", token);
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
		public string HasUserAToken(string alias)
		{
			//ToDo: sql Prameter
			string sql = @"SELECT token FROM token
						     WHERE userID = (SELECT id FROM users WHERE alias = @alias);";

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.Parameters.AddWithValue("alias", alias);
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
		public bool IsTokenFree(string token)
		{
			NpgsqlCommand command = new NpgsqlCommand("SELECT 1 FROM token WHERE token = @token");
			command.Parameters.AddWithValue("token", token);
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
		public int InsertNewMessage(string fromAlias, string toAlias, DateTime send, string message)
		{
			//ToDo: add parameter
			string sql = @"INSERT INTO messages(from_user, to_user, send, content) 
							VALUES((SELECT id FROM users WHERE alias = @fromAlias), 
							(SELECT id FROM users WHERE alias = @toAlias), 
							@send, 
							@message)";

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.Parameters.AddWithValue("fromAlias", fromAlias);
			command.Parameters.AddWithValue("toAlias", toAlias);
			command.Parameters.AddWithValue("send", NpgsqlDbType.Date, send);
			command.Parameters.AddWithValue("message", message);

			return ExecuteNonQuerry(command);
		}

		/// <summary>
		/// Get all unread messages of a user
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public List<Message> SelectUnreadMessages(string token)
		{
			string sql = @"SELECT uf.alias, ut.alias, m.send, m.content, m.id
						     FROM messages m
							 JOIN users uf
							   ON m.from_user = uf.id
							 JOIN users ut
							   ON m.to_user = ut.id
						    WHERE m.to_user = (SELECT userID FROM token WHERE token = @token)
							  AND m.unread = true;";

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.Parameters.AddWithValue("token", token);

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
		public void AssignToken(string alias, string token)
		{
			//ToDo: sql parameter
			string sql = @"INSERT INTO token(token, expire, userID)
						   VALUES(@token, @exp,
						   (SELECT id FROM users WHERE alias = @alias));";

			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.Parameters.AddWithValue("token", token);
			command.Parameters.AddWithValue("exp", NpgsqlDbType.Date, DateTime.Now.AddHours(24));
			command.Parameters.AddWithValue("alias", alias);

			//ToDo: unknown exeption
			ExecuteNonQuerry(command);
		}

		/// <summary>
		/// Select the public key of an user
		/// </summary>
		/// <param name="alias">user alias</param>
		/// <returns>public key as a xml string</returns>
		public string GetUserPublicKey(string alias)
		{
			string sql = "SELECT u.publickey FROM token t JOIN users u ON t.userID = u.id WHERE u.alias = @alias";
			NpgsqlCommand command = new NpgsqlCommand(sql);
			command.Parameters.AddWithValue("alias", alias);
			DataTable data = SelectDataTable(command);

			//ToDo: Send unknown error
			if (data == null)
				return null;

			return data.Rows[0].ItemArray[0].ToString();
		}

		/// <summary>
		/// Select a Query and return a DataTable
		/// </summary>
		private DataTable SelectDataTable(NpgsqlCommand cmd)
		{
			DataTable dt;

			//Connect
			using (NpgsqlConnection con = new NpgsqlConnection(ConnectionString))
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
		private int ExecuteNonQuerry(NpgsqlCommand command)
		{
			int result;

			// create SqlConnection object
			using (NpgsqlConnection con = new NpgsqlConnection(ConnectionString))
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