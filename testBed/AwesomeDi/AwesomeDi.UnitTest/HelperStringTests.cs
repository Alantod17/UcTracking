using AwesomeDi.Api.Handlers;
using AwesomeDi.Api.Handlers.ResearchArticle;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;

namespace AwesomeDi.UnitTest
{
	public class HelperStringTests
    {
		[SetUp]
		public void Setup()
		{
		}
        [Test]
        public void Decrypt()
        {
	        var res = HelperString.Decrypt("XHrTNfuR9Q8m44Odx2DAUw==");
        }
        
	}
}