#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;
using Touch.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class GalleryViewModel
    {
        public GalleryViewModel()
        {
            Images = new ObservableCollection<Image>();
        }

        public ObservableCollection<Image> Images { get; }

        /// <summary>
        ///     Load images from all folders in database and add them to collections.
        /// </summary>
        public async Task LoadImagesAsync()
        {
            // TODO: It is wrong
            var settingsVm = new SettingsViewModel();
            settingsVm.LoadFolders();

            // Load all folders in database as StorageFolder
            var storageFolders = await Utils.WaitAllTasksAsync(settingsVm.Folders, folder =>
                StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folder.Token).AsTask());

            // Enumerate all image files in folders (including subfolders)
            var fileTypeFilter = new List<string> {".jpg", ".jpeg", ".png", ".bmp"};
            var queryOptions = new QueryOptions(CommonFileQuery.OrderByName, fileTypeFilter);
            var allFilesList = await Utils.WaitAllTasksAsync(storageFolders,
                storageFolder => storageFolder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync().AsTask());

            // Add images to collections
            foreach (var allFiles in allFilesList)
            foreach (var file in allFiles)
                using (var thumbnail = await file.GetThumbnailAsync(ThumbnailMode.PicturesView, 300))
                {
                    var bitmap = new BitmapImage();
                    bitmap.SetSource(thumbnail);
                    // BUG: Too many images may cause memory leak
                    Images.Add(new Image {Path = file.Path, Thumbnail = bitmap});
                }
        }

        /// <summary>
        ///     Get suggestion choices based on query text.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns>Suggestion texts</returns>
        public IList<string> GetSuggestions(string query)
        {
            return Label.Category.Where(item => item.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .OrderBy(s => s).ToList();
        }

        /// <summary>
        ///     Check if query text is in seggestions.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns>Whether is in seggestions</returns>
        public bool IsInSuggestions(string query)
        {
            return Label.Category.Contains(query);
        }
    }
}