using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeDi.Api.Helpers
{
	public class HelperString
	{
        private static readonly string PasswordHash = "Di!7P@@Sw0rd";
        private static readonly string SaltKey = "S@LT&KEYDi!7";
        private static readonly string VIKey = "@1B7c3D5e5F6g7H8";


        public static string Encrypt(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;
            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    var bufSize = 1048576;
                    var totalCount = plainTextBytes.Length;
                    for (int i = 0; i < totalCount; i += bufSize)
                    {
                        var count = totalCount - i > bufSize ? bufSize : totalCount - i;
                        cryptoStream.Write(plainTextBytes, i, count);
                    }
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }

                memoryStream.Close();
            }

            return Convert.ToBase64String(cipherTextBytes);
        }


        public static string Decrypt(string encryptedText)
        {
            var cipherTextBytes = Convert.FromBase64String(encryptedText);
            var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            var plainTextBytes = new byte[cipherTextBytes.Length];

            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }

        public static string EnsureStringEndWith(string stringToCheck, char characterToCheck)
        {
            return stringToCheck.TrimEnd(characterToCheck) + characterToCheck;
        }


        public static string GetStringParts(string targetString, char splitCharacter, int takeCount, bool endWithSplitCharacter = false, bool reverseTake = false)
        {
            var stringList = targetString.Split(splitCharacter).ToList();
            if (reverseTake) stringList = stringList.Skip(Math.Max(0, stringList.Count() - takeCount)).ToList();
            stringList = stringList.Take(takeCount).ToList();
            var res = stringList.Aggregate("", (current, str) => current + (str + splitCharacter));
            if (!endWithSplitCharacter) res = res.TrimEnd(splitCharacter);
            return res;
        }

        public static bool IsValidUrl(string urlStr, out Uri url)
        {
            return  Uri.TryCreate(urlStr, UriKind.Absolute, out url) && (url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps);
        }

        public static string GetValidFileName(string input, char? invalidCharReplacer = null)
        {
            IList<char> invalidFileNameChars = Path.GetInvalidFileNameChars();
            if (invalidCharReplacer == null)
            {
                return new string(input.Where(ch => !invalidFileNameChars.Contains(ch)).ToArray());
            }
            else
            {
                return new string(input.Select(ch => invalidFileNameChars.Contains(ch) ? (char)invalidCharReplacer : ch).ToArray());
            }
        }
    }
}
