#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;
using Touch.Data;
using Touch.Helpers;
using Touch.Models;
using Touch.Services;
using Touch.Views.Pages;

#endregion

namespace Touch.ViewModels
{
    internal class GalleryViewModel
    {
        /// <summary>
        ///     Label category
        /// </summary>
        private readonly Category _category;

        private readonly INavigationService _navigationService;

        public GalleryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _category = new Category();
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
            return _category.GetMatchList(query);
        }

        /// <summary>
        ///     Check if query text is in seggestions.
        /// </summary>
        /// <param name="query">Query text</param>
        /// <returns>Whether is in seggestions</returns>
        public bool IsInSuggestions(string query)
        {
            return _category.IsInCategory(query);
        }

        /// <summary>
        ///     Navigate to <see cref="GallerySearchPage" />
        /// </summary>
        /// <param name="query">Query text</param>
        public void NavigateToSearchPage(string query)
        {
            _navigationService.NavigateAsync(typeof(GallerySearchPage), query);
        }
    }
}