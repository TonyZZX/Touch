#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class GallerySearchViewModel
    {
        /// <summary>
        ///     Index for the query label
        /// </summary>
        private readonly int _searchLabelIndex;

        public GallerySearchViewModel(string query)
        {
            _searchLabelIndex = query != null ? new Category().Get(query) : -1;
            Images = new ObservableCollection<ThumbnailImage>();
        }

        /// <summary>
        ///     Image list for search result
        /// </summary>
        public ObservableCollection<ThumbnailImage> Images { get; }

        /// <summary>
        ///     Load images based on query
        /// </summary>
        /// <returns>Void Task</returns>
        public async Task LoadImagesAsync()
        {
            if (_searchLabelIndex == -1) return;
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var db = new Database())
                {
                    var allImages = db.Images.Include(image => image.Labels).ToList();
                    var selectedImages = allImages.Where(image => image.IfContainsLabel(_searchLabelIndex)).ToList();
                    var folders = db.Folders.ToList();
                    foreach (var image in selectedImages)
                    {
                        var storageFile = await Utils.GetFileAsync(image.Path, folders);
                        using (var thumbnail = await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, 300))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.SetSource(thumbnail);
                            var newImage = new ThumbnailImage(image) {Thumbnail = bitmap};
                            Images.Add(newImage);
                        }
                    }
                }
            });
        }
    }
}