using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using nuntiusModel;
using Xamarin.Forms;
using nuntiusClientChat.Controller;
using System.Security.Cryptography;
using System.Timers;


namespace nuntiusClientChat.Controller
{
    static class NetworkController
    {
        private static Timer nagTimer = new Timer();


        /// <summary>
        /// When user is successfully Logged in the metode shoud be called to start Nag Requests
        /// </summary>
        public static void NagServer()
        {
            nagTimer.Interval = 1000;
            nagTimer.Elapsed += NagTimer_Elapsed;
            nagTimer.Enabled = true;
            nagTimer.AutoReset = true;
            nagTimer.Start();
        }

        private async static void NagTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
           await Task.Run(() => sendNaggRequst("zrtete54ret564"));
        }

        //TODO: sting pwd to mb5;
        public async static Task SendRegisterRequest(string alias, string pwd)
        {
            string hashPwd;

            using (MD5 md5hash = MD5.Create())
            {
                hashPwd = Encryption.GetMd5Hash(md5hash, pwd);
            }


            Request request = new Request();
            request.RegisterRequest(alias, hashPwd);

            string send = JsonSerializer.Serialize(request);
            await Task.Run(() => SendReqestToServerAsync(request));



        }
        //TODO string pwd to mb5
        public async static Task SendLoginRequest(string alias, string pwd)
        {
            string hashPwd;

            using (MD5 md5hash = MD5.Create())
            {
                hashPwd = Encryption.GetMd5Hash(md5hash, pwd);
            }

            Request request = new Request();
            request.LoginRequest(alias, hashPwd);

            string send = JsonSerializer.Serialize(request);
            await Task.Run(() => SendReqestToServerAsync(request));

        }
        public async static Task sendMsgRequest(string s, string toAlias, DateTime sendTime, string msgText)
        {
            Request request = new Request();
            request.SendRequest("wqeqwa14", toAlias, sendTime, msgText);

            string send = JsonSerializer.Serialize(request);
            await Task.Run(() => SendReqestToServerAsync(request));

        }

        public async static Task sendNaggRequst(string s)
        {
            Request request = new Request();
            request.NaggRequst(s);

     
            await Task.Run(() => SendReqestToServerAsync(request));
        }

        public static async Task SendReqestToServerAsync(Request request)
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

                IPAddress ipAddress = IPAddress.Parse("10.100.100.35");
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

                    //Calls the ResponseHandler
                    await ResponseHandler.HandelServerResponseAsync(response,request);
                    
                    // Release the socket.    
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();

                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());

                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine("Unexpected exception : {0}", e.ToString());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }


}
