#region

using System;
using System.Collections.Generic;
using System.Linq;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public abstract class BaseImageGroupsViewModel : BaseImagesViewModel
    {
        private IList<ImageGroup> _imageGroups;

        public IList<ImageGroup> ImageGroups
        {
            get => _imageGroups;
            set => SetProperty(ref _imageGroups, value);
        }

        /// <summary>
        ///     Group images based on date
        /// </summary>
        /// <returns>List of image groups</returns>
        public void LoadImageGroups(Func<Image, bool> selectFunc = null)
        {
            LoadImages(selectFunc);
            ImageGroups = Images.GroupBy(image => image.MonthYearDate, (key, list) => new ImageGroup(key, list))
                .OrderByDescending(group => group.Key).ToList();
        }
    }
}