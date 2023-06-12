using AwesomeDi.Api.Helpers;
using NUnit.Framework;

namespace AwesomeDi.UnitTest
{
	public class VideoTests
	{
		private readonly string _assetPath = "..\\..\\..\\Assets\\";

		[Test]
		public void GetThumbnailImageTest()
		{
			var fileName = "video8";
			var extension = ".mov";
			var inputPath = $"{_assetPath}{fileName}{extension}";
			var defaultThumbPath = $"{_assetPath}image1.jpg";
			HelperThumbnail.CreateVideoThumbnail(inputPath, _assetPath, defaultThumbPath, fileName);
		}

	}
}