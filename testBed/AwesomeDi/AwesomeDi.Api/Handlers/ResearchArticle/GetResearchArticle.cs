using System.Collections.Generic;
using System.Linq;
using System.Web;
using AwesomeDi.Api.Models;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class GetResearchArticleParameter
    {
        public int? Id { get; set; }
    }
    public class GetResearchArticleResult: Models.ResearchArticle
    {
        public GetResearchArticleResult()
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
        public string Link { get; set; }
    }

    public class GetResearchArticle
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public GetResearchArticle(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(GetResearchArticleParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            return errorList;
        }

        public GetResearchArticleResult Handle(GetResearchArticleParameter param)
        {
            var ra = _db.ResearchArticle.FirstOrDefault(x=>x.Id == param.Id);
            if (ra == null) return null;
            var res = JsonConvert.DeserializeObject<GetResearchArticleResult>(JsonConvert.SerializeObject(ra));
            if(!string.IsNullOrWhiteSpace(res.ResearchMethod)) res.ResearchMethodList = res.ResearchMethod.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(!string.IsNullOrWhiteSpace(res.ResearchDataSource)) res.ResearchDataSourceList = res.ResearchDataSource.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(!string.IsNullOrWhiteSpace(res.ResearchContext)) res.ResearchContextList = res.ResearchContext.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(!string.IsNullOrWhiteSpace(res.ResearchedProblemType)) res.ResearchedProblemTypeList = res.ResearchedProblemType.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(!string.IsNullOrWhiteSpace(res.ResearchPlatform)) res.ResearchPlatformList = res.ResearchPlatform.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(!string.IsNullOrWhiteSpace(res.ResearchedTechnology)) res.ResearchedTechnologyList = res.ResearchedTechnology.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(!string.IsNullOrWhiteSpace(res.ResearchedApproach)) res.ResearchedApproachList = res.ResearchedApproach.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if(!string.IsNullOrWhiteSpace(res.ResearchContributionType)) res.ResearchContributionTypeList = res.ResearchContributionType.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            if (!string.IsNullOrWhiteSpace(res.ResearchBugRootCause)) res.ResearchBugRootCauseList = res.ResearchBugRootCause.Split(';').Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
            switch (ra.DataSource)
            {
                case "IEEE":
	                res.Link = $"https://ieeexplore-ieee-org.ezproxy.canterbury.ac.nz/search/searchresult.jsp?newsearch=true&queryText={HttpUtility.UrlEncode(ra.Title)}";
                    break;

                case "ACM":
	                res.Link = $"https://dl-acm-org.ezproxy.canterbury.ac.nz/action/doSearch?AllField={ra.Title.Replace(' ','+')}";
	                break;

                case "ScienceDirect":
	                res.Link = $"https://www-sciencedirect-com.ezproxy.canterbury.ac.nz/search?qs={HttpUtility.UrlEncode(ra.Title)}";
	                break;

                case "SpringerLink":
	                res.Link = $"https://link-springer-com.ezproxy.canterbury.ac.nz/search?query={ra.Title.Replace(' ', '+')}";
	                break;
            }
            return res;
        }
    }
}
