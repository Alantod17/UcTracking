using System.Drawing;
using System.IO;
using AwesomeDi.Api.Helpers;
using AwesomeDi.Api.Models;

namespace AwesomeDi.Api.Handlers.FileEntry
{
    public class HelperFileEntry
    {
        public static bool IsThumbnailExist(Models.FileEntry fileEntry, string thumbnailFolderPath)
        {
            var thumbPath = HelperThumbnail.GetThumbnailFileName(thumbnailFolderPath, fileEntry.Id.ToString(), HelperThumbnail.VideoThumbnailTypeEnum.Standard);
            return File.Exists(thumbPath);
        }

        public static void CreateThumbnailIfNotExist(_DbContext.AwesomeDiContext db, Models.FileEntry fileEntry, Configuration config)
        {
            if (IsThumbnailExist(fileEntry, config.ThumbnailFolderPath)) return;
            if (HelperFile.IsImageFile(fileEntry.FilePath))
            {
                HelperThumbnail.CreateImageThumbnail(fileEntry.FilePath, config.ThumbnailFolderPath, fileEntry.Id.ToString());
                var image = new Bitmap(Image.FromFile(fileEntry.FilePath));
                fileEntry.Width = image.Width;
                fileEntry.Height = image.Height;
            }else if (HelperFile.IsVideoFile(fileEntry.FilePath))
            {
                var fullThumbPath = HelperThumbnail.GetThumbnailFileName(config.ThumbnailFolderPath, fileEntry.Id.ToString(), HelperThumbnail.VideoThumbnailTypeEnum.Full);
                HelperThumbnail.CreateVideoThumbnail(fileEntry.FilePath, config.ThumbnailFolderPath, config.DefaultThumbnailFilePath, fileEntry.Id.ToString());
                if (File.Exists(fullThumbPath))
                {
                    var image = new Bitmap(Image.FromFile(fullThumbPath));
                    fileEntry.Width = image.Width;
                    fileEntry.Height = image.Height;
                }
            }
            else
            {
                HelperThumbnail.CreateImageThumbnail(config.DefaultThumbnailFilePath, config.ThumbnailFolderPath, fileEntry.Id.ToString());
            }
            fileEntry.Extension = Path.GetExtension(fileEntry.FilePath)?.ToUpper();
            db.SaveChanges();
        }
    }
}
