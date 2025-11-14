using System.Security.Cryptography;
using System.Text;

namespace SalesPipeline.Utils
{
	public class Securitys
	{
		private static byte[] _sellTokenIV = new byte[] { 115, 31, 49, 175, 126, 221, 141, 16, 91, 181, 203, 40, 198, 201, 123, 10 };
		private static byte[] _sellTokenKey = new byte[] { 93, 239, 60, 28, 109, 32, 146, 175, 187, 25, 203, 70, 231, 8, 11, 253, 159, 150, 55, 143, 190, 65, 103, 46, 48, 150, 204, 67, 23, 110, 81, 79 };

		public static string key { get; set; } = "AESSalesPipeline123456789!";

        public static string EncryptAES(string text)
        {
            var data = Convert.ToString(text);

            using var aes = Aes.Create();
            aes.Key = _sellTokenKey;
            aes.IV = _sellTokenIV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7; // แทน ISO10126 (deprecated)

            using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream();
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var sw = new StreamWriter(cs))
            {
                sw.Write(data);
            }

            return Base64UrlEncode(ms.ToArray());
        }

        public static string DecryptAES(string text)
        {
            using var aes = Aes.Create();
            aes.Key = _sellTokenKey;
            aes.IV = _sellTokenIV;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7; // ปลอดภัยและรองรับข้อมูลเดิม

            var cipherBytes = Base64UrlDecode(text);

            using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
            using var ms = new MemoryStream(cipherBytes);
            using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
            using var sr = new StreamReader(cs);

            return sr.ReadToEnd();
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
					throw new ArgumentException(
			 "Illegal base64url string!");
			}
			return Convert.FromBase64String(s);
		}

        public static string MD5Encrypt(string text)
        {
            using var sha = SHA256.Create();
            using var aes = Aes.Create();

            aes.Key = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            var iv = aes.IV;
            var encryptor = aes.CreateEncryptor();

            var plainBytes = Encoding.UTF8.GetBytes(text);
            var cipherBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            // รวม IV + Ciphertext
            var result = new byte[iv.Length + cipherBytes.Length];
            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
            Buffer.BlockCopy(cipherBytes, 0, result, iv.Length, cipherBytes.Length);

            return Convert.ToBase64String(result);
        }


        public static string MD5Decrypt(string cipher)
        {
            using var sha = SHA256.Create();
            using var aes = Aes.Create();

            aes.Key = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            var fullCipher = Convert.FromBase64String(cipher);

            // แยก IV (16 bytes) ออกจาก ciphertext
            var iv = new byte[16];
            var cipherBytes = new byte[fullCipher.Length - 16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipherBytes, 0, cipherBytes.Length);

            aes.IV = iv;

            var decryptor = aes.CreateDecryptor();
            var plainBytes = decryptor.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);

            return Encoding.UTF8.GetString(plainBytes);
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
