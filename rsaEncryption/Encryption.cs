using System;
using System.Text;
using System.Security.Cryptography;

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
			byte[] dataToEncrypt = Encoding.Unicode.GetBytes(text);
			byte[] encryptedData = rsa.Encrypt(dataToEncrypt, RSAEncryptionPadding.OaepSHA512);

			return encryptedData;
		}

		/// <summary>
		/// Decrypt a string with the rsa algorythem
		/// </summary>
		/// <param name="dataToDecrypt">Encrypted data</param>
		/// <returns>Decrypted unicode string</returns>
		public string DecryptString(byte[] dataToDecrypt)
		{
			byte[] decryptedData = rsa.Decrypt(dataToDecrypt, RSAEncryptionPadding.OaepSHA512);

			return Convert.ToBase64String(decryptedData);
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