#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Touch.Models
{
    public class ImageGroup : IGrouping<MonthYear, Image>
    {
        private readonly IEnumerable<Image> _images;

        public ImageGroup(MonthYear date, IEnumerable<Image> images)
        {
            Key = date;
            _images = images;
        }

        public MonthYear Key { get; }

        public IEnumerator<Image> GetEnumerator()
        {
            return _images.OrderByDescending(image => image.DisplayDate).ThenBy(image => image.Name).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}