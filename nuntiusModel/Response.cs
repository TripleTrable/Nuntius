using System;
using System.Collections.Generic;

namespace nuntiusModel
{
	public class Response
	{
		protected string type;
		protected object[] parameters;

		#region SetResponseType

		public void LoginSuccessResponse(Token token)
		{
			type = "loginSuccess";
			parameters = new object[1];

			parameters[0] = token;
		}

		public void LoginErrorResponse(string error)
		{
			type = "loginError";
			parameters = new object[1];

			parameters[0] = error;
		}

		public void RegistrationSuccessResponse(Token token)
		{
			type = "registationSuccess";
			parameters = new object[1];

			parameters[0] = token;
		}

		public void RegistrationErrorResponse()
		{
			type = "registationError";
			parameters = null;
		}

		public void SendSuccessResponse()
		{
			type = "sendSuccess";
			parameters = null;
		}

        //ToDo: Überlegen die das sinnvoll gelöst werden kann
		public void ParentResponse(List<Message> messages)
		{
			type = "parent";
			parameters = new object[1];

			parameters[0] = messages;
		}

		public void UnknownErrorRespone()
		{
			 type="unknownError";
			 parameters = null;
		}

		#endregion

		#region Properties
		public string Type
		{
			get { return type; }
			set { type = value; }
		}

		public object[] Parameters
		{
			get { return parameters; }
			set { parameters = value; }
		}
		#endregion
	}
}