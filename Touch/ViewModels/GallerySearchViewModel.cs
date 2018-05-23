#region

using System;
using System.Collections.Generic;
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
    internal class GallerySearchViewModel : NotificationHelper
    {
        /// <summary>
        ///     Index for the query label
        /// </summary>
        private readonly IList<int> _searchLabelIndex;

        /// <summary>
        ///     Group on date
        /// </summary>
        private IEnumerable<ImageGroup> _imageGroup;

        public GallerySearchViewModel(string queryStr)
        {
            _searchLabelIndex = new List<int>();
            var labels = queryStr.Split(' ');
            foreach (var label in labels)
            {
                var index = label != null ? new Category().Get(label) : -1;
                if (index != -1)
                    _searchLabelIndex.Add(index);
            }
        }

        /// <summary>
        ///     Group on date
        /// </summary>
        public IEnumerable<ImageGroup> ImageGroup
        {
            get => _imageGroup;
            private set => SetProperty(ref _imageGroup, value);
        }

        /// <summary>
        ///     Load images based on query
        /// </summary>
        /// <returns>Void Task</returns>
        public async Task LoadImagesAsync()
        {
            if (!_searchLabelIndex.Any()) return;
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                var images = new List<ThumbnailImage>();
                using (var db = new Database())
                {
                    var allImages = db.Images.Include(image => image.Labels).ToList();
                    var selectedImages = allImages.Where(image => image.IfContainsLabel(_searchLabelIndex)).ToList();
                    var folders = db.Folders.ToList();
                    foreach (var image in selectedImages)
                    {
                        var storageFile = await Utils.GetFileAsync(image.Path, folders);
                        using (var thumbnail = await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, 240))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.SetSource(thumbnail);
                            var newImage = new ThumbnailImage(image) {Thumbnail = bitmap};
                            images.Add(newImage);
                        }
                    }
                }

                ImageGroup = images.GroupBy(image => image.MonthYear, (key, list) => new ImageGroup(key, list));
            });
        }
    }
}