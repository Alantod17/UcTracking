using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.FileEntry
{
	public class GetThumbnailImageParameter
	{
		public int? Id { get; set; }
	}
	public class GetThumbnailImageResult
	{
		public int Id { get; set; }
		public string FileBase64 { get; set; }
        public string Extension { get; set; }
        public int? Height { get; set; }
        public int? Width { get; set; }
    }
	public class GetThumbnailImage
	{
		private readonly _DbContext.AwesomeDiContext _db;

		public GetThumbnailImage(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

		public List<KeyValuePair<string, string>> Validate(GetThumbnailImageParameter param)
		{
			var errorList = new List<KeyValuePair<string, string>>();
			if (!_db.FileEntry.Any(x=>x.Id == param.Id)) errorList.Add(new KeyValuePair<string, string>("Id", "Id is invalid"));
			if (string.IsNullOrWhiteSpace(_db.Configuration.FirstOrDefault()?.ThumbnailFolderPath)) errorList.Add(new KeyValuePair<string, string>("ThumbnailPath", "ThumbnailPath did not configured"));
			if (string.IsNullOrWhiteSpace(_db.Configuration.FirstOrDefault()?.DefaultThumbnailFilePath)) errorList.Add(new KeyValuePair<string, string>("DefaultThumbnailFilePath", "DefaultThumbnailFilePath did not configured"));
			return errorList;
		}

		public GetThumbnailImageResult Handle(GetThumbnailImageParameter param)
		{
			var config = _db.Configuration.First();
			string thumbnailFilePath = null;
			var fileEntry = _db.FileEntry.First(x => x.Id == param.Id);
            HelperFileEntry.CreateThumbnailIfNotExist(_db, fileEntry, config);
			thumbnailFilePath = HelperThumbnail.GetThumbnailFileName(config.ThumbnailFolderPath, fileEntry.Id.ToString(), HelperThumbnail.VideoThumbnailTypeEnum.Square);
            if (!File.Exists(thumbnailFilePath)) thumbnailFilePath = config.DefaultThumbnailFilePath;
            var fileBase64String = Convert.ToBase64String(File.ReadAllBytes(thumbnailFilePath));
			var result = new GetThumbnailImageResult();
			result.Id = fileEntry.Id;
			result.Width = fileEntry.Width;
			result.Height = fileEntry.Height;
			result.Extension = fileEntry.Extension;
			result.FileBase64 = fileBase64String;
			return result;
        }
	}
}
