using nuntiusModel;
using System;
using System.Collections.Generic;

namespace NuntiusServer
{
	public interface IDbController
	{
		string ConnectionString { get; set; }

		/// <summary>
		/// Check the login data
		/// </summary>
		bool LogInUser(string alias, string password);

		/// <summary>
		/// Check if a alias already exists
		/// </summary>
		bool CheckUsersAliasAvalible(string alias);
		
		/// <summary>
		/// Register a new unique user
		/// </summary>
		bool RegisterUser(string alias, string password, string publicKey);

		/// <summary>
		/// Get the alias of a user from the token 
		/// </summary>
		string GetAliasFromToken(string token);
		
		/// <summary>
		/// Check if a user already has a token
		/// </summary>
		string HasUserAToken(string alias);
		
		/// <summary>
		/// Check if a token is alrady used
		/// </summary>
		/// <param name="token">token to check</param>
		/// <returns>true if the token is not used</returns>
		bool IsTokenFree(string token);

		/// <summary>
		/// Inset a new message in the table
		/// </summary>
		/// <param name="fromAlias">alias from the user who sent the message</param>
		/// <param name="toAlias">alias of the user which get the message</param>
		/// <param name="send">date when the message was sent</param>
		/// <param name="message">message</param>
		/// <returns>1 when the message was inseted sucessfuly else 0</returns>
		int InsertNewMessage(string fromAlias, string toAlias, DateTime send, string message);
		

		/// <summary>
		/// Get all unread messages of a user
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		List<Message> SelectUnreadMessages(string token);
		
		/// <summary>
		/// Assing a new token to a user
		/// </summary>
		void AssignToken(string alias, string token);
		
		/// <summary>
		/// Select the public key of an user
		/// </summary>
		/// <param name="alias">user alias</param>
		/// <returns>public key as a xml string</returns>
		string GetUserPublicKey(string alias);
	}
}