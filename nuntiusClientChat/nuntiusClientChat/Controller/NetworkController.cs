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
		private static readonly Timer nagTimer = new Timer();
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
				await SendNaggRequstAsync();
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

			if (r == null)
				return;

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

			if (r == null)
				return;

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

		public async static Task SendMsgRequest(string toAlias, DateTime sendTime, string msgText)
		{

			if (UserController.CurrentTocken == null)
			{
				return;
			}

			UserController.CurrentTocken = UserController.CurrentTocken;

			Request request = new Request();
			request.SendRequest(UserController.CurrentTocken, toAlias, sendTime, msgText);

			Response r = await SendReqestToServerAsync(request);

			if (r == null)
				return;
		}

		public async static Task SendNaggRequstAsync()
		{
			Request request = new Request();
			request.NaggRequst(UserController.CurrentTocken);

			if (UserController.CurrentTocken == null)
			{
				return;
			}

			Response r = await SendReqestToServerAsync(request);

			if (r == null)
				return;
			//convets the response to a List of Messeges
			string s = r.Parameters[0].ToString();
			List<Message> messages = JsonSerializer.Deserialize<List<Message>>(s);

			if (messages != null)
			{
				NagTimerRun = false;
				await Task.Run(() => selectionController.SortMessages(messages));
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
	}
}