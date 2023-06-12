using System;
using System.Collections.Generic;
using System.IO;
using AwesomeDi.Api.Helpers;

namespace AwesomeDi.Api.Handlers.FileEntry
{
	public class DecryptFilesParameter
	{
		public string PathToDecrypt { get; set; }
		public string OutputPath { get; set; }
	}
    public class DecryptFiles
	{
        public List<KeyValuePair<string, string>> Validate(DecryptFilesParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            if (!File.Exists(param.PathToDecrypt) && !Directory.Exists(param.PathToDecrypt)) errorList.Add(new KeyValuePair<string, string>("PathToEncrypt", "Path does not exist"));
            if (!string.IsNullOrEmpty(param.OutputPath))
            {
                try
                {
                    FileAttributes attr = File.GetAttributes(param.OutputPath);
                    if (!attr.HasFlag(FileAttributes.Directory))
                        errorList.Add(new KeyValuePair<string, string>("OutputPath",
                            "Path does not point to a directory"));
                }
                catch (Exception)
                {
                    errorList.Add(new KeyValuePair<string, string>("OutputPath",
                        "OutputPath is invalid"));
                }
            }
            return errorList;
        }

        public void Handle(DecryptFilesParameter param)
        {
            var filePathList = new List<string>();
            //Get all files for process
            if (File.Exists(param.PathToDecrypt)) filePathList.Add(param.PathToDecrypt);
            if (Directory.Exists(param.PathToDecrypt))
                filePathList.AddRange(Directory.GetFiles(param.PathToDecrypt, "*", SearchOption.AllDirectories));
            //get output direct
            var outputDir = param.OutputPath;
            if (string.IsNullOrEmpty(param.OutputPath))
            {
                if (File.Exists(param.PathToDecrypt))
                {
                    outputDir = Path.GetDirectoryName(param.PathToDecrypt);
                }
                else if (Directory.Exists(param.PathToDecrypt))
                {
                    outputDir = param.PathToDecrypt;
                }
            }
            if (string.IsNullOrEmpty(outputDir))  throw new Exception("output path is invalid");
            // var decryptFolderName = $"Decrypt-{Guid.NewGuid()}";
            // outputDir = $@"{outputDir}\{decryptFolderName}";
            var oldPath = param.PathToDecrypt;
            if (File.Exists(param.PathToDecrypt)) oldPath = Path.GetDirectoryName(param.PathToDecrypt);
            foreach (var filePath in filePathList)
            {
                var finalFileName = filePath.Substring(0, filePath.LastIndexOf(".AwesomeDi", StringComparison.Ordinal)).Replace(oldPath, outputDir);
                var dir = Path.GetDirectoryName(finalFileName);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                HelperFile.Decrypt(filePath, finalFileName);
            }
        }
    }
}
