using RSAEncryption;

namespace nuntiusClientChat.Controller
{
	public static class UserController
	{

		//TODO: Prop LogedInUser
		//public static nuntiusModel.User LogedInUser = new nuntiusModel.User();
		public static nuntiusModel.User LogedInUser = null;
		private static string currentToken = null;


		public static Encryption CreateRsaKey()
		{
			if (UserRsaKeys == null)
			{
				 return UserRsaKeys = new Encryption();
			}
			else
			{
				return UserRsaKeys;
			}
		}

		public static string CurrentTocken
		{
			get { return currentToken; }
			set { currentToken = value; }
		}

		public static Encryption UserRsaKeys { get; set; }


	}
}
