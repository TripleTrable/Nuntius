namespace nuntiusClientChat.Controller
{
    public static class UserController
    {
        public static nuntiusModel.User LogedInUser = null;

        private static string currentToken = null;

        public static string CurrentTocken
        {
            get { return currentToken; }
            set { currentToken = value; }
        }


    }
}
