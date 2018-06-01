#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Helpers;
using Touch.Models;
using Touch.Services;
using Touch.Views.Pages;

#endregion

namespace Touch.ViewModels
{
    internal class ObjectsViewModel
    {
        private readonly INavigationService _navigationService;

        /// <summary>
        ///     Object list
        /// </summary>
        public ObservableCollection<LabelObject> LabelObjects;

        public ObjectsViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            LabelObjects = new ObservableCollection<LabelObject>();
        }

        /// <summary>
        ///     Load available objects from database
        /// </summary>
        /// <returns></returns>
        public async Task LoadObjectsAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                using (var db = new Database())
                {
                    var distinctLabels = db.Labels.Select(label => label.Index).ToHashSet();

                    var gridItemWidth = Application.Current.Resources["AcrylicGridItemWidth"] as double?;
                    if (gridItemWidth != null)
                        foreach (var index in distinctLabels)
                        {
                            var image = db.Images.Include(img => img.Labels).ToList()
                                .Last(img => img.IfContainsLabel(index));
                            var storageFile = await Utils.GetFileAsync(image.Path, db.Folders.ToList());
                            using (var thumbnail =
                                await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, (uint) gridItemWidth))
                            {
                                var bitmap = new BitmapImage();
                                bitmap.SetSource(thumbnail);
                                var categoryObject = new LabelObject
                                {
                                    Name = new Category().Get(index),
                                    CoverThumbnail = bitmap
                                };
                                LabelObjects.Add(categoryObject);
                            }
                        }
                }
            });
        }

        /// <summary>
        ///     Navigate to <see cref="ObjectDetailsPage" />
        /// </summary>
        /// <param name="labelObject">Classification object</param>
        public void NavigateToDetailsage(object labelObject)
        {
            _navigationService.NavigateAsync(typeof(ObjectDetailsPage), labelObject);
        }
    }
}