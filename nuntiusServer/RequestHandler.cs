using nuntiusModel;
using System;
using System.Collections.Generic;

namespace NuntiusServer
{
	public static class RequstHandler
	{
		public static Response NewRequest(Request request)
		{
			Response response = new Response();

			// Check the Request Type
			switch (request.Type)
			{
				case "login":
					response = LoginResponse(request);
					break;

				case "nagg":
					response = NagResponse(request);
					break;

				case "register":
					response = RegisterResponse(request);
					break;

				case "send":
					response = SendResponse(request);
					break;

				default:
					response.UnknownErrorRespone();
					break;
			}

			return response;
		}

		/// <summary>
		/// Handel the login request
		/// </summary>
		private static Response LoginResponse(Request request)
		{
			Response response = new Response();
			string alias = request.Parameters[0].ToString();
			string password = request.Parameters[1].ToString();

			//Was the login successfull?
			string token = UserController.Login(alias, password);
			if (token != null)
				response.LoginSuccessResponse(token);
			else
				response.LoginErrorResponse("Invalid alias or password");

			return response;
		}

		/// <summary>
		/// Handel the register request
		/// </summary>
		private static Response RegisterResponse(Request request)
		{
			Response response = new Response();

			string alias = request.Parameters[0].ToString();
			string password = request.Parameters[1].ToString();

			//Was the registration successfull?
			string token = UserController.RegisterNewUser(alias, password);
			if (token != null)
				response.RegistrationSuccessResponse(token);
			else
				response.RegistrationErrorResponse();

			return response;
		}

		/// <summary>
		/// Handel the send request
		/// </summary>
		private static Response SendResponse(Request request)
		{
			Response response = new Response();
			string token = request.Parameters[0].ToString();
			string toAlias = request.Parameters[1].ToString();
			DateTime send = Convert.ToDateTime(request.Parameters[2].ToString());
			string message = request.Parameters[3].ToString();

			//Check token
			int? fromUserID = DbController.GetIdFromToken(token);

			if (fromUserID == null)
			{
				response.UnknownErrorRespone();
				return response;
			}
			else if (fromUserID == 0)
			{
				response.InvalidToken();
				return response;
			}

			//User must exist
			if (DbController.CheckUsersAliasAvalible(toAlias))
			{
				response.SendErrorResponse();
				return response;
			}

			//Save messages
			int result = DbController.InsertNewMessage(fromUserID.Value, toAlias, send, message);

			if (result == 0)
			{
				response.SendErrorResponse();
				return response;
			}

			//succsess
			response.SendSuccessResponse();

			return response;
		}

		/// <summary>
		/// Send unread messages
		/// </summary>
		private static Response NagResponse(Request request)
		{
			Response response = new Response();

			string userAlias = request.Parameters[0].ToString();
			int? userID = DbController.GetIdFromToken(userAlias);

			if (userID == null)
			{
				response.UnknownErrorRespone();
				return response;
			}
			else if (userID == 0)
			{
				response.InvalidToken();
				return response;
			}

			//Get the unread messages
			List<Message> newMessages = DbController.SelectUnreadMessages(userID.Value);

			//Send the messages
			response.ParentResponse(newMessages);

			return response;
		}
	}
}