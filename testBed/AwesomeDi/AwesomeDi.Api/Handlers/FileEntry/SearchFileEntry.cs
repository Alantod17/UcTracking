using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;
using Omu.ValueInjecter;

namespace AwesomeDi.Api.Handlers.FileEntry
{
    public class SearchFileEntryParameter
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
    }
    public class SearchFileEntryResult
    {
        public int Id { get; set; }
        public int? Width { get; set; }
        public int? Height { get; set; }
        public string Extension { get; set; }
        public DateTime LastWriteUtcDateTime { get; set; }
    }

    public class SearchFileEntry
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public SearchFileEntry(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(SearchFileEntryParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            // if (!db.FileEntry.Any(x => x.Id == param.Id)) errorList.Add(new KeyValuePair<string, string>("Id", "Id is invalid"));
            return errorList;
        }

        public List<SearchFileEntryResult> Handle(SearchFileEntryParameter param)
        {
            IQueryable<Models.FileEntry> query = _db.FileEntry;
            
            var queryResList = query.OrderByDescending(x=>x.LastWriteUtcDateTime)
                .Skip(param.Skip ?? 0).Take(param.Take ?? 50)
                .Select(x=> new SearchFileEntryResult
                {
                    Id = x.Id,
                    Height = x.Height,
                    Width = x.Width,
                    Extension = x.Extension,
                    LastWriteUtcDateTime = x.LastWriteUtcDateTime
                })
                .ToList();
           
            return queryResList;
        }
    }
}
