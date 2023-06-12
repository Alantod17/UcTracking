using AwesomeDi.Api.Helpers;
using NUnit.Framework;

namespace AwesomeDi.UnitTest
{
	public class ImageTests
	{
		private readonly string _assetPath = "..\\..\\..\\Assets\\";

		[Test]
		public void GetThumbnailImageTest()
		{
			var fileName = "image7";
			var outFileName = fileName + "-out";
			var extension = ".jpg";

			var inputPath = $"{_assetPath}{fileName}{extension}";
			var outputPath = $"{_assetPath}{outFileName}{extension}";
			var outputPathSquare = $"{_assetPath}{outFileName}-Square{extension}";

			HelperThumbnail.CreateImageThumbnail(inputPath, _assetPath, fileName);
		}

	}
}