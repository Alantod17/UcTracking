using System.Threading.Tasks;
using AwesomeDi.Api.Handlers.Youtube;
using NUnit.Framework;

namespace AwesomeDi.UnitTest
{
    public class YoutubeTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task GetPlayList()
        {
            var handler = new GetPlayList();
            var res = await handler.HandleAsync(new GetPlayList.Parameter{Id = "https://www.youtube.com/playlist?list=PLULEYZawGZNl3-e8WGarLFj7fP2VMPgqA" });
        }
        [Test]
        public async Task DownloadPlayList()
        {
            var handler = new DownloadPlayList();
            await handler.HandleAsync(new DownloadPlayList.Parameter
            {
                Id = "https://www.youtube.com/playlist?list=PLULEYZawGZNl3-e8WGarLFj7fP2VMPgqA",
                FolderPath = @"D:\NewFolder", 
                Format = "AudioOnly",
                SkipIfExist = true
            });
        }

        [Test]
        public async Task GetChannel()
        {
            var handler = new GetChannel();
            await handler.HandleAsync(new GetChannel.Parameter
            {
                Id = "https://www.youtube.com/channel/UClkRzsdvg7_RKVhwDwiDZOA",
                Keyword = "( cover by J.Fla )"
            });
        }
        [Test]
        public async Task DownloadChannel()
        {
            var handler = new DownloadChannel();
            await handler.HandleAsync(new DownloadChannel.Parameter
            {
                Id = "https://www.youtube.com/channel/UClkRzsdvg7_RKVhwDwiDZOA",
                FolderPath = @"D:\JFla",
                Format = "AudioOnly",
                Keyword = "( cover by J.Fla )",
                SkipIfExist = true
            });
        }

        [Test]
        public async Task DownloadVideo()
        {
            var handler = new DownloadVideo();
            await handler.HandleAsync(new DownloadVideo.Parameter
            {
                Id = "https://www.youtube.com/watch?v=tb2Ofv8Ewfs&list=PLR3gS7aEmKgotIAbN7JI60FNs793F2zMQ&index=7&ab_channel=POPMusicChannel",
                FolderPath = @"D:\JFla",
                // Format = "AudioOnly",
                // SkipIfExist = true
            });
        }
    }
}
