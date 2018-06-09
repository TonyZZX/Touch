#region

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Touch.Database;
using Touch.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public abstract class BaseClassificationViewModel : PropertyChangedHelper
    {
        private ObservableCollection<Cover> _covers;

        protected BaseClassificationViewModel()
        {
            Covers = new ObservableCollection<Cover>();
        }

        public ObservableCollection<Cover> Covers
        {
            get => _covers;
            set => SetProperty(ref _covers, value);
        }

        /// <summary>
        ///     Load classification covers based on <paramref name="selectFunc" />
        /// </summary>
        /// <param name="hashSet">Distinct t for classification</param>
        /// <param name="selectFunc">Select function</param>
        protected void LoadCovers<T>(HashSet<T> hashSet, Func<Image, T, bool> selectFunc)
        {
            var orderedSet = hashSet.OrderBy(t => t);
            using (var db = new Context())
            {
                foreach (var t in orderedSet)
                {
                    var coverImage = db.Images.Include(image => image.Tags).AsEnumerable()
                        .Select(image => new Image(image)).Last(image => selectFunc(image, t));
                    var cover = new Cover(coverImage)
                    {
                        Name = t.ToString()
                    };
                    Covers.Add(cover);
                }
            }
        }
    }
}