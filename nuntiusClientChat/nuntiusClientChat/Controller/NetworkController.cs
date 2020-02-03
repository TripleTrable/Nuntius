using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;


namespace nuntiusClientChat.Controller
{
    static class NetworkController
    {
        private static Timer nagTimer = new Timer();

        #region NagTimer
        /// <summary>
        /// When user is successfully Logged in the metode shoud be called to start Nag Requests
        /// </summary>
        public static void NagServer()
        {
            nagTimer.Interval = 1000;
            nagTimer.Elapsed += NagTimer_ElapsedAsync;
            nagTimer.Enabled = true;
            nagTimer.AutoReset = true;
            nagTimer.Start();
        }

        private async static void NagTimer_ElapsedAsync(object sender, ElapsedEventArgs e)
        {
            await Task.Run(() => sendNaggRequstAsync());
        }
        #endregion

        public async static Task<bool> SendRegisterRequestAsync(string alias, string pwd)
        {
            string hashPwd;

            using (MD5 md5hash = MD5.Create())
            {
                hashPwd = Encryption.GetMd5Hash(md5hash, pwd);
            }

            Request request = new Request();
            request.RegisterRequest(alias, hashPwd);

            Response r = await SendReqestToServerAsync(request);

            if (r.Type == "registationSuccess")
            {

                UserController.CurrentTocken = r.Parameters[0].ToString();
                UserController.LogedInUser = new User(request.Parameters[0].ToString(), request.Parameters[1].ToString());
                NagServer();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async static Task<bool> SendLoginRequestAsync(string alias, string pwd)
        {
            string hashPwd;
            using (MD5 md5hash = MD5.Create())
            {
                hashPwd = Encryption.GetMd5Hash(md5hash, pwd);
            }

            Request request = new Request();
            request.LoginRequest(alias, hashPwd);

            Response r = await SendReqestToServerAsync(request);

            if (r.Type == "loginSuccess")
            {

                UserController.CurrentTocken = r.Parameters[0].ToString();
                UserController.LogedInUser = new User(request.Parameters[0].ToString(), request.Parameters[1].ToString());
                NagServer();
                return true;
            }
            else
            {
                return false;
            }
        }
        public async static Task sendMsgRequest(string s, string toAlias, DateTime sendTime, string msgText)
        {
            Request request = new Request();
            request.SendRequest(UserController.CurrentTocken, toAlias, sendTime, msgText);

            Response r = await SendReqestToServerAsync(request);

        }
        public async static Task sendNaggRequstAsync()
        {
            Request request = new Request();
            request.NaggRequst(UserController.CurrentTocken);

            Response r = await SendReqestToServerAsync(request);

            //convets the response to a List of Messeges
            string s = r.Parameters[0].ToString();
            List<Message> messages = JsonSerializer.Deserialize<List<Message>>(s);
                      

        }
        //TODP: await implementation
        public static async Task<Response> SendReqestToServerAsync(Request request)
        {
            byte[] bytes = new byte[4096];
            string message = JsonSerializer.Serialize(request);
            try
            {
                // Connect to a Remote server  
                // Get Host IP Address that is used to establish a connection  
                // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
                // If a host has multiple addresses, you will get a list of addresses  
                // IPHostEntry host = Dns.GetHostEntry("localhost");
                // IPAddress ipAddress = host.AddressList[0];
                // IPAddress ipAddress = IPAddress.Parse("2a02:908:5b0:a480:7286:7d52:53e5:6ce");

                IPAddress ipAddress = IPAddress.Parse("10.100.100.15");
                // IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

                // Create a TCP/IP  socket.    
                Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                //sender.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

                // Connect the socket to the remote endpoint. Catch any errors.    
                try
                {
                    // Connect to Remote EndPoint  
                    sender.Connect(remoteEP);

                    // Encode the data string into a byte array.    
                    byte[] msg = Encoding.ASCII.GetBytes(message);

                    // Send the data through the socket.    
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.    
                    int bytesRec = sender.Receive(bytes);

                    string text = Encoding.ASCII.GetString(bytes, 0, bytesRec);

                    //Server Response
                    Response response = JsonSerializer.Deserialize<Response>(text);

                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                    return response;
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                    return null;
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                    return null;
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                    return null;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return null;
            }
        }
    }


}
