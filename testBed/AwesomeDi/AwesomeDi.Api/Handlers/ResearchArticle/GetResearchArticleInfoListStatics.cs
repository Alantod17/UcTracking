using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class GetResearchArticleInfoListStaticsResult
    {
        public class Data
        {
            public string TagName { get; set; }
            public int UsageCount { get; set; }
        }
        public GetResearchArticleInfoListStaticsResult()
        {
            ResearchMethodList = new List<Data>();
            ResearchDataSourceList = new List<Data>();
            ResearchContextList = new List<Data>();
            ResearchedProblemTypeList = new List<Data>();
            ResearchPlatformList = new List<Data>();
            ResearchedTechnologyList = new List<Data>();
            ResearchedApproachList = new List<Data>();
            ResearchContributionTypeList = new List<Data>();
            ResearchBugRootCauseList = new List<Data>();
        }
        public List<Data> ResearchMethodList { get; set; }
        public List<Data> ResearchDataSourceList { get; set; }
        public List<Data> ResearchContextList { get; set; }
        public List<Data> ResearchedProblemTypeList { get; set; }
        public List<Data> ResearchPlatformList { get; set; }
        public List<Data> ResearchedTechnologyList { get; set; }
        public List<Data> ResearchedApproachList { get; set; }
        public List<Data> ResearchContributionTypeList { get; set; }
        public List<Data> ResearchBugRootCauseList { get; set; }
    }

    public class GetResearchArticleInfoListStatics
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public GetResearchArticleInfoListStatics(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate()
        {
            var errorList = new List<KeyValuePair<string, string>>();
            return errorList;
        }

        public GetResearchArticleInfoListStaticsResult Handle()
        {
            var res = new GetResearchArticleInfoListStaticsResult();
            res.ResearchMethodList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchMethod).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x=>x)
                .Select(x=>new GetResearchArticleInfoListStaticsResult.Data{TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchMethod.Contains(x))}).ToList();
            res.ResearchDataSourceList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchDataSource).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchDataSource.Contains(x)) }).ToList(); 
            res.ResearchContextList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchContext).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchContext.Contains(x)) }).ToList(); 
            res.ResearchedProblemTypeList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchedProblemType).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchedProblemType.Contains(x)) }).ToList(); 
            res.ResearchPlatformList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchPlatform).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchPlatform.Contains(x)) }).ToList(); 
            res.ResearchedTechnologyList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchedTechnology).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchedTechnology.Contains(x)) }).ToList(); 
            res.ResearchedApproachList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchedApproach).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchedApproach.Contains(x)) }).ToList(); 
            res.ResearchContributionTypeList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchContributionType).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchContributionType.Contains(x)) }).ToList(); 
            res.ResearchBugRootCauseList = string.Join(";", _db.ResearchArticle.Select(x => x.ResearchBugRootCause).ToList()).Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).Distinct().OrderBy(x => x)
                .Select(x => new GetResearchArticleInfoListStaticsResult.Data { TagName = x, UsageCount = _db.ResearchArticle.Count(ra => ra.ResearchBugRootCause.Contains(x)) }).ToList();
            return res;
        }
    }
}
