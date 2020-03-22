using nuntiusModel;
using System;
using System.Collections.Generic;

namespace NuntiusServer
{
	public static class RequstHandler
	{
		public static Tuple<Response, string> NewRequest(Request request)
		{
			Tuple<Response, string> r;

			// Check the Request Type
			switch (request.Type)
			{
				case "login":
					r = LoginResponse(request);
					break;

				case "nagg":
					r = NagResponse(request);
					break;

				case "register":
					r = RegisterResponse(request);
					break;

				case "send":
					r = SendResponse(request);
					break;

				default:
					Response response = new Response();
					response.UnknownErrorRespone();
					r = Tuple.Create(response, "");
					break;
			}

			return r;
		}

		/// <summary>
		/// Handel the login request
		/// </summary>
		private static Tuple<Response, string> LoginResponse(Request request)
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

			return Tuple.Create(response, alias);
		}

		/// <summary>
		/// Handel the register request
		/// </summary>
		private static Tuple<Response, string> RegisterResponse(Request request)
		{
			Response response = new Response();

			string alias = request.Parameters[0].ToString();
			string password = request.Parameters[1].ToString();
			string publicKey = request.Parameters[2].ToString();

			//Was the registration successfull?
			string token = UserController.RegisterNewUser(alias, password, publicKey);
			if (token != null)
				response.RegistrationSuccessResponse(token);
			else
				response.RegistrationErrorResponse();

			return Tuple.Create(response, alias);
		}

		/// <summary>
		/// Handel the send request
		/// </summary>
		private static Tuple<Response, string> SendResponse(Request request)
		{
			Response response = new Response();
			string token = request.Parameters[0].ToString();
			string toAlias = request.Parameters[1].ToString();
			DateTime send = Convert.ToDateTime(request.Parameters[2].ToString());
			string message = request.Parameters[3].ToString();

			//Check token
			string fromAlias = DbController.Instance.GetAliasFromToken(token);

			if (fromAlias == null)
			{
				response.UnknownErrorRespone();
				return Tuple.Create(response, fromAlias);
			}
			else if (token == "")
			{
				response.InvalidToken();
				return Tuple.Create(response, fromAlias);
			}

			//User must exist
			if (DbController.Instance.CheckUsersAliasAvalible(toAlias))
			{
				response.SendErrorResponse();
				return Tuple.Create(response, fromAlias);
			}

			//Save messages
			int result = DbController.Instance.InsertNewMessage(fromAlias, toAlias, send, message);

			if (result == 0)
			{
				response.SendErrorResponse();
				return Tuple.Create(response, fromAlias);
			}

			//succsess
			response.SendSuccessResponse();

			return Tuple.Create(response, fromAlias);
		}

		/// <summary>
		/// Send unread messages
		/// </summary>
		private static Tuple<Response, string> NagResponse(Request request)
		{
			Response response = new Response();

			string token = request.Parameters[0].ToString();

			//Get the unread messages
			List<Message> newMessages = DbController.Instance.SelectUnreadMessages(token);

			//Send the messages
			response.ParentResponse(newMessages);

			return Tuple.Create(response, DbController.Instance.GetAliasFromToken(token));
		}
}
}