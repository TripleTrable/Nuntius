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
using RSAEncryption;


namespace nuntiusClientChat.Controller
{
	internal static class NetworkController
	{
		private static readonly Timer nagTimer = new Timer();

		//public static readonly string ServerAddres = "172.16.13.28";
		//public static readonly string ServerAddres = "10.100.100.15";
		public static string ServerAddres = "2a02:908:5b0:a480:7286:7d52:53e5:6ce";


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
		private static async void NagTimer_ElapsedAsync(object sender, ElapsedEventArgs e)
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
		/// <summary>
		/// Register Request:
		/// Sends the User Name and a Md5 hashed Pwd
		/// </summary>
		/// <param name="alias">User Name</param>
		/// <param name="pwd">Password String</param>
		/// <returns></returns>
		public static async Task SendRegisterRequestAsync(string alias, string pwd)
		{
			string hashPwd;

			//Create a hashed pwd
			using (MD5 md5hash = MD5.Create())
			{
				hashPwd = EncryptionMD5.GetMd5Hash(md5hash, pwd);
			}

			RSAEncryption.Encryption userEncryption = UserController.GetEncryption();


			Request request = new Request();
			request.RegisterRequest(alias, hashPwd, userEncryption.PublicKey);

			//var t = userEncryption.Rsa.KeySize;

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
		/// <summary>
		/// Sends a Login Request 
		/// </summary>
		/// <param name="alias"></param>
		/// <param name="pwd"></param>
		/// <returns></returns>
		public static async Task SendLoginRequestAsync(string alias, string pwd)
		{
			string hashPwd;

			//Create a hashed pwd
			using (MD5 md5hash = MD5.Create())
			{
				hashPwd = EncryptionMD5.GetMd5Hash(md5hash, pwd);
			}
			//StorageController.LoadeRsa();

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
				//debug
				sendAllert = 0;

			}
			else
			{
				UserController.CurrentTocken = "";
				UserController.LogedInUser = null;
			}
		}
		/// <summary>
		/// Sends a Message request 
		/// </summary>
		/// <param name="alias"></param>
		/// <param name="pwd"></param>
		/// <returns></returns>
		public static async Task SendMsgRequest(string toAlias, DateTime sendTime, string msgText)
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
			//debug
			sendAllert = 0b0;
		}
		/// <summary>
		/// Sends a Nag Request when the user has new messages, the server sends the reply with the new messages
		/// </summary>
		/// <returns></returns>
		public static async Task SendNaggRequstAsync()
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

			Encryption serverPubKey = new Encryption();
			serverPubKey.PublicKey = "<RSAKeyValue><Modulus>nyY7GtgnDvu0w1AJvbrLo+R7f5lRjnZsASBMfZcYGW6SXwprMtW8E+U312oXA26fl8WLoW9U/AvbYKfecm2Kdn911o6dAwT6FS0CHFQueaZF+5g5hf3SB/qBvvA/suFibAjsHkfe2ssL8j+q9x0j4axG0dBBVKOKXu/B2eePDongvRTIUgNReQ1xYWR+MYAYOqiRMstV0eVvpynUTrW8WQ3GEoL+SunpgAIkJ+1GWQje65GoEWE9TFZSWS3RkBZf2wOHPITpY7j87m+oOO8AxLOtvptNb9u0tNkPQTCu10prtFB1NuJBzn3p7IepNRd2EbawQQ8HrnvW05ksjdaeOXrnugJXXKLwScGPg0VhdjB+/aCG1n/81CGIFEL4jx4GvV+ydpgJ+VyKSqLrr62oo4rZofM7Ye3hgo4YJ6fD6V6qvswAvOY+Y36DB4juOZns9qcaBYymJaSvgPAbfkcdNvuamTOk7DDtQ1SKRw4WpDlICB16RRLeJ65cMStNoViSQsXKkflKqoDusq5cKpa0/Wp5IZk7wlSZl0mT4tS0wEuMT5a8Ob/mGcy4uqxM41/V2Cz1ONHiqlVjKsJ8v7LjebK7sIZ2qSb7wM/E2p9JNujm55+QtWODRq5b82bLnHa8oujKNXs3jmbrwfN0t0SbOD3zJ/Qkl2hyoVm8GVYyLnk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
			//Converts the request to the JSON format
			string message = JsonSerializer.Serialize(request);

			try
			{
				IPAddress ipAddress = IPAddress.Parse(ServerAddres);

				IPEndPoint remoteEP = new IPEndPoint(ipAddress, 11000);

				// Create a TCP/IP  socket.    
				Socket sender = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

				//Checks is the ip Added is a IPv6
				if (ipAddress.AddressFamily == AddressFamily.InterNetworkV6)
				{
					sender.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
				}

				// Connect the socket to the remote endpoint. Catch any errors.    
				try
				{
					// Connect to Remote EndPoint  
					await sender.ConnectAsync(remoteEP);

					// Encode the data string into a byte array.    
					byte[] msg = serverPubKey.EncryptString(message);

					// Send the data through the socket.    
					int bytesSent = sender.Send(msg);

					// Receive the response from the remote device.    
					int bytesRec = sender.Receive(bytes);

					byte[] b = new byte[bytesRec];

					for (int i = 0; i < bytesRec; i++)
					{
						b[i] = bytes[i];
					}

					string text = UserController.GetEncryption().DecryptString(b);

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
					DisplayError(ane);
					return null;
				}
				catch (SocketException se)
				{
					Console.WriteLine("SocketException : {0}", se.ToString());
					DisplayError(se);
					return null;
				}
				catch (Exception ee)
				{
					Console.WriteLine("Unexpected exception : {0}", ee.ToString());
					DisplayError(ee);
					return null;
				}

			}
			catch (Exception eee)
			{
				Console.WriteLine(eee.ToString());
				DisplayError(eee);
				return null;
			}

		}

		//TODO: Remove Only for debug 
		private static int sendAllert = 0;

		/// <summary>
		/// Converts an exception into a display alert.
		/// </summary>
		/// <param name="exception"></param>
		public static void DisplayError(Exception exception)
		{
			sendAllert++;
			
			if (sendAllert >= 30)
			{
				return;
			}
			else
			{
				Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
				{
					Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Exception", exception.Message, "Ok");
				});
			}
		}
	}
}