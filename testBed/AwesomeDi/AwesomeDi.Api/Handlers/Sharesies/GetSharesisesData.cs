using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;
using AwesomeDi.Api.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharesiesInstrumentComparisonPrice = AwesomeDi.Api.Models.SharesiesInstrumentComparisonPrice;

namespace AwesomeDi.Api.Handlers.Sharesies
{
    public class GetSharesisesData
    {
        private readonly _DbContext.AwesomeDiContext _db;
        private readonly ISharesiesService _sharesiesService;
        readonly ILogger _logger;

        public GetSharesisesData(_DbContext.AwesomeDiContext db, ISharesiesService sharesiesService, ILogger logger)
        {
            _db = db;
            _sharesiesService = sharesiesService;
            _logger = logger;
        }
        public List<KeyValuePair<string, string>> Validate()
        {
            var errorList = new List<KeyValuePair<string, string>>();
            // if (!db.FileEntry.Any(x => x.Id == param.Id)) errorList.Add(new KeyValuePair<string, string>("Id", "Id is invalid"));
            return errorList;
        }

        public async Task Handle()
        {
            await FetchInstruments();
            // await ProcessPriceHistory();
        }

        private async Task FetchInstruments()
        {
            LogMsg("Start FetchInstruments");
            var currentPage = 1;
            var run = true;
            var processCount = 1;
            var taskList = new List<Task>();
            while (run)
            {
                var instrumentRes = await _sharesiesService.GetInstruments(currentPage, perPage: 500);
                if (instrumentRes?.CurrentPage == null)
                {
                    run = false;
                    continue;
                }

                if (currentPage >= instrumentRes.NumberOfPages)
                {
                    run = false;
                }

                LogMsg($"Start Process instrumentRes page {currentPage}/{instrumentRes.NumberOfPages}");
                currentPage = (int)instrumentRes.CurrentPage + 1;
                foreach (var instrumentResInstrument in instrumentRes.Instruments)
                {
                    LogMsg($"Start Process instrumentRes: {processCount}-{instrumentResInstrument.Name}");
                    processCount++;
                    taskList.Add(ProcessInstrument(instrumentResInstrument));
                }
            }

            await Task.WhenAll(taskList);
        }

        private async Task ProcessInstrument(SharesiesInstruments instrumentResInstrument)
        {
            var connectionString = _db.Database.GetConnectionString();
            var options = new DbContextOptionsBuilder<_DbContext.AwesomeDiContext>()
                .UseSqlServer(connectionString)
                .Options;
            var context = new _DbContext.AwesomeDiContext(options);
            var dbInstrument = context.SharesiesInstrument.FirstOrDefault(x => x.SharesiesId == instrumentResInstrument.Id);
            if (dbInstrument == null)
            {
                dbInstrument = new SharesiesInstrument();
                dbInstrument.SharesiesId = instrumentResInstrument.Id;
                dbInstrument.ExchangeCountry = instrumentResInstrument.ExchangeCountry;
                dbInstrument.InstrumentType = instrumentResInstrument.InstrumentType;
                dbInstrument.Name = instrumentResInstrument.Name;
                dbInstrument.RiskRating = instrumentResInstrument.RiskRating;
                dbInstrument.Symbol = instrumentResInstrument.Symbol;
                context.SharesiesInstrument.Add(dbInstrument);
                dbInstrument.UrlSlug = instrumentResInstrument.UrlSlug;
                foreach (var category in instrumentResInstrument.Categories)
                {
                    var cat = context.SharesiesInstrumentCategory.FirstOrDefault(x => x.Name == category);
                    if (cat == null)
                    {
                        cat = new SharesiesInstrumentCategory();
                        cat.Name = category;
                        context.SharesiesInstrumentCategory.Add(cat);
                    }

                    dbInstrument.SharesiesInstrumentXCategoryList.Add(new SharesiesInstrumentXCategory
                        { SharesiesInstrument = dbInstrument, SharesiesInstrumentCategory = cat });
                }
            }

            await context.SaveChangesAsync();

            var priceHistory = new SharesiesInstrumentPriceHistory
            {
                SharesiesInstrumentId = dbInstrument.Id,
                Date = instrumentResInstrument.MarketLastCheck ?? DateTime.Now,
                Price = instrumentResInstrument.MarketPrice
            };
            await context.SharesiesInstrumentPriceHistory.AddAsync(priceHistory);
            await context.SaveChangesAsync();

            var comparisonList = new List<SharesiesInstrumentComparisonPrice>();
            var priceList = context.SharesiesInstrumentPriceHistory.Where(x => x.SharesiesInstrumentId == dbInstrument.Id)
                .OrderBy(x => x.ModifiedUtcDateTime).ToList();
            var lastDate = priceList.Last();
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddHours(-1)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.Hour, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddHours(-3)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.ThreeHour, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddHours(-6)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.SixHour, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddHours(-12)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.TwelveHour, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddDays(-1)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.Day, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddDays(-3)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.ThreeDay, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddDays(-7)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.Week, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddDays(-14)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.TwoWeek, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddMonths(-1)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.Month, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddMonths(-2)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.TwoMonth, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddMonths(-3)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.ThreeMonth, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddMonths(-6)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.SixMonth, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddYears(-1)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.Year, comparisonList);
            AddComparison(priceList.Where(x => x.ModifiedUtcDateTime >= lastDate.ModifiedUtcDateTime?.AddYears(-2)).ToList(),
                SharesiesInstrumentComparisonPrice.TypeEnum.TwoYear, comparisonList);
            var existList = context.SharesiesInstrumentComparisonPrice.Where(x => x.SharesiesInstrumentId == dbInstrument.Id)
                .ToList();
            context.SharesiesInstrumentComparisonPrice.RemoveRange(existList);
            context.SharesiesInstrumentComparisonPrice.AddRange(comparisonList);
            await context.SaveChangesAsync();
        }

        private async Task ProcessPriceHistory()
        {
            var sql = "select SharesiesInstrument.Id, Max([Date]) as Date,SharesiesInstrument.SharesiesId from SharesiesInstrument left join SharesiesInstrumentPriceHistory on SharesiesInstrument.Id = SharesiesInstrumentPriceHistory.SharesiesInstrumentId group by SharesiesInstrument.Id,SharesiesInstrument.SharesiesId";
            var list = HelperDb.RawSqlQuery(_db, sql,x => new PriceHistory
            {
                InstrumentId = (int)x[0], 
                Date = (DateTime?)(x[1] == DBNull.Value?DateTime.MinValue: x[1]),
                SharesiesId = (string)(x[2] == DBNull.Value?null:x[2])
            });

            list = list.OrderBy(x => x.Date).ToList();
            foreach (var line in list)
            {
                var priceHistory = await _sharesiesService.GetInstrumentPriceHistory(line.SharesiesId);
                priceHistory = priceHistory.Where(x => x.Date > line.Date).OrderBy(x=>x.Date).ToList();
                var priceHistoryList = priceHistory.Select(x=> new SharesiesInstrumentPriceHistory
                {
                    SharesiesInstrumentId = line.InstrumentId,
                    Date = x.Date,
                    Price = x.Price,
                }).ToList();
                if (priceHistoryList.Any())
                {
                    var priceList = _db.SharesiesInstrumentPriceHistory.Where(x => x.SharesiesInstrumentId == line.InstrumentId).OrderBy(x=>x.Date).ToList();
                    priceList.AddRange(priceHistoryList);
                    if (priceList.Any())
                    {
                        var comparisonList = new List<SharesiesInstrumentComparisonPrice>();
                        var lastDate = priceList.Last();
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddHours(-1)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.Hour,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddHours(-3)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.ThreeHour,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddHours(-6)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.SixHour,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddHours(-12)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.TwelveHour,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddDays(-1)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.Day,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddDays(-3)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.ThreeDay,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddDays(-7)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.Week,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddDays(-14)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.TwoWeek,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddMonths(-1)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.Month,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddMonths(-2)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.TwoMonth,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddMonths(-3)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.ThreeMonth,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddMonths(-6)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.SixMonth,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddYears(-1)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.Year,comparisonList);
                        AddComparison(priceList.Where(x=>x.Date>=lastDate.Date.AddYears(-2)).ToList(),SharesiesInstrumentComparisonPrice.TypeEnum.TwoYear,comparisonList);
                        var existList = _db.SharesiesInstrumentComparisonPrice.Where(x=>x.SharesiesInstrumentId == line.InstrumentId).ToList();
                        _db.SharesiesInstrumentComparisonPrice.RemoveRange(existList);
                        _db.SharesiesInstrumentComparisonPrice.AddRange(comparisonList);
                    }
                    _db.SharesiesInstrumentPriceHistory.AddRange(priceHistoryList);
                    await _db.SaveChangesAsync();
                }
                
            }

        }

        public class PriceHistory
        {
            public int InstrumentId { get; set; }
            public DateTime? Date { get; set; }
            public string SharesiesId { get; set; }
        }

        private void LogMsg(string msg)
        {
            _logger.Log(LogLevel.Trace, $"{DateTime.Now:O}-{msg}");
        }

        private static void AddComparison(List<SharesiesInstrumentPriceHistory> priceHistory, SharesiesInstrumentComparisonPrice.TypeEnum typeEnum,List<SharesiesInstrumentComparisonPrice> comparisonList)
        {
            priceHistory = priceHistory.OrderBy(x => x.Date).ToList();
            var record = new SharesiesInstrumentComparisonPrice
            {
                SharesiesInstrumentId = priceHistory.First().SharesiesInstrumentId,
                Value = priceHistory.Last().Price - priceHistory.First().Price,
                Max = priceHistory.Max(x => x.Price),
                Min = priceHistory.Min(x => x.Price),
                Type = typeEnum,
            };
            var startPrice = priceHistory.First().Price == 0 ? 0.0001m : priceHistory.First().Price;
            record.Percent = record.Value / startPrice;
            comparisonList.Add(record);
            
        }
    }
}
