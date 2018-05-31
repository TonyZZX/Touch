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

#endregion

namespace Touch.ViewModels
{
    internal class ObjectsViewModel
    {
        /// <summary>
        ///     Object list
        /// </summary>
        public ObservableCollection<CategoryObject> CategoryObjects;

        public ObjectsViewModel()
        {
            CategoryObjects = new ObservableCollection<CategoryObject>();
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

                    var gridItemWidth = Application.Current.Resources["GridItemWidth"] as double?;
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
                                var categoryObject = new CategoryObject
                                {
                                    Name = new Category().Get(index),
                                    Thumbnail = bitmap
                                };
                                CategoryObjects.Add(categoryObject);
                            }
                        }
                }
            });
        }
    }
}