using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using nuntiusModel;
using nuntiusClientChat.Controller;


namespace nuntiusClientChat.Controller
{
    public static class ResponseHandler
    {

        public static async Task HandelServerResponseAsync(Response response)
        {
            switch (response.Type)
            {
                 //User is set to LogedIn,Token is set for the User,Nag Starts, Loging Entrys are nulled 
                case "loginSuccess":
                    NetworkController.NagServer();
                    break;
                //User shoud get a visual que that somhing is wrong
                case "loginError":
                    break;
                //User is set to LogedIn,Token is set for the User,Nag Starts, Loging Entrys are nulled 
                case "registationSuccess":
                    NetworkController.NagServer();
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
