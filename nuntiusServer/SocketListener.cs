using nuntiusModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using nuntiusModel;
using RSAEncryption;

namespace NuntiusServer
{
	internal class SocketListener
	{
		private Socket listenSocket;
		private List<Socket> clientSockets;
		private IPEndPoint listenEndPoint;
		byte[] buffer;
		Encryption e;
		Encryption ec;

		public SocketListener()
		{
			//listenSocket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
			//listenSocket.SetSocketOption(SocketOptionLevel.IPv6, SocketOptionName.IPv6Only, false);
			//listenEndPoint = new IPEndPoint(IPAddress.IPv6Any, 11000);
			e = new Encryption();
			e.PrivateKey = "<RSAKeyValue><Modulus>nyY7GtgnDvu0w1AJvbrLo+R7f5lRjnZsASBMfZcYGW6SXwprMtW8E+U312oXA26fl8WLoW9U/AvbYKfecm2Kdn911o6dAwT6FS0CHFQueaZF+5g5hf3SB/qBvvA/suFibAjsHkfe2ssL8j+q9x0j4axG0dBBVKOKXu/B2eePDongvRTIUgNReQ1xYWR+MYAYOqiRMstV0eVvpynUTrW8WQ3GEoL+SunpgAIkJ+1GWQje65GoEWE9TFZSWS3RkBZf2wOHPITpY7j87m+oOO8AxLOtvptNb9u0tNkPQTCu10prtFB1NuJBzn3p7IepNRd2EbawQQ8HrnvW05ksjdaeOXrnugJXXKLwScGPg0VhdjB+/aCG1n/81CGIFEL4jx4GvV+ydpgJ+VyKSqLrr62oo4rZofM7Ye3hgo4YJ6fD6V6qvswAvOY+Y36DB4juOZns9qcaBYymJaSvgPAbfkcdNvuamTOk7DDtQ1SKRw4WpDlICB16RRLeJ65cMStNoViSQsXKkflKqoDusq5cKpa0/Wp5IZk7wlSZl0mT4tS0wEuMT5a8Ob/mGcy4uqxM41/V2Cz1ONHiqlVjKsJ8v7LjebK7sIZ2qSb7wM/E2p9JNujm55+QtWODRq5b82bLnHa8oujKNXs3jmbrwfN0t0SbOD3zJ/Qkl2hyoVm8GVYyLnk=</Modulus><Exponent>AQAB</Exponent><P>zNmJmRgNmlc1jugauSbwQ8OJs0JcixJUFoqMeEPRcwPd+C/uUbMHJAE5QgplbSWusmb8dk3fOCxe2zA6R+AAZ7h1QZh/QDdHsncJW57A0mSYbCbx5JjFlq4Zw+Qtm8n6x5gsCY9GAgi8qKnJ9tJ+6CXQJVmlYIPz/TRQ2xiuImcMz4/+/j9k9qzAPsE9q0+Xclkfr+dZR6n5+zJFoZx1hQfeUiNrRJKiNhQIYnOLHiRXPJ82zskPUdX4iY63lXKUxe1NwpmUFJtyCGvKgHX4+zHhpOIGShG7YOkcrLXRrPTImhgJzNIFer8jknEQ6ja5bMEtMSbncYRJ5jkYio+yiQ==</P><Q>xuNrNR0N3CdPuYofxgTc1sE6ACjeuljah/j2sCJgHiGm3RjRZqE1xh6POCf8IlnYK0sA6lIayEq2rgGkkaxhduzK6mzxACHab3PJ3TH3e5QPUkra3ASJnRne0lR8lCgkPJl3mr3rnenzw2qvoRQFNtABC+uG/60XjCOGDVz4tfJxRPDzYqT5elgQlP2TqBAZq3QETqgid+58o6JlxJXZZAwwDAOuy68UscnPPGsbba47LqXCWfMhAsUiqPyfFMcBuAPqd0EuYJ9+I+kay6+cIOOOIB9AWLUTlH4nY26LmoAW+t31ZQlZFDOrIHxPgwLniSHrQbP9b9iOXb8kKwlgcQ==</Q><DP>YbWxqFCMzH2kklGrzX0BRZFTcSnKMpu/eDEF56eRlCbYDbXvGxEl6sqaoSGMV4+N0JLHATcLriOb6zkJ05bJGrAlXfB7Ygn8LOgSkp0apEBPY+b/omYseT76EzIXfyPMdr/i4XxxjC7WriyupQiqd9jxe59S/sJx8uitWJRhGw/EUI0CuAtNAUxNNqG95KTGPEi+kIztTP2Ku4KzK+8RLy8doFg8piy7KshLZ4ptZSc4ZTYcUm4c3JAPSK3Ga4aH6BDU8mmG9H1g2xtvL+8Vn9ufIaKeFV533jXS856WMMVhmf61Tfkvm+jn8sXzp4QK1hEemT9RThgnl6TMS+tW4Q==</DP><DQ>rPfzsMmH2ENIDrCVWX25IfwCGUlyCtZjl4Vp5VzAAvR5TifbVUo2ngrNu6TqnmVMnrooHaEyMabooQREv3wPd4IQJzh/kAHhGrS0hm6CMTNe786x8E7x73MkWf+oKKGQgjNl1Wn+k+N3YChUfd4PetADWP6I9arvo6zxVIXat0IWSELVKKEZZtFX+nteAvedm4LWjCGleHAsbZM6tnEsEtqWnpxFVFb7xHMijAUKYaT5S2RVVi0gNGyF7DlnkIZSdFrTestxLg8HgOulpicfvWqpnD4RlBRG5wmf79WgC8oCMkL+u9QZaHxXA3WZIPNRse2R0B5c9vx7NASby+HnwQ==</DQ><InverseQ>Nzy1Ac7HAp0mEpfTXZuGENcaaWQ3a5JIJ3oRJ+fzkn/eQR9V9UHsXkF1WcnZPw/UtP7v8RP6/t+QHNG4q0c0EBwz/CxpqgW5r9YfSgs/hXkHvIyUSl1piJnmY5mwX1xw/Pp12tYbp3O3Y//XKwmIE3pW2F6UJ2RJ3w7Jg8kWs7o+TW/tEd0CZCp13lkTjk9eSwovE4/FAQfrEhaDX/yUN1SGvIejnNu/t9GJLltxGGIhfrTe+q+RbKs6ayj6yoBplqlMMyj8DClGKQu8t2YlDKXDQv1vXA+qmjKCLeCU5IbkhcuZQwLZApeu1gtjZ+RoOnWOIWjYcoSyiMPb0n9fDA==</InverseQ><D>kGO8Lg+76BEfoDAGlndZ+Hj8PvkYEFuKg78NBsjFmZVDAS1PstBNkfNdpXfgJ0H/U1Br7Ww3u3D6eQf0fva5L+dWPvImq62hQwMRrigF/mKHjcs/LTzqiEiT3GrWL/HiPMgfiR2FqBWd4gw3jXmHq2CE5CjAjQzPJnD5/DvlA7i+CG5JjHbsPRqgKk9lWn2v/dAf4+itFQDHOu9tanfhyR4A/ZyRH+B286d9rCfOskqlDS6aQCJr3c1Xawdx4PsbvnXGBXdE1C/qcl0lT3ovxUaLNKpsKDc6DPX9rBSCHFutuethXQkL0U9Voz41mzCM82c/I9eCDaDXeCS5UnpME8YMfNxe06iCE1Ia6oLB/oUs/88KQ9xwdayXwRpNfsipMYYKgVsX5EAeN7bA4InmnNjQZbSJgFHbQ8u1LorBwJK66XY2JgVBKPrcMGIbUspZfhoBPX6H6h6HQaLjc68NbvIN7PQ/EMomPJDhCCbZZNbcjm3hgmXhPu6qs0V/9ArO9k0u0bgP36R0em/AYFMBmXbad/0TnfnkCwI++AiwGjTPi8TNLvFJAFxH5sWxpUhpK6mdwTQy72+0EsJysOumTrf/lgNTUoskt2YA1p+2bT6JMOc/SKlelHRespG4rU+1GRb+Kr6F//Y/LFr4Lya7qOg9ggO3nFyIvGady5Z9MoE=</D></RSAKeyValue>";
			ec = new Encryption();
			ec.PublicKey = "<RSAKeyValue><Modulus>pj36f6IXX+1MrJtXTx6JIPxMPpu5iGEsQBSMls5KdwfLGONJ131vvwPTTOl+IL3iYvF5inWb09L1mGGMlCOGQdkUxDlsZt0/NdGXlm0Rw1O91ozCQijcTiU5gkAOPOsTV4yF3b4dZPUS0D0jaoxKRCtMZDqToKWMka1KEsHeOMPR2X3N2Pve+cLFj1ASDHMntAf13mRE5FOOL8E+yrkWQMDtPhysmMej+pga98fEMIvIYiNiIuvMdNbA7itS9ZcJFDrw8pWOmTjO5EZMMXp0fXO4msAPKWPiHfk8VaLIjDxNR9zEM4Q/DbcGVzm3qYA+ji8dxkImz4ZqWkG5uZmPLo0M/y07eKc3YkmFuMnlvCLXlEu6MvsRCbgxtlp6xKUobYc/Jk/rAonet15xmu4UxQdEwGjFWiQ9/siPVhhO234tq0XShudCe27z8TnNFFmU8Dr4j4/wleMajnshlIOiUckEGQlPaipYohO/xhhQMYc+e+XuglC5zfKmZ6xQu2NfgACXyPhqZ5wjJrEEP/Tvs9Q5ucZ2qsmZDnBeVSdSI7ZIHqP+S7dwgaL92HtGbAGr7zHDg1cKPf+Xagy93cmnQKT0MMAr8gVD6gbATmd4Ke6+3q5WwjObsXjC7o8bf9/Um62mMlOpUcHqy0dekYo/X2mFf+XOiVgDJXq2OOP6gJM=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";

			listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			listenEndPoint = new IPEndPoint(IPAddress.Any, 11000);

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
			listenSocket.Listen(100);
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

			//Parse the request
			try
			{
				int received = socket.EndReceive(ar);
				byte[] dataBuf = new byte[received];
				Array.Copy(buffer, dataBuf, received);

				string text = e.DecryptString(dataBuf);
				Console.WriteLine($"Request:  {text}");

				//React to the request
				Request request = JsonSerializer.Deserialize<Request>(text);
				Response response = RequstHandler.NewRequest(request);

				//Send the awnser
				string json = JsonSerializer.Serialize(response);
				System.Console.WriteLine($"Response: {json}");
				byte[] data = ec.EncryptString(json);
				socket.BeginSend(data, 0, data.Length, SocketFlags.None, new AsyncCallback(SendCallback), socket);
			}
			catch (Exception e)
			{
				//ToDo: write in logfile
				System.Console.WriteLine(e);
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

