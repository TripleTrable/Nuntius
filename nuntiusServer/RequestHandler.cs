using System;
using nuntiusModel;

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
					response.LoginErrorResponse("Not implemented yet");
					break;

				case "nagg":
					response.UnknownErrorRespone();
					break;

				case "register":
					string alias = request.Parameters[0].ToString();
					string password = request.Parameters[1].ToString();

					//Was the registration successfull?
					string token = UserController.RegisterNewUser(alias, password);
					if (token != null)
						response.RegistrationSuccessResponse(token);
					else
						response.RegistrationErrorResponse();
					break;

				case "send":
					response.SendSuccessResponse();
					break;

				default:
					response.UnknownErrorRespone();
					break;
			}

			return response;
		}
	}
}