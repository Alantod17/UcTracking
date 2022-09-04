using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class UpdateResearchArticleResearchDetailParameter
    {
        public UpdateResearchArticleResearchDetailParameter()
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
        public int? Id { get; set; }
        public string ResearchGoal { get; set; }
        public string ResearchQuestions { get; set; }
        public string ResearchFutureWorks { get; set; }
        public string ResearchChallenges { get; set; }
        public string ResearchMainFindings { get; set; }
        public string Notes { get; set; }
        // public string ResearchDataSource { get; set; }
        // public string ResearchContext { get; set; }
        // public string ResearchedProblemType { get; set; }
        // public string ResearchPlatform { get; set; }
        // public string ResearchedTechnology { get; set; }
        // public string ResearchedApproach { get; set; }
        // public string ResearchContributionType { get; set; }
        // public string ResearchBugRootCause { get; set; }
        // public string ResearchMethod { get; set; }
    }

    public class UpdateResearchArticleResearchDetail
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public UpdateResearchArticleResearchDetail(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(UpdateResearchArticleResearchDetailParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            if (_db.ResearchArticle.All(x => x.Id != param.Id)) errorList.Add(new KeyValuePair<string, string>("Id", "Id is invalid"));
            return errorList;
        }

        public void Handle(UpdateResearchArticleResearchDetailParameter param)
        {
            var ra = _db.ResearchArticle.First(x => x.Id == param.Id);
            ra.ResearchGoal = param.ResearchGoal;
            ra.Notes = param.Notes;
            ra.ResearchQuestions = param.ResearchQuestions;
            ra.ResearchMethod = string.Join(';', param.ResearchMethodList);
            ra.ResearchDataSource = string.Join(';', param.ResearchDataSourceList);
            ra.ResearchContext = string.Join(';', param.ResearchContextList);
            ra.ResearchedProblemType = string.Join(';', param.ResearchedProblemTypeList);
            ra.ResearchPlatform = string.Join(';', param.ResearchPlatformList);
            ra.ResearchedTechnology = string.Join(';', param.ResearchedTechnologyList);
            ra.ResearchedApproach = string.Join(';', param.ResearchedApproachList);
            ra.ResearchMainFindings = param.ResearchMainFindings;
            ra.ResearchContributionType = string.Join(';', param.ResearchContributionTypeList);
            ra.ResearchFutureWorks = param.ResearchFutureWorks;
            ra.ResearchChallenges = param.ResearchChallenges;
            ra.ResearchBugRootCause = string.Join(';', param.ResearchBugRootCauseList);
            _db.SaveChanges();
        }
    }
}
