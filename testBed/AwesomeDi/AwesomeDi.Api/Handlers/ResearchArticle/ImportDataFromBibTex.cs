using System.Collections.Generic;
using System.IO;
using System.Linq;
using AwesomeDi.Api.Models;
using imbSCI.BibTex;
using imbSCI.Core.extensions.data;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class ImportDataFromBibTex
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public ImportDataFromBibTex(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        public void Handle()
        {
            var dir = new DirectoryInfo(@"C:\Users\DiWan\Desktop\Papers");
            var fileList = dir.GetFiles("SL-FromMendely.bib").toList();
            // var fileList = new List<FileInfo>();
            // var dir2 = new DirectoryInfo(@"C:\Users\DiWan\Desktop\Papers\SL-Clean");
            // fileList.AddRange(dir2.GetFiles("*.bib").toList());
            foreach (var fileInfo in fileList)
            {
                var bibTexDataFile = new BibTexDataFile();
                bibTexDataFile.Load(fileInfo.FullName);
                var dataTable = bibTexDataFile.ConvertToDataTable();
                var jsonString = JsonConvert.SerializeObject(dataTable);
                var objList = JsonConvert.DeserializeObject<List<Models.ResearchArticle>>(jsonString);
                foreach (var researchArticle in objList)
                {
                    if (string.IsNullOrWhiteSpace(researchArticle.Title)) researchArticle.Title = researchArticle.Booktitle;
                    if (string.IsNullOrWhiteSpace(researchArticle.Title)) continue;
                    researchArticle.DataSource = "SpringerLink";
                    if (researchArticle.Pages.Contains('-'))
                    {
                        var nums = researchArticle.Pages.Split('-');
                        var start = int.Parse(new string(nums[0].Where(char.IsDigit).ToArray()));
                        var end = int.Parse(new string(nums[1].Where(char.IsDigit).ToArray()));
                        researchArticle.PageCount = end - start;
                    }
                    // if (_db.ResearchArticle.Any(x => 
                    //     x.Doi.Equals(researchArticle.Doi) || 
                    //     (x.Title.Equals(researchArticle.Title) && 
                    //      x.Year.Equals(researchArticle.Year)))) 
                    //     continue;
                    _db.ResearchArticle.Add(researchArticle);

                }
            }
            _db.SaveChanges();

        }

        public void FixBibTexFile()
        {
            var dir = new DirectoryInfo(@"C:\Users\DiWan\Desktop\Papers\SL-Original");
            var fileList = dir.GetFiles("*.bib");
            foreach (var fileInfo in fileList)
            {
                var str = System.IO.File.ReadAllText(fileInfo.FullName);
                str = str.Replace("=\"", "={");
                str = str.Replace("\",", "},");
                str = str.Replace("\"", "}");
                str = str.Replace("=", " = ");
                System.IO.File.WriteAllText(fileInfo.FullName.Replace("SL-Original", "SL-Clean"), str);
            }
        }


    }
}