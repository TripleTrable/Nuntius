using System;
using System.Text;
using System.Security.Cryptography;

namespace RSAEncryption
{
	public class Encryption
	{
		RSA rsa { get; set; }
		RSAParameters publicKey { get; }
		RSAParameters privateKey { get; }

		public Encryption()
		{
			rsa = RSA.Create(4096);
			publicKey = rsa.ExportParameters(false);
			privateKey = rsa.ExportParameters(true);
		}

		/// <summary>
		/// Export the public key
		/// </summary>
		/// <returns>Public key as a string</returns>
		public string ExportPublicKey()
		{
			return "";
		}

		/// <summary>
		/// Export the private key
		/// </summary>
		/// <returns>Private key as a string</returns>
		public string ExportPrivateKey()
		{
			return "";
		}

		/// <summary>
		/// Import a pair of keys
		/// </summary>
		/// <param name="parameters">The keys</param>
		public void LoadKeys(RSAParameters parameters)
		{
			rsa.ImportParameters(parameters);
		}

		/// <summary>
		/// Load a pair of Keys 
		/// </summary>
		/// <param name="n">modulus (p * q)</param>
		/// <param name="e">public exponent</param>
		/// <param name="d">private exponent</param>
		public void LoadKeys(byte[] n, byte[] e, byte[] d)
		{
			RSAParameters p = new RSAParameters()
			{
				Modulus = n,
				Exponent = e,
				D = d
			};

			LoadKeys(p);
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

			return Encoding.Unicode.GetString(decryptedData);
		}
	}
}