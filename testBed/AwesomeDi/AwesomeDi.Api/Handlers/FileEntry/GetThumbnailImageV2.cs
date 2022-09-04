using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.FileEntry
{
	public class GetThumbnailImageV2Parameter
	{
		public int? Id { get; set; }
		public bool IsFullSizeThumbnail { get; set; }
	}
	public class GetThumbnailImageV2Result
	{
		public int Id { get; set; }
		public string ThumbnailFilePath { get; set; }
    }
	public class GetThumbnailImageV2
	{
		private readonly _DbContext.AwesomeDiContext _db;

		public GetThumbnailImageV2(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

		public List<KeyValuePair<string, string>> Validate(GetThumbnailImageV2Parameter param)
		{
			var errorList = new List<KeyValuePair<string, string>>();
			if (!_db.FileEntry.Any(x=>x.Id == param.Id)) errorList.Add(new KeyValuePair<string, string>("Id", "Id is invalid"));
			if (string.IsNullOrWhiteSpace(_db.Configuration.FirstOrDefault()?.ThumbnailFolderPath)) errorList.Add(new KeyValuePair<string, string>("ThumbnailPath", "ThumbnailPath did not configured"));
			if (string.IsNullOrWhiteSpace(_db.Configuration.FirstOrDefault()?.DefaultThumbnailFilePath)) errorList.Add(new KeyValuePair<string, string>("DefaultThumbnailFilePath", "DefaultThumbnailFilePath did not configured"));
			return errorList;
		}

		public GetThumbnailImageV2Result Handle(GetThumbnailImageV2Parameter param)
		{
			var config = _db.Configuration.First();
			string thumbnailFilePath = null;
			var fileEntry = _db.FileEntry.First(x => x.Id == param.Id);
            HelperFileEntry.CreateThumbnailIfNotExist(_db, fileEntry, config);
			thumbnailFilePath = HelperThumbnail.GetThumbnailFileName(config.ThumbnailFolderPath, fileEntry.Id.ToString(), param.IsFullSizeThumbnail?HelperThumbnail.VideoThumbnailTypeEnum.Standard:HelperThumbnail.VideoThumbnailTypeEnum.Square);
            if (!File.Exists(thumbnailFilePath)) thumbnailFilePath = config.DefaultThumbnailFilePath;
			var result = new GetThumbnailImageV2Result();
			result.Id = fileEntry.Id;
			result.ThumbnailFilePath= thumbnailFilePath;
			return result;
        }
	}
}
