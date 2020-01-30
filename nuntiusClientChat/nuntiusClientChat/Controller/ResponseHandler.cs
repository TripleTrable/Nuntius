using nuntiusModel;
using System.Threading.Tasks;


namespace nuntiusClientChat.Controller
{
    public static class ResponseHandler
    {

        public static async Task HandelServerResponseAsync(Response response, Request request)
        {
            switch (response.Type)
            {
                //User is set to LogedIn,Token is set for the User,Nag Starts, Loging Entrys are nulle, Loade Chat layout
                case "loginSuccess":
                    // NetworkController.NagServer();
                    UserController.CurrentTocken = response.Parameters[0].ToString();
                    UserController.LogedInUser = new User(request.Parameters[0].ToString(), request.Parameters[1].ToString());
                    break;
                //User shoud get a visual que that somhing is wrong
                case "loginError":
                    break;
                //User is set to LogedIn,Token is set for the User,Nag Starts, Loging Entrys are nulled, Loade Chat layout
                case "registationSuccess":
                    //NetworkController.NagServer();
                    UserController.CurrentTocken = response.Parameters[0].ToString();
                    UserController.LogedInUser = new User(request.Parameters[0].ToString(), request.Parameters[1].ToString());
                    break;
                //User shoud get a visual que that somhing is wrong
                case "registationError":
                    break;
                //Msg send Icon set
                case "sendSuccess":
                    break;
                //Chat is updatet with the new Msgs
                case "parent":
                    break;
                case "unknownError":
                    break;
            }
        }


    }
}
