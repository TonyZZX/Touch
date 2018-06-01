#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Touch.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    /// <summary>
    ///     Get grouped images based on selecting function
    /// </summary>
    internal abstract class SelectedImagesViewModel : NotificationHelper
    {
        public ObservableCollection<ThumbnailImage> Images;

        protected SelectedImagesViewModel()
        {
            Images = new ObservableCollection<ThumbnailImage>();
        }

        /// <summary>
        ///     Load images
        /// </summary>
        /// <typeparam name="T">Parameter Type</typeparam>
        /// <param name="param">Parameter in selectFunc</param>
        /// <param name="selectFunc">Selecting function</param>
        /// <returns>Void Task</returns>
        protected async Task LoadImagesAsync<T>(T param, Func<Image, T, bool> selectFunc)
        {
            using (var db = new Database())
            {
                var allImages = db.Images.Include(image => image.Labels).ToList();
                var selectedImages = allImages.Where(image => selectFunc(image, param)).ToList();
                var folders = db.Folders.ToList();
                var galleryItemWidth = Application.Current.Resources["GalleryItemWidth"] as double?;
                if (galleryItemWidth != null)
                    foreach (var image in selectedImages)
                    {
                        var storageFile = await Utils.GetFileAsync(image.Path, folders);
                        using (var thumbnail =
                            await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, (uint) galleryItemWidth))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.SetSource(thumbnail);
                            var newImage = new ThumbnailImage(image) {Thumbnail = bitmap};
                            Images.Add(newImage);
                        }
                    }
            }
        }

        /// <summary>
        ///     Load images (Interface)
        /// </summary>
        /// <returns>Void Task</returns>
        public abstract Task LoadImagesAsync();
    }
}