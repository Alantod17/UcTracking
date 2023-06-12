using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AwesomeDi.Api.Helpers;
using Polly;
using YoutubeExplode;
using YoutubeExplode.Common;
using YoutubeExplode.Videos.Streams;

namespace AwesomeDi.Api.Handlers.Youtube
{
    public class DownloadVideo
    {
        private readonly YoutubeClient _youtubeClient;

        public DownloadVideo()
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
            var key = param.Url;
            if (!string.IsNullOrWhiteSpace(param.Id)) key = param.Id;
            if (string.IsNullOrWhiteSpace(key)) throw new ArgumentException("Url and id are invalid");

            var video = await _youtubeClient.Videos.GetAsync(key);
            
            var title = video.Title; 
            var author = video.Author.Title; 
            var duration = video.Duration; 
            
            var streamManifest = await _youtubeClient.Videos.Streams.GetManifestAsync(video.Id);
            
            var streamInfo = GetStreamInfo(param, streamManifest);


            // Get the actual stream
            // var stream = await _youtubeClient.Videos.Streams.GetAsync(streamInfo);

            // Download the stream to a file

            if (string.IsNullOrWhiteSpace(param.FolderPath)) throw new ArgumentException("FolderPath is invalid");
            var policy = Policy.Handle<Exception>().WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(120),
                TimeSpan.FromSeconds(240),
                TimeSpan.FromSeconds(480)
            });
            await policy.ExecuteAsync(async () =>
            {
                var filePath = $@"{param.FolderPath}\{HelperString.GetValidFileName(title, '_')}.{streamInfo.Container}";
                var fileInfo = new FileInfo(filePath);
                if (param.SkipIfExist && fileInfo.Exists && fileInfo.Length>0) return;
                await using (StreamWriter w = File.AppendText(@"C:\temp\log.txt"))
                {
                    await w.WriteLineAsync($"Try download {filePath} at {DateTime.Now:hh:mm:ss.fff}");
                }
                await _youtubeClient.Videos.Streams.DownloadAsync(streamInfo, filePath);
            });

        }


        private IStreamInfo GetStreamInfo(Parameter param, StreamManifest streamManifest)
        {
            var format = param.Format?.ToLower();
            switch (format)
            {
                case "videoonly":
                    return streamManifest
                        .GetVideoOnlyStreams()
                        .Where(s => s.Container == Container.Mp4)
                        .GetWithHighestVideoQuality();
                case "audioonly":
                    // ...or highest bitrate audio-only stream
                    return streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                default:
                    return streamManifest.GetMuxedStreams().GetWithHighestVideoQuality();
            }


           
        }
    }
}
