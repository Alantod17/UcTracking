using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwesomeDi.Api.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace AwesomeDi.Api.Handlers.FileEntry
{
    public class GetFileEntryParameter
    {
        public int? Id { get; set; }
    }
    public class GetFileEntryResult
    {
        public int Id { get; set; }
        public string FileMimeType { get; set; }
        public int? FileHeight { get; set; }
        public int? FileWidth { get; set; }
        // public string FileBase64 { get; set; }
        public DateTime LastWriteUtcDateTime { get; set; }
        public string Extension { get; set; }
        public string FilePath { get; set; }
    }

    public class GetFileEntry
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public GetFileEntry(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(GetFileEntryParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            if (!_db.FileEntry.Any(x => x.Id == param.Id)) errorList.Add(new KeyValuePair<string, string>("Id", "Id is invalid"));
            return errorList;
        }

        public GetFileEntryResult Handle(GetFileEntryParameter param)
        {
            var fileEntry = _db.FileEntry.Find(param.Id);
            new FileExtensionContentTypeProvider().TryGetContentType(fileEntry.FilePath, out var mimeType);
            var res = new GetFileEntryResult
            {
                LastWriteUtcDateTime = fileEntry.LastWriteUtcDateTime,
                FileMimeType = mimeType,
                FilePath = fileEntry.FilePath,
                Id = fileEntry.Id,
                FileWidth = fileEntry.Width,
                FileHeight = fileEntry.Height,
                Extension = fileEntry.Extension,
                // FileBase64 = Convert.ToBase64String(File.ReadAllBytes(fileEntry.FilePath!))
            };
            return res;
        }
    }
}
