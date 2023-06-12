using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AwesomeDi.Api.Models;
using System.Linq.Dynamic.Core;
using AwesomeDi.Api.Helpers;
using LinqKit;

namespace AwesomeDi.Api.Handlers.ResearchArticle
{
    public class SearchResearchArticleParameter
    {
        public bool? IsDeleted { get; set; }
        public bool? IsDuplicate { get; set; }
        public bool? IsNeedReview { get; set; }
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public string Keywords { get; set; }
    }
    public class SearchResearchArticleResult
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Abstract { get; set; }
        public string Year { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsDuplicate { get; set; }
        public bool IsNeedReview { get; set; }
        public bool IsIrrelevant { get; set; }
    }
    public class SearchResearchArticlePagedResult
    {
        public int TotalCount { get; set; }
        public List<SearchResearchArticleResult> Results { get; set; }
    }

    public class SearchResearchArticle
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public SearchResearchArticle(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(SearchResearchArticleParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            return errorList;
        }

        public SearchResearchArticlePagedResult PagedHandle(SearchResearchArticleParameter param)
        {
            var result = new SearchResearchArticlePagedResult();
            var res = Search(param);
            result.Results = res.results;
            result.TotalCount = res.totleCount;
            return result;
        }
        public List<SearchResearchArticleResult> Handle(SearchResearchArticleParameter param)
        {
            var res = Search(param);
            return res.results;
        }

        private (int totleCount, List<SearchResearchArticleResult> results) Search(SearchResearchArticleParameter param)
        {
            IQueryable<Models.ResearchArticle> query = _db.ResearchArticle;

            if (param.IsDeleted != null) query = query.Where(x => x.IsDeleted == param.IsDeleted);
            if (param.IsDuplicate != null) query = query.Where(x => x.IsDuplicate == param.IsDuplicate);
            if (param.IsNeedReview != null) query = query.Where(x => x.IsNeedReview == param.IsNeedReview);
            if (!string.IsNullOrWhiteSpace(param.Keywords))
            {
                List<string> searchKeys = param.Keywords.Split(' ').ToList<string>();
                foreach (var searchKey in searchKeys)
                {
                    query = query.Where(x => x.Title.Contains(searchKey));
                }
            }
            var totalCount = query.Count();

            query = HelperDb.Sort(param.SortField, param.SortDirection, query);

            var queryResList = query.Skip(param.Skip ?? 0).Take(param.Take ?? 50)
                .Select(x => new SearchResearchArticleResult
                {
                    Id = x.Id,
                    Title = x.Title,
                    Abstract = x.Abstract,
                    Year = x.Year,
                    IsDeleted = x.IsDeleted,
                    IsDuplicate = x.IsDuplicate,
                    IsNeedReview = x.IsNeedReview,
                    IsIrrelevant = x.IsIrrelevant,
                })
                .ToList();

            return (totalCount, queryResList);
        }

        
    }
}