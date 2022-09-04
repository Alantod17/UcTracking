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
    public class DownloadPlayList
    {
        private readonly YoutubeClient _youtubeClient;

        public DownloadPlayList()
        {
            _youtubeClient = new YoutubeClient();
        }

        public class Parameter
        {
            public string Url { get; set; }
            public string Id { get; set; }
            public string Format { get; set; }
            public string FolderPath { get; set; }
            public bool SkipIfExist { get; set; }
        }
        public async Task HandleAsync(Parameter param)
        {
            if (string.IsNullOrWhiteSpace(param.FolderPath) || !System.IO.Directory.Exists(param.FolderPath)) throw new ArgumentException("FolderPath is invalid");
            var hGetPlayList = new GetPlayList();
            var playList = await hGetPlayList.HandleAsync(new GetPlayList.Parameter
            {
                Id = param.Id
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
                if (param.SkipIfExist)
                {
                    var filePath = $@"{param.FolderPath}\{HelperString.GetValidFileName(video.Title, '_')}.test";
                    if (HelperFile.IsFileExistWithoutExtension(filePath))
                    {
                        continue;
                    }
                }
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