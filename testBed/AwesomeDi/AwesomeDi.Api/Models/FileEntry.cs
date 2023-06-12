using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AwesomeDi.Api.Models
{
	public class FileEntry: DatedEntity
	{
		public FileEntry()
		{
			FileEntryEncryptionLogList = new List<FileEntryEncryptionLog>();
		}
		[Key] public int Id { get; set; }
		[Required] public string FilePath { get; set; }
		[Required] public DateTime LastWriteUtcDateTime { get; set; }
        public string Extension { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }

		[InverseProperty("FileEntry")]
		public List<FileEntryEncryptionLog> FileEntryEncryptionLogList { get; set; }

        public int? Size { get; set; }
    }
}
