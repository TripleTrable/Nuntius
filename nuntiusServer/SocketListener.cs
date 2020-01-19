using System;
using System.Text.Json;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using nuntiusModel;

namespace NuntiusServer
{
	class SocketListener
	{
		private Socket listenSocket;
		private List<Socket> clientSockets;
		private IPEndPoint listenEndPoint;
		byte[] buffer;

		public SocketListener()
		{
			listenSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
			listenSocket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
			listenEndPoint = new IPEndPoint(IPAddress.IPv6Any, 11000);
			clientSockets = new List<Socket>();
			buffer = new byte[1024];
		}

		/// <summary>
		/// Start listening for requests
		/// and handel them
		/// </summary>
		public void StartListening()
		{
			Console.WriteLine("Waiting for connection...");
			listenSocket.Bind(listenEndPoint);
			listenSocket.Listen(10);
			listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
		}

		#region Callbacks
		private void AcceptCallback(IAsyncResult ar)
		{
			Console.WriteLine("Client conneted!");
			Socket socket = listenSocket.EndAccept(ar);
			clientSockets.Add(socket);
			socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
			listenSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);
		}

		private void ReceiveCallback(IAsyncResult ar)
		{
			Socket socket = (Socket)ar.AsyncState;
			int received = socket.EndReceive(ar);
			byte[] dataBuf = new byte[received];
			Array.Copy(buffer, dataBuf, received);

			string text = Encoding.ASCII.GetString(dataBuf);
			Console.WriteLine(text);

			//string response = "";

			//if (text.ToLower() == "time")
			//    response = DateTime.Now.ToLongTimeString();
			//else
			//    response = "Invalid Request";


			//byte[] data = Encoding.ASCII.GetBytes(response);

			Request request = JsonSerializer.Deserialize<Request>(text);

			if (request.Type == "register")
			{
                Response response = new Response();
                response.RegistrationErrorResponse();
                string json = JsonSerializer.Serialize(response);

                byte[] data = Encoding.ASCII.GetBytes(json);
				socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
			}


		}

		private void SendCallback(IAsyncResult ar)
		{
			Socket socket = (Socket)ar.AsyncState;
			socket.EndSend(ar);
		}

		#endregion
	}
}
