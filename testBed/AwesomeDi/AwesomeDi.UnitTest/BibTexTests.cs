using AwesomeDi.Api.Handlers;
using AwesomeDi.Api.Handlers.ResearchArticle;
using AwesomeDi.Api.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace AwesomeDi.UnitTest
{
	public class BibTexTests
	{
		[SetUp]
		public void Setup()
		{
		}
        [Test]
        public void Test2()
        {
            var options = new DbContextOptionsBuilder<_DbContext.AwesomeDiContext>()
                .UseInMemoryDatabase(databaseName: "AwesomeDiDatabase")
                .Options;
            // Insert seed data into the database using one instance of the context
            using (var context = new _DbContext.AwesomeDiContext(options))
            {
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new _DbContext.AwesomeDiContext(options))
            {
                var sut = new ImportDataFromIeeeCsv(context);
                sut.Handle(@"c:\temp\export2021.06.06-03.07.02-old-new.csv");
                sut.Handle(@"c:\temp\export2021.06.06-03.05.33-new-old.csv");
            }
        }
        [Test]
		public void Test1()
		{
			var options = new DbContextOptionsBuilder<_DbContext.AwesomeDiContext>()
                .UseInMemoryDatabase(databaseName: "AwesomeDiDatabase")
                .Options;
            // Insert seed data into the database using one instance of the context
            using (var context = new _DbContext.AwesomeDiContext(options))
            {
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new _DbContext.AwesomeDiContext(options))
            {
			    var sut = new ImportDataFromBibTex(context);
			    sut.FixBibTexFile();
            }
        }
	}
}