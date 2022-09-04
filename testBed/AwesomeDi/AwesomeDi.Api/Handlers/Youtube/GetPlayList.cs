using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace AwesomeDi.Api.Handlers.Youtube
{
    public class GetPlayList
    {
        private readonly YoutubeClient _youtubeClient;

        public GetPlayList()
        {
            _youtubeClient = new YoutubeClient();
        }

        public class Parameter
        {
            public string Url { get; set; }
            public string Id { get; set; }
        }

        public class Result
        {
            public Result()
            {
                this.VideoList = new List<Video>();
            }
            public string Title { get; set; }
            public string AuthorTitle { get; set; }
            public string AuthorChannelId { get; set; }
            public List<Video> VideoList { get; set; }
            public string Description { get; set; }
            public string Id { get; set; }

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
            string id = null;
            if (Helpers.HelperString.IsValidUrl(param.Url, out var url))
            {
                var queryDictionary = System.Web.HttpUtility.ParseQueryString(url.Query);
                id = queryDictionary["list"];
            }

            if (!string.IsNullOrWhiteSpace(param.Id)) id = param.Id;
            if (string.IsNullOrWhiteSpace(id)) return null;
            var playlist = await _youtubeClient.Playlists.GetAsync(id);
            var res = new Result();
            res.Title = playlist.Title; 
            res.Description = playlist.Description; 
            res.Id = playlist.Id.Value; 
            res.AuthorTitle = playlist.Author?.Title; 
            res.AuthorChannelId = playlist.Author?.ChannelId;
            // Get all playlist videos
            var videos = await _youtubeClient.Playlists.GetVideosAsync(playlist.Id);
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
