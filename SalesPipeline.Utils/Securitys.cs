using NPOI.POIFS.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SalesPipeline.Utils
{
	public class Securitys
	{
		private static byte[] _sellTokenIV = new byte[] { 115, 31, 49, 175, 126, 221, 141, 16, 91, 181, 203, 40, 198, 201, 123, 10 };
		private static byte[] _sellTokenKey = new byte[] { 93, 239, 60, 28, 109, 32, 146, 175, 187, 25, 203, 70, 231, 8, 11, 253, 159, 150, 55, 143, 190, 65, 103, 46, 48, 150, 204, 67, 23, 110, 81, 79 };

		public static string key { get; set; } = "AESSalesPipeline123456789!";

		public static string EncryptAES(string text)
		{
			string _data = Convert.ToString(text);
			byte[] encrypted = null;
			AesCryptoServiceProvider csp = new AesCryptoServiceProvider();
			csp.IV = _sellTokenIV;
			csp.Key = _sellTokenKey;
			csp.Padding = PaddingMode.ISO10126;
			csp.Mode = CipherMode.CBC;
			//csp.KeySize = 256;
			using (var encryptor = csp.CreateEncryptor(csp.Key, csp.IV))
			{
				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
						{
							swEncrypt.Write(_data);
						}
					}
					encrypted = msEncrypt.ToArray();
				}
			}
			return Base64UrlEncode(encrypted);
		}

		public static string DecryptAES(string text)
		{
			string decrypted_data = null;

			AesCryptoServiceProvider csp = new AesCryptoServiceProvider();
			csp.IV = _sellTokenIV;
			csp.Key = _sellTokenKey;
			csp.Padding = PaddingMode.ISO10126;
			csp.Mode = CipherMode.CBC;
			//csp.KeySize = 256;
			using (var decryptor = csp.CreateDecryptor(_sellTokenKey, _sellTokenIV))
			{
				using (MemoryStream msDecrypt = new MemoryStream(Base64UrlDecode(text)))
				{
					using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
					{
						using (StreamReader srDecrypt = new StreamReader(csDecrypt))
						{
							decrypted_data = srDecrypt.ReadToEnd();
						}
					}
				}
			}

			return decrypted_data;
		}

		public static string Base64UrlEncode(byte[] arg)
		{
			string s = Convert.ToBase64String(arg);
			s = s.Split('=')[0];
			s = s.Replace('+', '-');
			s = s.Replace('/', '_');
			return s;
		}

		public static byte[] Base64UrlDecode(string arg)
		{
			string s = arg;
			s = s.Replace('-', '+');
			s = s.Replace('_', '/');
			switch (s.Length % 4)
			{
				case 0: break;
				case 2: s += "=="; break;
				case 3: s += "="; break;
				default:
					throw new System.Exception(
			 "Illegal base64url string!");
			}
			return Convert.FromBase64String(s);
		}

		public static string MD5Encrypt(string text)
		{
			using (var md5 = new MD5CryptoServiceProvider())
			{
				using (var tdes = new TripleDESCryptoServiceProvider())
				{
					tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
					tdes.Mode = CipherMode.ECB;
					tdes.Padding = PaddingMode.PKCS7;

					using (var transform = tdes.CreateEncryptor())
					{
						byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
						byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
						return Convert.ToBase64String(bytes, 0, bytes.Length);
					}
				}
			}
		}

		public static string MD5Decrypt(string cipher)
		{
			using (var md5 = new MD5CryptoServiceProvider())
			{
				using (var tdes = new TripleDESCryptoServiceProvider())
				{
					tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
					tdes.Mode = CipherMode.ECB;
					tdes.Padding = PaddingMode.PKCS7;

					using (var transform = tdes.CreateDecryptor())
					{
						byte[] cipherBytes = Convert.FromBase64String(cipher);
						byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
						return UTF8Encoding.UTF8.GetString(bytes);
					}
				}
			}
		}

		public static string Base64StringEncode(string plainText)
		{
			var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
			return System.Convert.ToBase64String(plainTextBytes);
		}

		public static string Base64StringDecode(string base64EncodedData)
		{
			var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
			return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
		}

	}
}
