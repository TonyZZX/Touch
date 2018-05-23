#region

using System.Collections;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Group on image date
    /// </summary>
    internal class ImageGroup : IGrouping<string, ThumbnailImage>
    {
        private readonly IEnumerable<ThumbnailImage> _images;

        public ImageGroup(string date, IEnumerable<ThumbnailImage> images)
        {
            Key = date;
            _images = images;
        }

        public string Key { get; }

        public IEnumerator<ThumbnailImage> GetEnumerator()
        {
            return _images.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}