using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using nuntiusModel;
using Xamarin.Forms;


namespace nuntiusClientChat.Controller
{
    static class NetworkController
    {


        //TODO: sting pwd to mb5;
        public async static Task SendRegisterRequest(string alias, string pwd)
        {
            Request request = new Request();
            request.RegisterRequest(alias, pwd);

            string send = JsonSerializer.Serialize(request);
            await Task.Run(() => SendReqestToServer(send));

        }
        //TODO string pwd to mb5
        public async static Task SendLoginRequest(string alias, string pwd)
        {
            Request request = new Request();
            request.LoginRequest(alias, pwd);

            string send = JsonSerializer.Serialize(request);
            await Task.Run(() => SendReqestToServer(send));
        }
        public async static Task sendMsgRequest(Token token, string toAlias, DateTime sendTime, string msgText)
        {
            Request request = new Request();
            request.SendRequest(token, toAlias, sendTime, msgText);

            string send = JsonSerializer.Serialize(request);
            await Task.Run(() => SendReqestToServer(send));
        }

        public async static Task sendNaggRequst(Token token)
        {
            Request request = new Request();
            request.NaggRequst(token);

            string send = JsonSerializer.Serialize(request);
            await Task.Run(() => SendReqestToServer(send));
        }

        public static void SendReqestToServer(string message)
        {
            byte[] bytes = new byte[4096];

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
                    byte[] msg = Encoding.UTF32.GetBytes(message);

                    // Send the data through the socket.    
                    int bytesSent = sender.Send(msg);

                    // Receive the response from the remote device.    
                    int bytesRec = sender.Receive(bytes);

                    string text = Encoding.UTF32.GetString(bytes, 0, bytesRec);

                    //Server Response
                    Response response = JsonSerializer.Deserialize<Response>(text);



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
