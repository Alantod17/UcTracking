using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;

namespace AwesomeDi.Api.Helpers
{
	public class HelperThumbnail
	{
        public enum VideoThumbnailTypeEnum {Standard,Square,Full}
        public static string GetThumbnailFileName(string thumbnailFolderPath, string fileName, VideoThumbnailTypeEnum type = VideoThumbnailTypeEnum.Standard)
        {
            return type switch
            {
                VideoThumbnailTypeEnum.Square => Path.Combine(thumbnailFolderPath, fileName + "-thumb-square.png"),
                VideoThumbnailTypeEnum.Full => Path.Combine(thumbnailFolderPath, fileName + "-thumb-full.png"),
                VideoThumbnailTypeEnum.Standard => Path.Combine(thumbnailFolderPath, fileName + "-thumb.png"),
                _ => Path.Combine(thumbnailFolderPath, fileName + "-thumb.png")
            };
        }
        public static void CreateVideoThumbnail(string videoPath, string thumbnailFolderPath, string defaultThumbnailPath, string fileName, int size = 300, int quality = 50)
		{
			var processFilePath = GetThumbnailFileName(thumbnailFolderPath,fileName,VideoThumbnailTypeEnum.Full);

			var ffmpegFileName = "Assets\\ffmpeg.exe";
			var procStartInfo = new ProcessStartInfo()
			{
				FileName = ffmpegFileName,
				Arguments = $"-i \"{videoPath}\" -ss 00:00:01 -t 00:00:01 -r 1  \"{processFilePath}\" -y",
				RedirectStandardOutput = true,
				UseShellExecute = false,
				CreateNoWindow = false
			};
            var maxWaitCount = 30;
			var proc = new Process { StartInfo = procStartInfo };
			if (proc.Start())
			{
				var waitCount = 0;
				while (!File.Exists(processFilePath) && waitCount < maxWaitCount)
				{
					waitCount++;
					Thread.Sleep(1000);
				}
                CreateImageThumbnail(waitCount >= maxWaitCount ? defaultThumbnailPath : processFilePath, thumbnailFolderPath, fileName, size, quality);
            }
			System.GC.Collect();
			System.GC.WaitForPendingFinalizers();
		}
        

        public static void CreateImageThumbnail(string inputPath, string outputFolderPath, string fileName, int size = 300, int quality = 50)
        {
            var thumbnailPath = GetThumbnailFileName(outputFolderPath, fileName);
            var thumbnailSquarePath = GetThumbnailFileName(outputFolderPath, fileName, VideoThumbnailTypeEnum.Square);

            var image = new Bitmap(Image.FromFile(inputPath));

            Rectangle cloneRect;
            if (image.Width > image.Height)
            {
                cloneRect = new Rectangle((image.Width - image.Height) / 2, 0, image.Height, image.Height);
            }
            else
            {
                cloneRect = new Rectangle(0, (image.Height - image.Width) / 2, image.Width, image.Width);
            }
            var imageSquare = image.Clone(cloneRect, image.PixelFormat);

            PaintImage(size, quality, image, thumbnailPath);
            PaintImage(size, quality, imageSquare, thumbnailSquarePath);
            image.Dispose();
            imageSquare.Dispose();
        }

        private static void PaintImage(int size, int quality, Bitmap image, string outputPath)
        {
            int width;
            int height;
            if (image.Width > image.Height)
            {
                width = size;
                height = Convert.ToInt32(image.Height * size / (double)image.Width);
            }
            else
            {
                width = Convert.ToInt32(image.Width * size / (double)image.Height);
                height = size;
            }

            var resized = new Bitmap(width, height);
            using (var graphics = Graphics.FromImage(resized))
            {
                graphics.CompositingQuality = CompositingQuality.HighSpeed;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.DrawImage(image, 0, 0, width, height);
                var qualityParamId = Encoder.Quality;
                var encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = new EncoderParameter(qualityParamId, quality);
                resized.Save(outputPath);
            }
        }
    }


}
