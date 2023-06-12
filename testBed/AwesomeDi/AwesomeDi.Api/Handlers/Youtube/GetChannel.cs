using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace AwesomeDi.Api.Handlers.Youtube
{
    public class GetChannel
    {
        private readonly YoutubeClient _youtubeClient;

        public GetChannel()
        {
            _youtubeClient = new YoutubeClient();
        }

        public class Parameter
        {
            public string Url { get; set; }
            public string Id { get; set; }
            public string Keyword { get; set; }
        }

        public class Result
        {
            public Result()
            {
                this.VideoList = new List<Video>();
            }
            public string Title { get; set; }
            public string Id { get; set; }
            public List<Video> VideoList { get; set; }

            public class Video
            {
                public string Id { get; set; }
                public string Title { get; set; }
                public string Author { get; set; }
                public TimeSpan? Duration { get; set; }
                public string Url { get; set; }
            }
        }
        public async Task<Result> HandleAsync(Parameter param)
        {
            var res = new Result();
            var channel = await _youtubeClient.Channels.GetAsync(param.Id);
            res.Title = channel.Title;
            res.Id = channel.Id;
            var videos = await _youtubeClient.Channels.GetUploadsAsync(channel.Id);
            if (!string.IsNullOrWhiteSpace(param.Keyword))
            {
                videos = videos.Where(x => x.Title.Contains(param.Keyword, StringComparison.OrdinalIgnoreCase)).ToList();
            }
            foreach (var video in videos)
            {
                var resVideo = new Result.Video();
                resVideo.Title = video.Title;
                resVideo.Duration = video.Duration;
                resVideo.Author = video.Author.Title;
                resVideo.Id = video.Id;
                resVideo.Url = video.Url;
                res.VideoList.Add(resVideo);
            }
            return res;
        }
    }
}
