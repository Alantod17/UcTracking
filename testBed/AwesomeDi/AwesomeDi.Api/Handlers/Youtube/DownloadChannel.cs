using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AwesomeDi.Api.Helpers;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace AwesomeDi.Api.Handlers.Youtube
{
    public class DownloadChannel
    {
        private readonly YoutubeClient _youtubeClient;

        public DownloadChannel()
        {
            _youtubeClient = new YoutubeClient();
        }

        public class Parameter
        {
            public string Url { get; set; }
            public string Id { get; set; }
            public string Format { get; set; }
            public string FolderPath { get; set; }
            public string Keyword { get; set; }
            public bool SkipIfExist { get; set; }
        }
        public async Task HandleAsync(Parameter param)
        {
            if (string.IsNullOrWhiteSpace(param.FolderPath) || !System.IO.Directory.Exists(param.FolderPath)) throw new ArgumentException("FolderPath is invalid");
            var hGetPlayList = new GetChannel();
            var playList = await hGetPlayList.HandleAsync(new GetChannel.Parameter
            {
                Id = param.Id,
                Keyword = param.Keyword,
            });
            var taskList = new List<Task>();
            var hDownloadVideo = new DownloadVideo();
            foreach (var video in playList.VideoList)
            {
                var phDownloadVideo = new DownloadVideo.Parameter();
                phDownloadVideo.Id = video.Id;
                phDownloadVideo.Format = param.Format;
                phDownloadVideo.FolderPath = param.FolderPath;
                phDownloadVideo.SkipIfExist = param.SkipIfExist;
               
                await hDownloadVideo.HandleAsync(phDownloadVideo);
                // taskList.Add(hDownloadVideo.HandleAsync(phDownloadVideo));
                // if (taskList.Count >= 10)
                // {
                //     Task.WaitAll(taskList.ToArray());
                //     taskList.Clear();
                // }
            }
        }
    }
}
