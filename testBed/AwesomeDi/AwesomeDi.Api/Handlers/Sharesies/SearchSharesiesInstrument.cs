using System;
using System.Collections.Generic;
using System.Linq;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.Sharesies
{
    public class SearchSharesiesInstrumentParameter
    {
        public int? Take { get; set; }
        public int? Skip { get; set; }
        public string SortField { get; set; }
        public string SortDirection { get; set; }
        public string Keywords { get; set; }
        public string ExchangeCountry { get; set; }
    }
    public class SearchSharesiesInstrumentResult
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal? DayChangePercent { get; set; }
        public decimal? ThreeDayChangePercent { get; set; }
        public decimal? WeekChangePercent { get; set; }
        public decimal? TwoWeekChangePercent { get; set; }
        public decimal? MonthChangePercent { get; set; }
        public decimal? TwoMonthChangePercent { get; set; }
        public decimal? ThreeMonthChangePercent { get; set; }
        public decimal? SixMonthChangePercent { get; set; }
        public decimal? YearChangePercent { get; set; }
        public string SharesiesId { get; set; }
        public string UrlSlug { get; set; }
        public int RiskRating { get; set; }
        public string ExchangeCountry { get; set; }
        public decimal? LastPrice { get; set; }
    }
    public class SearchSharesiesInstrumentPagedResult
    {
        public int TotalCount { get; set; }
        public List<SearchSharesiesInstrumentResult> Results { get; set; }
    }

    public class SearchSharesiesInstrument
    {
        private readonly _DbContext.AwesomeDiContext _db;

        public SearchSharesiesInstrument(_DbContext.AwesomeDiContext db)
        {
            _db = db;
        }
        public List<KeyValuePair<string, string>> Validate(SearchSharesiesInstrumentParameter param)
        {
            var errorList = new List<KeyValuePair<string, string>>();
            return errorList;
        }

        public SearchSharesiesInstrumentPagedResult PagedHandle(SearchSharesiesInstrumentParameter param)
        {
            var result = new SearchSharesiesInstrumentPagedResult();
            var res = Search(param);
            result.Results = res.results;
            result.TotalCount = res.totleCount;
            return result;
        }
        public List<SearchSharesiesInstrumentResult> Handle(SearchSharesiesInstrumentParameter param)
        {
            var res = Search(param);
            return res.results;
        }

        private (int totleCount, List<SearchSharesiesInstrumentResult> results) Search(SearchSharesiesInstrumentParameter param)
        {
            IQueryable<Models.SharesiesInstrument> query = _db.SharesiesInstrument;
            if (!string.IsNullOrWhiteSpace(param.ExchangeCountry))
            {
                query = query.Where(x => x.ExchangeCountry.ToLower() == param.ExchangeCountry.ToLower());
            }
            if (!string.IsNullOrWhiteSpace(param.Keywords))
            {
                List<string> searchKeys = param.Keywords.Split(' ').ToList<string>();
                foreach (var searchKey in searchKeys)
                {
                    query = query.Where(x => x.Name.Contains(searchKey));
                }
            }
            var resQuery = query.Select(x => new SearchSharesiesInstrumentResult
            {
                Id = x.Id,
                Name = x.Name,
                SharesiesId = x.SharesiesId,
                UrlSlug = x.UrlSlug,
                RiskRating = x.RiskRating,
                ExchangeCountry = x.ExchangeCountry,
                LastPrice = x.SharesiesInstrumentPriceHistoryList.OrderByDescending(p=>p.Id).FirstOrDefault().Price,
                DayChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.Day).Percent,
                ThreeDayChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.ThreeDay).Percent,
                WeekChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.Week).Percent,
                TwoWeekChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.TwoWeek).Percent,
                MonthChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.Month).Percent,
                TwoMonthChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.TwoMonth).Percent,
                ThreeMonthChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.ThreeMonth).Percent,
                SixMonthChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.SixMonth).Percent,
                YearChangePercent = x.SharesiesInstrumentComparisonPriceList.FirstOrDefault(c=>c.Type == SharesiesInstrumentComparisonPrice.TypeEnum.Year).Percent,
            });

            resQuery = HelperDb.Sort(param.SortField, param.SortDirection, resQuery);

            var totalCount = resQuery.Count();
            var queryResList = resQuery.Skip(param.Skip ?? 0).Take(param.Take ?? 50).ToList();
            foreach(var item in queryResList)
            {
                item.DayChangePercent = Math.Round((item.DayChangePercent ?? 0) * 100, 2);
                item.ThreeDayChangePercent = Math.Round((item.ThreeDayChangePercent ?? 0) * 100, 2);
                item.WeekChangePercent = Math.Round((item.WeekChangePercent ?? 0) * 100, 2);
                item.TwoWeekChangePercent = Math.Round((item.TwoWeekChangePercent ?? 0) * 100, 2);
                item.MonthChangePercent = Math.Round((item.MonthChangePercent ?? 0) * 100, 2);
                item.TwoMonthChangePercent = Math.Round((item.TwoMonthChangePercent ?? 0) * 100, 2);
                item.ThreeMonthChangePercent = Math.Round((item.ThreeMonthChangePercent ?? 0) * 100, 2);
                item.SixMonthChangePercent = Math.Round((item.SixMonthChangePercent ?? 0) * 100, 2);
                item.YearChangePercent = Math.Round((item.YearChangePercent ?? 0) * 100, 2);
            }
            return (totalCount, queryResList);
        }

        
    }
}