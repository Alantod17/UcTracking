using System;
using System.Threading.Tasks;
using AwesomeDi.Api.Helpers;
using NUnit.Framework;
using YahooFinanceApi;

namespace AwesomeDi.UnitTest
{
	public class YahooFinanceTests
	{

		[Test]
		public async Task GetStockQuotes()
		{
            var securities = await Yahoo.Symbols("AAPL", "GOOG").Fields(Field.Symbol, Field.RegularMarketPrice, Field.FiftyTwoWeekHigh).QueryAsync();
            var aapl = securities["AAPL"];
            var price = aapl[Field.RegularMarketPrice];
        }

        [Test]
        public async Task GetHistoricalAsync()
        {
            // You should be able to query data from various markets including US, HK, TW
            // The startTime & endTime here defaults to EST timezone
            var history = await Yahoo.GetHistoricalAsync("AAPL");

            foreach (var candle in history)
            {
                Console.WriteLine($"DateTime: {candle.DateTime}, Open: {candle.Open}, High: {candle.High}, Low: {candle.Low}, Close: {candle.Close}, Volume: {candle.Volume}, AdjustedClose: {candle.AdjustedClose}");
            }
        }

        [Test]
        public async Task GetDividendsAsync()
        {
            // You should be able to query data from various markets including US, HK, TW
            var dividends = await Yahoo.GetDividendsAsync("AAPL");
            foreach (var candle in dividends)
            {
                Console.WriteLine($"DateTime: {candle.DateTime}, Dividend: {candle.Dividend}");
            }
        }

        [Test]
        public async Task GetSplitsAsync()
        {
            var splits = await Yahoo.GetSplitsAsync("AAPL");
            foreach (var s in splits)
            {
                Console.WriteLine($"DateTime: {s.DateTime}, AfterSplit: {s.AfterSplit}, BeforeSplit: {s.BeforeSplit}");
            }
        }

    }
}