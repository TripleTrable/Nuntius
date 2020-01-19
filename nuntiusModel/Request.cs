using System;

namespace nuntiusModel
{
	public class Request
	{
		protected string type;
		protected object[] parameters;

		#region SetRequestType
		public void LoginRequest(string alias, string passwd)
		{
			type = "login";
			parameters = new object[2];

			parameters[0] = alias;
			parameters[1] = passwd;
		}

		public void NaggRequst(Token token)
		{
			type = "nagg";
			parameters = new object[1];
			parameters[0] = token;
		}

		public void RegisterRequest(string alias, string passwd)
		{
			type = "register";
			parameters = new object[2];

			parameters[0] = alias;
			parameters[1] = passwd;
		}

		public void SendRequest(Token token, string toAlias, DateTime sent, string text)
		{
			type = "send";
			parameters = new object[4];

			parameters[0] = token;
			parameters[1] = toAlias;
			parameters[2] = sent;
			parameters[3] = text;
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