using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Models;
using Newtonsoft.Json;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class UpdateResearchArticleStatusParameter
    {
        public int? Id { get; set; }
        public bool IsDuplicate { get; set; }
        public bool IsIrrelevant { get; set; }
        public bool IsNeedReview { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class UpdateResearchArticleStatus
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public UpdateResearchArticleStatus(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(UpdateResearchArticleStatusParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            if (_db.ResearchArticle.All(x => x.Id != param.Id)) errorList.Add(new KeyValuePair<string, string>("Id", "Id is invalid"));
            return errorList;
        }

        public void Handle(UpdateResearchArticleStatusParameter param)
        {
            var ra = _db.ResearchArticle.First(x => x.Id == param.Id);
            ra.IsDeleted = param.IsDeleted;
            ra.IsDuplicate = param.IsDuplicate;
            ra.IsIrrelevant = param.IsIrrelevant;
            ra.IsNeedReview = param.IsNeedReview;
            _db.SaveChanges();
        }
    }
}
