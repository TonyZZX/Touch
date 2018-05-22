#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Helpers;
using Touch.Models;
using Touch.Services;
using Touch.Views.Pages;

#endregion

namespace Touch.ViewModels
{
    internal class GalleryViewModel : NotificationHelper
    {
        /// <summary>
        ///     Label category
        /// </summary>
        private readonly Category _category;

        /// <summary>
        ///     Navigate to another page in this page
        /// </summary>
        private readonly INavigationService _navigationService;

        /// <summary>
        ///     Max value of ProgressBar
        /// </summary>
        private double _maxValue;

        /// <summary>
        ///     Current value of ProgressBar
        /// </summary>
        private double _progress;

        public GalleryViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _category = new Category();
            Images = new ObservableCollection<ThumbnailImage>();
            MaxValue = 1;
            Progress = 0;
        }

        /// <summary>
        ///     Image list of all folders
        /// </summary>
        public ObservableCollection<ThumbnailImage> Images { get; }

        /// <summary>
        ///     Max value of ProgressBar
        /// </summary>
        public double MaxValue
        {
            get => _maxValue;
            private set => SetProperty(ref _maxValue, value);
        }

        /// <summary>
        ///     Current value of ProgressBar
        /// </summary>
        public double Progress
        {
            get => _progress;
            private set => SetProperty(ref _progress, value);
        }

        /// <summary>
        ///     Load images from all folders in database and add them to collections.
        /// </summary>
        /// <returns>Void Task</returns>
        public async Task LoadImagesAsync()
        {
            // Load all images to collections.
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var db = new Database())
                {
                    // Load new images from folders
                    var queryOptions = new QueryOptions(CommonFileQuery.OrderByName,
                        new List<string> {".jpg", ".jpeg", ".png", ".bmp"});
                    var newImages = new HashSet<ThumbnailImage>();
                    var folders = db.Folders.ToList();
                    foreach (var folder in folders)
                    {
                        var storageFolder =
                            await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folder.Token);
                        var storageFiles =
                            await storageFolder.CreateFileQueryWithOptions(queryOptions).GetFilesAsync();
                        foreach (var storageFile in storageFiles)
                        {
                            var basicProperties = await storageFile.GetBasicPropertiesAsync();
                            var newImage = new ThumbnailImage
                            {
                                Path = storageFile.Path,
                                Size = basicProperties.Size,
                                DateModified = basicProperties.DateModified
                            };
                            if (newImages.Contains(newImage)) continue;
                            // TODO: Use local cache
                            using (var thumbnail =
                                await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, 300))
                            {
                                var bitmap = new BitmapImage();
                                bitmap.SetSource(thumbnail);
                                newImage.Thumbnail = bitmap;
                                newImages.Add(newImage);
                                Images.Add(newImage);
                            }
                        }
                    }
                }
            });
            // Save new/deleted images to database
            await Window.Current.Dispatcher.RunIdleAsync(args =>
            {
                using (var db = new Database())
                {
                    // Load old images from database
                    var oldImages = db.Images.Include(image => image.Labels).ToList();

                    var intersect = oldImages.Intersect(Images).ToList();
                    // Delete unexisted images in database
                    var deletedImages = oldImages.Except(intersect).ToList();
                    if (deletedImages.Count > 0)
                    {
                        db.RemoveRange(deletedImages);
                        db.SaveChanges();
                    }

                    // Add new images to database
                    var addImages = Images.ToList().ConvertAll(thumbnail => thumbnail.ConvertToImage())
                        .Except(intersect).ToList();
                    // ReSharper disable once InvertIf
                    if (addImages.Count > 0)
                    {
                        db.AddRange(addImages);
                        db.SaveChanges();
                    }
                }
            });
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

        /// <summary>
        ///     Upload images to label them
        /// </summary>
        /// <returns>Void Task</returns>
        public async Task UploadImagesAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var db = new Database())
                {
                    var allImages = db.Images.Include(image => image.Labels).ToList();
                    var unlabeledImages = allImages.Where(image => image.Labels?.Count <= 0).ToList();
                    var folders = db.Folders.ToList();
                    if (unlabeledImages.Count >= 0)
                    {
                        MaxValue = unlabeledImages.Count + 1;
                        foreach (var image in unlabeledImages)
                        {
                            Progress++;
                            var storageFile = await Utils.GetFileAsync(image.Path, folders);
                            using (var fileStream = await storageFile.OpenAsync(FileAccessMode.Read))
                            {
                                // TODO: Upload in low size
                                var streamContent = new HttpStreamContent(fileStream);
                                // Upload image to predict labels
                                image.Labels = await ClassificationHelper.GetLabelsOnMgmlAsync(streamContent);
                                db.Images.Update(image);
                                db.SaveChanges();
                            }
                        }

                        Progress++;
                    }
                }
            });
        }
    }
}