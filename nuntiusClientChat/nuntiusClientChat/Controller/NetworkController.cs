using nuntiusModel;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
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
		public static bool NagTimerRun { get; set; }
		public static ChatSelectionController selectionController = new ChatSelectionController();

		#region NagTimer
		/// <summary>
		/// When user is successfully Logged in the metode shoud be called to start Nag Requests
		/// </summary>
		public static void ConfigurNagServer()
		{
			nagTimer.Interval = 1000;
			nagTimer.Elapsed += NagTimer_ElapsedAsync;
			nagTimer.Enabled = true;
			nagTimer.AutoReset = true;
			NagTimerRun = true;
			//nagTimer.Start();
		}
		private async static void NagTimer_ElapsedAsync(object sender, ElapsedEventArgs e)
		{
			if (!NagTimerRun)
			{
				return;
			}
			else
			{
				await sendNaggRequstAsync();
			}

		}

		#endregion
		public async static Task SendRegisterRequestAsync(string alias, string pwd)
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
				UserController.LogedInUser = new User(alias, hashPwd);
				ConfigurNagServer();
				
			}
			else
			{
				UserController.CurrentTocken = "";
				UserController.LogedInUser = null;
				
			}
		}

		public async static Task SendLoginRequestAsync(string alias, string pwd)
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
				ConfigurNagServer();

			}
			else
			{
				UserController.CurrentTocken = "";
				UserController.LogedInUser = null;
			}
		}

		public async static Task sendMsgRequest(string toAlias, DateTime sendTime, string msgText)
		{
			
			if (UserController.CurrentTocken == null)
			{
				return;
			}

			UserController.CurrentTocken = UserController.CurrentTocken;

			Request request = new Request();
			request.SendRequest(UserController.CurrentTocken, toAlias, sendTime, msgText);

			Response r = await SendReqestToServerAsync(request);

		}

		public async static Task sendNaggRequstAsync()
		{
			Request request = new Request();
			request.NaggRequst(UserController.CurrentTocken);

			if (UserController.CurrentTocken == null)
			{
				return;
			}

			Response r = await SendReqestToServerAsync(request);

			//convets the response to a List of Messeges
			string s = r.Parameters[0].ToString();
			List<Message> messages = JsonSerializer.Deserialize<List<Message>>(s);

			if (messages != null)
			{
				NagTimerRun = false;
				await Task.Run(() => selectionController.SortMeseges(messages));
			}

		}
		public static async Task<Response> SendReqestToServerAsync(Request request)
		{
			byte[] bytes = new byte[4096];
			string message = JsonSerializer.Serialize(request);
			try
			{

				//IPAddress ipAddress = IPAddress.Parse("10.100.100.15");
				//IPAddress ipAddress = IPAddress.Loopback;

				IPAddress ipAddress = IPAddress.Parse("2a02:908:5b0:a480:7286:7d52:53e5:6ce");
				IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

				// Create a TCP/IP  socket.    
				Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
				sender.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);

				// Connect the socket to the remote endpoint. Catch any errors.    
				try
				{
					// Connect to Remote EndPoint  
					await sender.ConnectAsync(remoteEP);

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

		public static async Task<bool> SendPing()
		{
			var connectivity = CrossConnectivity.Current; bool reachable = false;

			if (!connectivity.IsConnected)
				return false;
			if (await connectivity.IsRemoteReachable("google.de", 80,3000))
			{
				//TODO: Remove set for debug if Nuntius Server is not Online
				reachable = true;
				//If the Nuntius Server is Reachebel
				reachable = await connectivity.IsRemoteReachable("2a02:908:5b0:a480:7286:7d52:53e5:6ce",11000,4000);
			}
			
			return reachable;
		}

	}
}