using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Models
{
    public class ResearchArticle : DatedEntity
    {
        [Key] public int Id { get; set; }
        [JsonProperty("EntryType")] public string EntryType { get; set; }

        [JsonProperty("EntryKey")] public string EntryKey { get; set; }

        [JsonProperty("author")] public string Author { get; set; }

        [JsonProperty("title")] public string Title { get; set; }

        [JsonProperty("year")] public string Year { get; set; }

        [JsonProperty("isbn")] public string Isbn { get; set; }

        [JsonProperty("publisher")] public string Publisher { get; set; }

        [JsonProperty("address")] public string Address { get; set; }

        [JsonProperty("url")] public string Url { get; set; }

        [JsonProperty("doi")] public string Doi { get; set; }

        [JsonProperty("abstract")] public string Abstract { get; set; }

        [JsonProperty("booktitle")] public string Booktitle { get; set; }

        [JsonProperty("pages")] public string Pages { get; set; }

        [JsonProperty("numpages")] public string Numpages { get; set; }

        [JsonProperty("keywords")] public string Keywords { get; set; }

        [JsonProperty("location")] public string Location { get; set; }

        [JsonProperty("series")] public string Series { get; set; }

        [JsonProperty("issue_date")] public string IssueDate { get; set; }

        [JsonProperty("volume")] public string Volume { get; set; }

        [JsonProperty("number")] public string Number { get; set; }

        [JsonProperty("issn")] public string Issn { get; set; }

        [JsonProperty("journal")] public string Journal { get; set; }

        [JsonProperty("month")] public string Month { get; set; }

        [JsonProperty("articleno")] public string ArticleNo { get; set; }
        public bool IsDuplicate { get; set; }
        public bool IsIrrelevant { get; set; }
        public string Notes { get; set; }
        public bool IsNeedReview { get; set; }
        public bool IsDeleted { get; set; }
        public string DataSource { get; set; }
        public int? PageCount { get; set; }
        public string DocumentFilePath { get; set; }
        public string ResearchGoal { get; set; }
        public string ResearchMethod { get; set; }
        public string ResearchQuestions { get; set; }
        public string ResearchDataSource { get; set; }
        public string ResearchContext { get; set; }
        public string ResearchedProblemType { get; set; }
        public string ResearchPlatform { get; set; }
        public string ResearchedTechnology { get; set; }
        public string ResearchedApproach { get; set; }
        public string ResearchMainFindings { get; set; }
        public string ResearchContributionType { get; set; }
        public string ResearchFutureWorks { get; set; }
        public string ResearchChallenges { get; set; }
        public string ResearchBugRootCause { get; set; }
    }
}