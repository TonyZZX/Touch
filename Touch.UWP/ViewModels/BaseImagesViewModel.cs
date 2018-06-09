#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Touch.Database;
using Touch.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public abstract class BaseImagesViewModel : PropertyChangedHelper
    {
        private IList<Image> _images;

        public IList<Image> Images
        {
            get => _images;
            set => SetProperty(ref _images, value);
        }

        /// <summary>
        ///     Select images from databse based on <paramref name="selectFunc" />
        /// </summary>
        /// <param name="selectFunc">Select function</param>
        protected void LoadImages(Func<Image, bool> selectFunc = null)
        {
            using (var db = new Context())
            {
                var images = db.Images.Include(image => image.Tags).AsEnumerable().Select(image => new Image(image));
                Images = selectFunc == null ? images.ToList() : images.Where(selectFunc).ToList();
            }
        }
    }
}