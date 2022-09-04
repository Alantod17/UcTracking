using System.IO;
using System.Linq;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class ImportDataFromIeeeCsv
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public ImportDataFromIeeeCsv(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }

        public void Handle(string filePath)
        {
            var fileBytes = File.ReadAllBytes(filePath);
            // var dataList = HelperExcel.LoadExcelSpreadsheetUsingGenericType<IeeeData>(fileBytes, Path.GetExtension(filePath));
            // foreach (var data in dataList)
            // {
            //     var ra = _db.ResearchArticle.FirstOrDefault(x => x.Title.ToLower() == data.DocumentTitle.ToLower());
            //     if(ra != null) continue;
            //     ra = new Models.ResearchArticle();
            //     ra.Title = data.DocumentTitle;
            //     ra.Author = data.Authors;
            //     ra.Year = data.PublicationYear;
            //     ra.Isbn = data.ISBNs;
            //     ra.Publisher = data.Publisher;
            //     ra.Address = null;
            //     ra.Doi = data.DOI;
            //     ra.Abstract = data.Abstract;
            //     ra.Booktitle = null;
            //     ra.Pages = $"{data.StartPage}-{data.EndPage}";
            //     ra.PageCount = data.EndPage - data.StartPage;
            //     ra.Numpages = ra.PageCount.ToString();
            //     ra.Keywords = data.AuthorKeywords;
            //     ra.Location = null;
            //     ra.Series = null;
            //     ra.IssueDate = data.IssueDate;
            //     ra.Volume = data.Volume;
            //     ra.Number = null;
            //     ra.Issn = data.ISSN;
            //     ra.Journal = null;
            //     ra.Month = null;
            //     ra.ArticleNo = null;
            //     ra.DataSource = "IEEE";
            //     _db.ResearchArticle.Add(ra);
            //     _db.SaveChanges();
            // }
        }

        private class IeeeData
        {
			public string DocumentTitle { get; set; }
            public string Authors { get; set; }
            public string AuthorAffiliations { get; set; }
            public string PublicationTitle { get; set; }
            public string DateAddedToXplore { get; set; }
            public string PublicationYear { get; set; }
            public string Volume { get; set; }
            public string Issue { get; set; }
            public int StartPage { get; set; }
            public int EndPage { get; set; }
            public string Abstract { get; set; }
            public string ISSN { get; set; }
            public string ISBNs { get; set; }
            public string DOI { get; set; }
            public string FundingInformation { get; set; }
            public string PDFLink { get; set; }
            public string AuthorKeywords { get; set; }
            public string IEEETerms { get; set; }
            public string INSPECControlledTerms { get; set; }
            public string ArticleCitationCount { get; set; }
            public string PatentCitationCount { get; set; }
            public string ReferenceCount { get; set; }
            public string License { get; set; }
            public string OnlineDate { get; set; }
            public string IssueDate { get; set; }
            public string MeetingDate { get; set; }
            public string Publisher { get; set; }
            public string DocumentIdentifier { get; set; }
	}
        


    }
}