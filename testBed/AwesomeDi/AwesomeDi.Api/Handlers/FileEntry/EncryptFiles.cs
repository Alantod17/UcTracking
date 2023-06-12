using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AwesomeDi.Api.Handlers.FileEntry
{
    public class EncryptFilesParameter
    {
        public string PathToEncrypt { get; set; }
        public string OutputPath { get; set; }
        public bool IncludeHiddenFile { get; set; }
        public string ZipFileName { get; set; }
        public bool ZipFileByDate { get; set; }
    }
    public class EncryptFiles
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        
        public EncryptFiles(IServiceProvider serviceProvider, ILogger logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }
        public List<KeyValuePair<string, string>> Validate(EncryptFilesParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            if (!Directory.Exists(param.PathToEncrypt)) errorList.Add(new KeyValuePair<string, string>("PathToEncrypt", "Path does not exist"));
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

        public void Handle(EncryptFilesParameter param)
        {
            //Get all files for process
            var filePathList = new List<string>();
            // var dirPathList = new List<string>();
            if (Directory.Exists(param.PathToEncrypt))
            {
                filePathList.AddRange(Directory.GetFiles(param.PathToEncrypt, "*", SearchOption.AllDirectories));
                // dirPathList.AddRange(Directory.GetDirectories(param.PathToEncrypt,"*", SearchOption.AllDirectories));
            }
            //get output direct
            var outputDir = param.OutputPath;
            if (string.IsNullOrEmpty(param.OutputPath))
            {
                if (File.Exists(param.PathToEncrypt))
                {
                    outputDir = Path.GetDirectoryName(param.PathToEncrypt);
                }
                else if (Directory.Exists(param.PathToEncrypt))
                {
                    outputDir = param.PathToEncrypt;
                }
            }
            if (string.IsNullOrEmpty(outputDir)) throw new Exception("output path is invalid");
            //start process file 
            var oldPath = param.PathToEncrypt;
            if (File.Exists(param.PathToEncrypt)) oldPath = Path.GetDirectoryName(param.PathToEncrypt);
            using var scope = _serviceProvider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<_DbContext.AwesomeDiContext>();

            
            SyncFileEntries(param, filePathList, db);

            var fileToEncryptList = db.FileEntry.Where(x => x.FileEntryEncryptionLogList.All(l => l.FileLastWriteUtcDateTime != x.LastWriteUtcDateTime)).ToList();
            foreach (var fileEntry in fileToEncryptList)
            {
	            if (File.Exists(fileEntry.FilePath))
	            {
		            var finalFileName = fileEntry.FilePath.Replace(oldPath, outputDir) + ".AwesomeDi";
		            HelperFile.Encrypt(fileEntry.FilePath, finalFileName);
                    fileEntry.FileEntryEncryptionLogList.Add(new FileEntryEncryptionLog{FileLastWriteUtcDateTime = fileEntry.LastWriteUtcDateTime, EncryptedFilePath = finalFileName});
	            }
	            else
	            {
		            db.FileEntry.Remove(fileEntry);
	            }
	            db.SaveChanges();
            }
        }

        private void SyncFileEntries(EncryptFilesParameter param, List<string> filePathList, _DbContext.AwesomeDiContext db)
        {
            var config = db.Configuration.First();
            var removeList = new List<Models.FileEntry>();
	        foreach (var fileEntry in db.FileEntry.ToList())
	        {
		        if(!File.Exists(fileEntry.FilePath)) 
			        removeList.Add(fileEntry);
	        }
            db.FileEntry.RemoveRange(removeList);
            db.SaveChanges();
            var taskList = new List<Task>();
            foreach (var filePath in filePathList)
            {
                var task = Task.Run(() =>
                {
                    try
                    {
                        using var taskScope = _serviceProvider.CreateScope();
                        var taskDb = taskScope.ServiceProvider.GetRequiredService<_DbContext.AwesomeDiContext>();
                        var fInfo = new FileInfo(filePath);
                        if (fInfo.Attributes.HasFlag(FileAttributes.Hidden) && !param.IncludeHiddenFile) return;
                        var fileEntry = taskDb.FileEntry.FirstOrDefault(x => x.FilePath == filePath);
                        if (fileEntry == null)
                        {
                            fileEntry = new Models.FileEntry {FilePath = filePath};
                            taskDb.FileEntry.Add(fileEntry);
                            taskDb.SaveChanges();
                        }

                        fileEntry.LastWriteUtcDateTime = fInfo.LastWriteTimeUtc;
                        fileEntry.Size = (int?) fInfo.Length;
                        fileEntry.Extension = Path.GetExtension(fileEntry.FilePath)?.ToUpper();
                        taskDb.SaveChanges();
                        HelperFileEntry.CreateThumbnailIfNotExist(taskDb, fileEntry, config);
                        taskDb.SaveChanges();
                        Console.WriteLine($"FileId {fileEntry.Id} - Thread: {Thread.CurrentThread.ManagedThreadId}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Log(LogLevel.Error,ex,$"Fail to process {filePath}");
                    }
                });
                taskList.Add(task);
                if (taskList.Count > 100)
                {
                    Task.WaitAll(taskList.ToArray());
                    taskList.Clear();
                }
            }

            Task.WaitAll(taskList.ToArray());
            Console.WriteLine("SyncFileEntries Done");
        }
    }
}
