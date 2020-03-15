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
		public static readonly string ServerAddres = "2a02:908:5b0:a480:7286:7d52:53e5:6ce";

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
		public static async Task SendRegisterRequestAsync(string alias, string pwd)
		{
			string hashPwd;

			using (MD5 md5hash = MD5.Create())
			{
				hashPwd = EncryptionMD5.GetMd5Hash(md5hash, pwd);
			}

			Encryption encryption = UserController.CreateRsaKey();

			Console.WriteLine(encryption.PrivateKey);

			Console.WriteLine(encryption.PublicKey);

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

		public static async Task SendLoginRequestAsync(string alias, string pwd)
		{
			string hashPwd;
			using (MD5 md5hash = MD5.Create())
			{
				hashPwd = EncryptionMD5.GetMd5Hash(md5hash, pwd);
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
		}

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
			Encryption e = new Encryption();
			e.PublicKey = "<RSAKeyValue><Modulus>nyY7GtgnDvu0w1AJvbrLo+R7f5lRjnZsASBMfZcYGW6SXwprMtW8E+U312oXA26fl8WLoW9U/AvbYKfecm2Kdn911o6dAwT6FS0CHFQueaZF+5g5hf3SB/qBvvA/suFibAjsHkfe2ssL8j+q9x0j4axG0dBBVKOKXu/B2eePDongvRTIUgNReQ1xYWR+MYAYOqiRMstV0eVvpynUTrW8WQ3GEoL+SunpgAIkJ+1GWQje65GoEWE9TFZSWS3RkBZf2wOHPITpY7j87m+oOO8AxLOtvptNb9u0tNkPQTCu10prtFB1NuJBzn3p7IepNRd2EbawQQ8HrnvW05ksjdaeOXrnugJXXKLwScGPg0VhdjB+/aCG1n/81CGIFEL4jx4GvV+ydpgJ+VyKSqLrr62oo4rZofM7Ye3hgo4YJ6fD6V6qvswAvOY+Y36DB4juOZns9qcaBYymJaSvgPAbfkcdNvuamTOk7DDtQ1SKRw4WpDlICB16RRLeJ65cMStNoViSQsXKkflKqoDusq5cKpa0/Wp5IZk7wlSZl0mT4tS0wEuMT5a8Ob/mGcy4uqxM41/V2Cz1ONHiqlVjKsJ8v7LjebK7sIZ2qSb7wM/E2p9JNujm55+QtWODRq5b82bLnHa8oujKNXs3jmbrwfN0t0SbOD3zJ/Qkl2hyoVm8GVYyLnk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
			Encryption ec = new Encryption();
			ec.PrivateKey = "<RSAKeyValue><Modulus>pj36f6IXX+1MrJtXTx6JIPxMPpu5iGEsQBSMls5KdwfLGONJ131vvwPTTOl+IL3iYvF5inWb09L1mGGMlCOGQdkUxDlsZt0/NdGXlm0Rw1O91ozCQijcTiU5gkAOPOsTV4yF3b4dZPUS0D0jaoxKRCtMZDqToKWMka1KEsHeOMPR2X3N2Pve+cLFj1ASDHMntAf13mRE5FOOL8E+yrkWQMDtPhysmMej+pga98fEMIvIYiNiIuvMdNbA7itS9ZcJFDrw8pWOmTjO5EZMMXp0fXO4msAPKWPiHfk8VaLIjDxNR9zEM4Q/DbcGVzm3qYA+ji8dxkImz4ZqWkG5uZmPLo0M/y07eKc3YkmFuMnlvCLXlEu6MvsRCbgxtlp6xKUobYc/Jk/rAonet15xmu4UxQdEwGjFWiQ9/siPVhhO234tq0XShudCe27z8TnNFFmU8Dr4j4/wleMajnshlIOiUckEGQlPaipYohO/xhhQMYc+e+XuglC5zfKmZ6xQu2NfgACXyPhqZ5wjJrEEP/Tvs9Q5ucZ2qsmZDnBeVSdSI7ZIHqP+S7dwgaL92HtGbAGr7zHDg1cKPf+Xagy93cmnQKT0MMAr8gVD6gbATmd4Ke6+3q5WwjObsXjC7o8bf9/Um62mMlOpUcHqy0dekYo/X2mFf+XOiVgDJXq2OOP6gJM=</Modulus><Exponent>AQAB</Exponent><P>1i2Ix1ScE4V1OtHs/+RFbzmfxRESILUT87kL1xHPrwI5LAdfS76U+/Qgy1Kpwt7+IEvgQxVpDTsbY5o4I4lVx7HdqVZyGM+c9USFXpSquXZShmsMsTBgigvLJ5/ii9Jd6k6k1Io8wPYWv4sF+Rjyqmox+++jbRqvm0FinDd30k+repXJpPzWXGexAhiSsLzMXTIKgo+jDlOfcVrYCjDGBXExvgWm/fXYZ9ldrurs7Y88zK16tcDmLNcjGhdIFfbj66mSl+N4zzHnCX14BYyn7Sy/Vs6LSbPs0Yfcbd08R7o2CYoq3t0I4Ch3QVY/KT29xlNKE+ASNWBoLV4k1xaSAw==</P><Q>xrQzB28pzc3DULAjsng8rHhPLtWZOgTyCiUXY64D5mOP43h9tx7xnDcG8MMXsiOgXnnfGTPs6vhjYZudCD1hJ+91c4O+ZgNnghw+1Uezrqoxz7QBvH4fztYJa9OUBCsAXt/MuaYtUoFX5oZuYj/zuEgpkf6pDEz2KYqolZ2qscnyrglFPQuAvzr3yqaDNL4N8B7u2tAkeIhj/TI6q3Bab9+8Bbs+Zas1L63OxDXMdmWgub1PoFGaUqFlCQjRb4iTd2Jr5d5snnXuY70QEUmqgfg1MKlVWVeoiT5XmBLE07B0ufZRAFBM3MO2if6vik90VsAgCSNZFmoFEndghsbaMQ==</Q><DP>bVUbQJ35eqGjIV6ufEZcB5ZD8AZx2Y52W/E3kRd/jFg3NTmiNPlZ9nA+GWzeSiMpWPVCyxvv35gvuB2u0L03s9QhUmNVpUK+XhoXdxuvNzOmlwWomf8XGQFCOL0Omc/Or2BsFi9nKh21qRvf9bP9lG7Xi0NeisCXqkqrexF5sjuHTvGN2RJUnyEzo91FbMFEFcMIyHNoilS3zNbR+AnD3F1XRih+gZ/xU7oLb9JvFioLoHRicsvA9Fzyh0whOU0qNk//DFhLgIF57kdFcJfH1XQFTNvtELeviZqwNMk+D2OIWEBW+COfd0ZQ8mKDpZrl+IrmAtVcljDbnWU0qzPTXw==</DP><DQ>B3bxi8/zRBX8xmU3khbOQLRsVRgVH4bcCOGH1WjOILOQzjzSjDSPYwz2J8lMdJa+CY6OgQXgWptmMF6GMa8MnVFzmhlS58Ys90hUW/LLnz4pjOH4Rrp+O2jzBaf3IYj0BJxntnm1rinJwZE/SFWatEuRtDha5WlmvfzFhIldxjwN79fNUBPEjGqMsII835hcMa8XzgSPui91gieG6LXZz+YZzCVSnMMnxljDMb/LpI8A0Ll56k3mfEAv6Xz5UTVSROPfpc0LS5CmmfyaQ+v+ic9nQCK8YBTY8f7zj3T1C/bPC8VjGZ3qpUh87QYyVgHSHwgsq+s2B4+IDT+K0sVZoQ==</DQ><InverseQ>WVg7903qs+HMxcH8v1PK8arfPpV+c+wpDD98wBeAWOMzZ98GUWy+3wqdxJCRYxJ89f7be8lf8izRBQ5e7x1PTBopZ+MVy6rJXMrkNL0qmoO9YapKoayOdr1C06i6aDJytC7EDS+RuVIPdQdLyp74JvEjyOzU412Kcc17O0DC7R5bjjN7TlVdfsZ/8BEDOLq54GM++YqVRkCS7pNOxyQ15n9iZC/0rCr6xeYhpyBQV9JATQycjU7+XYmT8xf4W6RzCpjFeRLSRGmWaNg4MRTa08Y3caBCPqhop2q8r845eYjIb3xvOv9KAr9yIFB0wAdqKxxyFWrzQM5tZCiAbw+4ww==</InverseQ><D>IwCzyMHxWAJX/ZPrqz4Ls4VL7HbPdta3AXsSKwKejTd33PKmbpf7umVszSnwo6Y3j4u2gp/GwrANhBsuBVcBgsXZaIOKvpHhKEzMNSEb91ufghg7IsEFufJ+jF0+YjdA6FMvMIRCHiq33l6xIVMOpBHV0NjAkreoxFePXCmzx5H7kma5u/E0frQy8rzyP+rTNkXi0AOeudCaBJYUyYx1EgdNLKHO+ZA3h6EVNIy8U1hrRbabiCzL7OH2128J1aoKgFb0uq8gykQ607e1fUmCU5PVD8hWqOleB9+dg4512Yks4G+sZAh5WkLqM910PV1CTp+/DiqJj3KG3CAwodKiYdzjzxiJ7XtbUiqvDGqAO1ab55d5c3ogOCm4RScOLomZ+xU3kpCFwU4f96NA4aZrWu+shiUBy8MWJuQNqWakN8ZH6JwfxdnRdmQkefthNn3zfHBAF7+F2JE05nfQon/VgGuvKcdzvIoUusZiSNWX4Bt+4aSFztBafRlCe+xSL9r7FNVriosB9OWJqHgy0+tTOCX+xdyatBkuQq4MSIg1tDCum3zrkKse6Bkt0IUaouQwUqkYUkPQ5fafCgQtctDC685TvJldSO93LqMwlpQNr5kvaCztCuNsZG8v8nrFTLjAcbhrXmJiZKsMjU47nSzFcWJCNzIDCn2aR7gl7cbUQqE=</D></RSAKeyValue>";


			string message = JsonSerializer.Serialize(request);
			try
			{

				IPAddress ipAddress = IPAddress.Parse(ServerAddres);
				//IPAddress ipAddress = IPAddress.Loopback;

				//IPAddress ipAddress = IPAddress.Parse(ServerAddres);
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
					byte[] msg = e.EncryptString(message);

					// Send the data through the socket.    
					int bytesSent = sender.Send(msg);

					// Receive the response from the remote device.    
					int bytesRec = sender.Receive(bytes);

					// string text = Encoding.ASCII.GetString(bytes, 0, bytesRec);
					//System.Console.WriteLine(bytes.Length);
					byte[] b = new byte[bytesRec];

					for (int i = 0; i < bytesRec; i++)
					{
						b[i] = bytes[i];
					}

					string text = ec.DecryptString(b);

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
		public static void DisplayError(Exception exception)
		{
			Xamarin.Forms.Device.BeginInvokeOnMainThread(() =>
			{
				Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Exception", exception.Message, "Ok");
			});
		}
	}
}