using System;
using System.ComponentModel.DataAnnotations;

namespace AwesomeDi.Api.Models
{
	public class Configuration : DatedEntity
	{
		[Key] public int Id { get; set; }
		public string ThumbnailFolderPath { get; set; }
		public string DefaultThumbnailFilePath { get; set; }
	}
}
