using System.Linq;
using System.Net;
using System;
using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;

namespace RSAEncryption
{
	public class Encryption
	{
		RSA rsa { get; set; }

		public Encryption()
		{
			rsa = RSA.Create();
			rsa.KeySize = 4096;
		}

		/// <summary>
		/// Encrypt a string with the rsa algorythem
		/// </summary>
		/// <param name="text">String to encrypt</param>
		/// <returns>Encrypted string</returns>
		public byte[] EncryptString(string text)
		{
			List<byte> encryptedData = new List<byte>();

			//Split the text into 200 charkter parts
			do
			{
				string s;

				if (text.Length > 200)
				{
					string part = text.Substring(0, 200);
					text = text.Substring(200);
					s = part;
				}
				else
				{
					s = text;
					text = "";
				}

				encryptedData.AddRange(_EncryptString(s));

			} while (text.Length != 0);

			return encryptedData.ToArray();
		}

		private byte[] _EncryptString(string text)
		{
			byte[] dataToEncrypt = Encoding.Unicode.GetBytes(text);
			byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.Pkcs1);

			return encryptedData;
		}

		/// <summary>
		/// Decrypt a string with the rsa algorythem
		/// </summary>
		/// <param name="dataToDecrypt">Encrypted data</param>
		/// <returns>Decrypted unicode string</returns>
		public string DecryptString(byte[] dataToDecrypt)
		{
			int len = rsa.KeySize / 8;
			string decryptedData = "";
			do
			{
				byte[] data;

				if (dataToDecrypt.Length > len)
				{
					System.Console.WriteLine(dataToDecrypt.Length);
					byte[] b = new byte[len];
					Array.Copy(dataToDecrypt, b, len);
					data = b;
					byte[] buffer = new byte[dataToDecrypt.Length - len];
					Array.Copy(dataToDecrypt, len, buffer, 0, buffer.Length);
					dataToDecrypt = buffer;
				}
				else
				{
					data = dataToDecrypt;
					dataToDecrypt = new byte[0];
				}

				decryptedData += _DecryptString(data);

			} while (dataToDecrypt.Length != 0);

			return decryptedData;
		}

		private string _DecryptString(byte[] dataToDecrypt)
		{
			byte[] decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.Pkcs1);

			return Encoding.Unicode.GetString(decryptedData);
		}

		/// <summary>
		/// Exports / imports the private key
		/// </summary>
		public string PublicKey
		{
			get { return rsa.ToXmlString(false); }
			set { rsa.FromXmlString(value); }
		}

		/// <summary>
		/// Exports / imports the private key
		/// </summary>
		public string PrivateKey
		{
			get { return rsa.ToXmlString(true); }
			set { rsa.FromXmlString(value); }
		}
	}
}