using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class GetResearchArticleInfoListResult
    {
        public GetResearchArticleInfoListResult()
        {
            ResearchMethodList = new List<string>();
            ResearchDataSourceList = new List<string>();
            ResearchContextList = new List<string>();
            ResearchedProblemTypeList = new List<string>();
            ResearchPlatformList = new List<string>();
            ResearchedTechnologyList = new List<string>();
            ResearchedApproachList = new List<string>();
            ResearchContributionTypeList = new List<string>();
            ResearchBugRootCauseList = new List<string>();
        }
        public List<string> ResearchMethodList { get; set; }
        public List<string> ResearchDataSourceList { get; set; }
        public List<string> ResearchContextList { get; set; }
        public List<string> ResearchedProblemTypeList { get; set; }
        public List<string> ResearchPlatformList { get; set; }
        public List<string> ResearchedTechnologyList { get; set; }
        public List<string> ResearchedApproachList { get; set; }
        public List<string> ResearchContributionTypeList { get; set; }
        public List<string> ResearchBugRootCauseList { get; set; }
    }

    public class GetResearchArticleInfoList
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public GetResearchArticleInfoList(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate()
        {
            var errorList = new List<KeyValuePair<string, string>>();
            return errorList;
        }

        public GetResearchArticleInfoListResult Handle()
        {
            var res = new GetResearchArticleInfoListResult();
            res.ResearchMethodList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchMethod).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x=>x).ToList(); 
            res.ResearchDataSourceList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchDataSource).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList(); 
            res.ResearchContextList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchContext).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList(); 
            res.ResearchedProblemTypeList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchedProblemType).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList(); 
            res.ResearchPlatformList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchPlatform).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList(); 
            res.ResearchedTechnologyList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchedTechnology).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList(); 
            res.ResearchedApproachList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchedApproach).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList(); 
            res.ResearchContributionTypeList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchContributionType).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList(); 
            res.ResearchBugRootCauseList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchBugRootCause).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x).ToList();
            return res;
        }
    }
}
