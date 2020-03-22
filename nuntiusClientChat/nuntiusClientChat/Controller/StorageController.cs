using nuntiusModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Xamarin.Essentials;
using RSAEncryption;

namespace nuntiusClientChat.Controller
{
	public static class StorageController
	{
		private static List<Chat> chats;
		private static readonly ChatSelectionController selectionController = NetworkController.selectionController;
		public static bool Loade = true;

		//static string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp2.txt");
		private static readonly string fileName = Path.Combine(FileSystem.AppDataDirectory, dataFile);
		private const string dataFile = "NuntiusData.txt";


		/// <summary>
		/// Saves the Chat data
		/// </summary>
		/// <param name="data"></param>
		public static void SaveData(object data)
		{
			//Reset saved chats List
			//data = new List<Chat>();

			if (data == null || UserController.LogedInUser == null)
				return;
			string dataFile = "NuntiusData" + UserController.LogedInUser.Alias + ".txt";
			string fileName = Path.Combine(FileSystem.AppDataDirectory, dataFile);

			using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
			{
					BinaryFormatter formatter = new BinaryFormatter();
					formatter.Serialize(fileStream, data);
					fileStream.Close();
			}
			
		}
		/// <summary>
		/// Saves the RSA Key (not in use)
		/// </summary>
		/// <param name="data"></param>
		public static void SaveRsaKeySet(object data)
		{
			/*if (data == null)
				return;
			if (UserController.LogedInUser == null)
				return;

			string dataFile = "NuntiusKey" + UserController.LogedInUser.Alias + ".txt";
			string fileName = Path.Combine(FileSystem.AppDataDirectory, dataFile);

			using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fileStream, data);
				fileStream.Close();
			}*/

		}

		/// <summary>
		/// Loades the Saved Data
		/// </summary>
		public static void LoadeData()
		{
			if (UserController.LogedInUser == null)
			{
				return;
			}

			string dataFile = "NuntiusData" + UserController.LogedInUser.Alias + ".txt";
			string fileName = Path.Combine(FileSystem.AppDataDirectory, dataFile);

			if (Loade)
			{
				LoadeChats();
			}

		}
		/// <summary>
		/// Loades the saved RSA Keys (not in use)
		/// </summary>
		public static void LoadeRsa()
		{
			/*string dataFile = "NuntiusKey" + UserController.LogedInUser.Alias + ".txt";
			string fileName = Path.Combine(FileSystem.AppDataDirectory, dataFile);
			Encryption encryption;

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fileStream;

			if (File.Exists(fileName))
			{
				fileStream = new FileStream(fileName, FileMode.Open);
				encryption = (Encryption)(formatter.Deserialize(fileStream));
			}
			else
			{
				return;
			}

			if (encryption.PrivateKey != "" && encryption.PublicKey != "")
			{
				UserController.UserRsaKeys = encryption;
				fileStream.Close();
			}*/

		}
		/// <summary>
		/// Loades the saved Chat data calls the event to add them to the UI
		/// </summary>
		private static void LoadeChats()
		{
			string dataFile = "NuntiusData" + UserController.LogedInUser.Alias + ".txt";
			string fileName = Path.Combine(FileSystem.AppDataDirectory, dataFile);

			BinaryFormatter formatter = new BinaryFormatter();
			FileStream fileStream;

			if (File.Exists(fileName))
			{
				fileStream = new FileStream(fileName, FileMode.Open);
				chats = (List<Chat>)(formatter.Deserialize(fileStream));
			}
			else
			{
				return;
			}

			if (chats.Count < 0)
			{
				fileStream.Close();
				return;
			}

			NetworkController.NagTimerRun = false;
			if (UserController.LogedInUser == null)
			{
				fileStream.Close();
				return;
			}
			else
			{
				selectionController.AddSavedChat(chats);
			}

			fileStream.Close();
			NetworkController.NagTimerRun = true;
			Loade = false;
		}
	}
}
