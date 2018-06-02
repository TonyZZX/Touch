#region

using System;
using System.Collections.Generic;
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
    internal abstract class AcrylicGridViewModel
    {
        /// <summary>
        ///     List
        /// </summary>
        public ObservableCollection<LabelObject> LabelObjects;

        protected AcrylicGridViewModel()
        {
            LabelObjects = new ObservableCollection<LabelObject>();
        }

        /// <summary>
        ///     Load available objects from database
        /// </summary>
        /// <typeparam name="T">Classification condition</typeparam>
        /// <param name="distinctTs">HashSet of distinct Ts</param>
        /// <param name="selectFunc">Select image based T</param>
        /// <param name="toNameFunc">T to string name</param>
        /// <returns>Void Task</returns>
        public async Task LoadObjectsAsync<T>(HashSet<T> distinctTs, Func<Image, T, bool> selectFunc,
            Func<T, string> toNameFunc)
        {
            using (var db = new Database())
            {
                var gridItemWidth = Application.Current.Resources["AcrylicGridItemWidth"] as double?;
                if (gridItemWidth != null)
                    foreach (var t in distinctTs)
                    {
                        var image = db.Images.Include(img => img.Labels).ToList().Last(img => selectFunc(img, t));
                        var storageFile = await Utils.GetFileAsync(image.Path, db.Folders.ToList());
                        using (var thumbnail =
                            await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, (uint) gridItemWidth))
                        {
                            var bitmap = new BitmapImage();
                            bitmap.SetSource(thumbnail);
                            var categoryObject = new LabelObject
                            {
                                Name = toNameFunc(t),
                                CoverThumbnail = bitmap
                            };
                            LabelObjects.Add(categoryObject);
                        }
                    }
            }
        }
    }
}