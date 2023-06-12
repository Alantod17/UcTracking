using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using AwesomeDi.Api.Models;
using Omu.ValueInjecter;

namespace AwesomeDi.Api.Handlers.FileEntry
{
    public class SearchFileEntryGroupParameter
    {
        public DateTime? StartUtcDate { get; set; }
        public DateTime? EndUtcDate { get; set; }
        public string GroupBy { get; set; }

    }
    public class SearchFileEntryGroupResult
    {
        public SearchFileEntryGroupResult()
        {
            FileList = new List<File>();
        }
        public string GroupKey { get; set; }
        public List<File> FileList { get; set; }
        public class File
        {
            public int Id { get; set; }
            public int? Width { get; set; }
            public int? Height { get; set; }
            public string Extension { get; set; }
            public DateTime LastWriteUtcDateTime { get; set; }
        }
    }

    public class SearchFileEntryGroup
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public SearchFileEntryGroup(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(SearchFileEntryGroupParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            return errorList;
        }
        public List<SearchFileEntryGroupResult> Handle(SearchFileEntryGroupParameter param)
        {
            IQueryable<Models.FileEntry> query = _db.FileEntry;
            if (param.StartUtcDate != null)
            {
                query = query.Where(x => x.LastWriteUtcDateTime.Date >= param.StartUtcDate.Value.Date);
            }
            if (param.StartUtcDate != null)
            {
                query = query.Where(x => x.LastWriteUtcDateTime.Date <= param.EndUtcDate.Value.Date);
            }
            if (param.GroupBy?.ToLower() == "year")
            {
                query = query.ToList().GroupBy(x => new {Year = x.LastWriteUtcDateTime.Year, Month = x.LastWriteUtcDateTime.Month})
                    .SelectMany(x=>x.OrderByDescending(f=>f.LastWriteUtcDateTime).Take(2)).AsQueryable();
               
            }
            query = query.OrderByDescending(x => x.LastWriteUtcDateTime);
            var queryList = query.ToList();

            return ProcessFileGroup(queryList, param.GroupBy?.ToLower());
        }
        

        private static List<SearchFileEntryGroupResult> ProcessFileGroup(List<Models.FileEntry> fileList, string groupBy)
        {
            var resultList = new List<SearchFileEntryGroupResult>();

            foreach (var fileEntry in fileList)
            {
                var groupKey = "";
                switch (groupBy)
                {
                    case "year":
                        groupKey = $"{fileEntry.LastWriteUtcDateTime.Year}-01-01";
                        break;
                    case "month":
                        var monthStr = fileEntry.LastWriteUtcDateTime.Month > 9 ? $"{fileEntry.LastWriteUtcDateTime.Month}" : $"0{fileEntry.LastWriteUtcDateTime.Month}";
                        groupKey = $"{fileEntry.LastWriteUtcDateTime.Year}-{monthStr}-01";
                        break;
                    default:
                        var month = fileEntry.LastWriteUtcDateTime.Month > 9 ? $"{fileEntry.LastWriteUtcDateTime.Month}" : $"0{fileEntry.LastWriteUtcDateTime.Month}";
                        var day = fileEntry.LastWriteUtcDateTime.Day > 9 ? $"{fileEntry.LastWriteUtcDateTime.Day}" : $"0{fileEntry.LastWriteUtcDateTime.Day}";
                        groupKey = $"{fileEntry.LastWriteUtcDateTime.Year}-{month}-{day}";
                        break;
                }
                var res = resultList.FirstOrDefault(x => x.GroupKey == groupKey);
                if (res == null)
                {
                    res = new SearchFileEntryGroupResult();
                    res.GroupKey = groupKey;
                    resultList.Add(res);
                }
                res.FileList.Add(new SearchFileEntryGroupResult.File
                {
                    Extension = fileEntry.Extension,
                    Height = fileEntry.Height,
                    Width = fileEntry.Width,
                    Id = fileEntry.Id,
                    LastWriteUtcDateTime = fileEntry.LastWriteUtcDateTime
                });
            }

            return resultList;
        }
    }
}
