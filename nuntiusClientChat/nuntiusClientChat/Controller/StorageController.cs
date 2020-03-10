using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using nuntiusClientChat.Controller;
using nuntiusClientChat.Controls;
using nuntiusModel;
using System.Runtime.Serialization.Formatters.Binary;
using Xamarin.Essentials;
using System.Threading.Tasks;

namespace nuntiusClientChat.Controller
{
	public static class StorageController
	{
		private static List<Chat> chats;
		private static readonly ChatSelectionController selectionController = NetworkController.selectionController;
		public static bool Loade = true;
		public static bool Loaded = false;
		//static string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "temp2.txt");
		private static readonly string fileName = Path.Combine(FileSystem.AppDataDirectory, dataFile);
		const string dataFile = "NuntiusData.txt";

		public static List<Chat> Chats
		{
			get { return chats; }
			set { chats = selectionController.CurrentChats; }
		}

		public static void SaveData()
		{
			//Reset saved chats List
			//selectionController.CurrentChats = new List<Chat>();

			using (FileStream fileStream = new FileStream(fileName, FileMode.OpenOrCreate))
			{
				BinaryFormatter formatter = new BinaryFormatter();
				formatter.Serialize(fileStream, selectionController.CurrentChats);
				fileStream.Close();
			}

		}

		public static void LoadeData()
		{
			if (Loade)
			{

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
					return;

				foreach (Chat chat in Chats)
				{
					if (chat.Owner == UserController.LogedInUser.Alias)
					{
						NetworkController.NagTimerRun = false;
						selectionController.AddSavedChat(chat);
					}
					else
					{
						return;
					}
				}

				fileStream.Close();
				NetworkController.NagTimerRun = true;
				Loaded = true;
			}
		}

	}
}
