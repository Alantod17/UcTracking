using System;
using System.ComponentModel.DataAnnotations;

namespace AwesomeDi.Api.Models
{
	public class FileEntryEncryptionLog : DatedEntity
	{
		[Key] public int Id { get; set; }
		[Required] public int FileEntryId { get; set; }
		[Required] public  FileEntry FileEntry { get; set; }
		[Required] public DateTime FileLastWriteUtcDateTime { get; set; }
		[Required] public string EncryptedFilePath { get; set; }
	}
}
