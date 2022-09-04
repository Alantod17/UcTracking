using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;

namespace AwesomeDi.Api.Helpers
{
	public class HelperFile
	{
		private static readonly string PasswordHash = "Di!7P@@Sw0rd";
		private static readonly string SaltKey = "S@LT&KEYDi!7";
		private static readonly string VIKey = "@1B7c3D5e5F6g7H8";
        public static readonly List<string> VideoExtensions = new List<string> { ".MP4", ".MOV", ".WMV", ".AVI", ".MKV",".MPG",".M4V" };
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPE", ".BMP", ".GIF", ".PNG", ".JPEG" };

        public static bool IsVideoFile(string filePath)
        {
            var fileExt = Path.GetExtension(filePath)?.ToUpper();
            return VideoExtensions.Contains(fileExt);
        }
        public static bool IsImageFile(string filePath)
        {
            var fileExt = Path.GetExtension(filePath)?.ToUpper();
            return ImageExtensions.Contains(fileExt);
        }
		public static void Encrypt(string filePath, string outFilePath)
		{
			//Get input file
			var fsIn = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			//Create output file
			var dir = Path.GetDirectoryName(outFilePath);
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			var fsCrypt = new FileStream(outFilePath, FileMode.Create);
			//Get encryptor ready
			var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC, Padding = PaddingMode.Zeros};
			var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
			//start process
			var buffer = new byte[1048576];
			using (var cryptoStream = new CryptoStream(fsCrypt, encryptor, CryptoStreamMode.Write))
			{
				try
				{
					int read;
					while ((read = fsIn.Read(buffer, 0, buffer.Length)) > 0) cryptoStream.Write(buffer, 0, read);
					cryptoStream.FlushFinalBlock();
				}
				finally
				{
					cryptoStream.Close();
					fsCrypt.Close();
					fsIn.Close();
				}
			}
		}

		public static void Decrypt(string filePath, string outFilePath)
		{
			//Get input file
			var fsIn = new FileStream(filePath, FileMode.Open);
			//Create output file
			var dir = Path.GetDirectoryName(outFilePath);
			if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
			var fsOut = new FileStream(outFilePath, FileMode.Create);
			//Get decryptor ready
			var keyBytes = new Rfc2898DeriveBytes(PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
			var symmetricKey = new RijndaelManaged {Mode = CipherMode.CBC, Padding = PaddingMode.None};
			var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
			//start process
			var buffer = new byte[1048576];
			using (var cryptoStream = new CryptoStream(fsIn, decryptor, CryptoStreamMode.Read))
			{
				try
				{
					int read;
					while ((read = cryptoStream.Read(buffer, 0, buffer.Length)) > 0) fsOut.Write(buffer, 0, read);
				}
				finally
				{
					fsOut.Close();
				}
			}
		}

        public static bool IsFileExistWithoutExtension(string filePath)
        {
            try
            {
                var path = Path.GetDirectoryName(filePath);
                var filename = Path.GetFileNameWithoutExtension(filePath);
                var fileList = Directory.GetFiles(path, $"{filename}*");
                foreach (var file in fileList)
                {
                    var fileInfo = new FileInfo(file);
                    if (fileInfo.Length > 0) return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
		}
    }
}