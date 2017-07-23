using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Touch.Models
{
    /// <summary>
    ///     ͼƬlist
    /// </summary>
    public class ImageList
    {
        /// <summary>
        ///     ͼƬlist
        /// </summary>
        public readonly List<MyImage> List;

        private ImageList()
        {
            List = new List<MyImage>();
        }

        /// <summary>
        ///     �첽���ʵ����һ���ļ����ڵ�ͼƬlist
        /// </summary>
        /// <param name="folder">һ��Ҫ���з���Ȩ�޵��ļ���</param>
        /// <returns></returns>
        public static async Task<ImageList> GetInstanceAsync(StorageFolder folder)
        {
            var imageList = new ImageList();
            var files = await folder.GetFilesAsync();
            foreach (var file in files)
            {
                if (file.FileType != ".jpg" && file.FileType != ".png")
                    continue;
                var myImage = await GetImageAsync(file);
                if (myImage == null)
                    continue;
                imageList.List.Add(myImage);
            }
            return imageList;
        }

        /// <summary>
        ///     ͨ���ļ����MyImage������Ϊnull
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private static async Task<MyImage> GetImageAsync(StorageFile file)
        {
            MyImage myImage;
            using (var stream = await file.OpenAsync(FileAccessMode.Read))
            {
                var bitmap = new BitmapImage();
                bitmap.SetSource(stream);
                var imageProperties = await file.Properties.GetImagePropertiesAsync();
                var basicProperties = await file.GetBasicPropertiesAsync();
                myImage = new MyImage
                {
                    ImagePath = file.Path,
                    Bitmap = bitmap,
                    Latitude = imageProperties.Latitude,
                    Longitude = imageProperties.Longitude,
                    // ���ͼƬ������ʱ��Ϊ�գ������ļ����޸�ʱ��
                    DateTaken = imageProperties.DateTaken.Year <= 1601
                        ? basicProperties.DateModified.LocalDateTime
                        : imageProperties.DateTaken.LocalDateTime
                };
            }
            return myImage;
        }
    }
}