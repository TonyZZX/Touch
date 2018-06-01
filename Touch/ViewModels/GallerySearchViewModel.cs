#region

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class GallerySearchViewModel : SelectedImagesViewModel
    {
        /// <summary>
        ///     Index for the query label
        /// </summary>
        private readonly IList<int> _searchLabelIndex;

        /// <summary>
        ///     Group on date
        /// </summary>
        private IEnumerable<ImageGroup> _imageGroup;

        public GallerySearchViewModel(string queryStr)
        {
            _searchLabelIndex = new List<int>();
            var labels = queryStr.Split(' ');
            foreach (var label in labels)
            {
                var index = label != null ? new Category().Get(label) : -1;
                if (index != -1)
                    _searchLabelIndex.Add(index);
            }
        }

        /// <summary>
        ///     Group on date
        /// </summary>
        public IEnumerable<ImageGroup> ImageGroup
        {
            get => _imageGroup;
            protected set => SetProperty(ref _imageGroup, value);
        }

        /// <summary>
        ///     Load images based on query
        /// </summary>
        /// <returns>Void Task</returns>
        public override async Task LoadImagesAsync()
        {
            if (!_searchLabelIndex.Any()) return;
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                await LoadImagesAsync(_searchLabelIndex,
                    (image, searchLabelIndex) => image.IfContainsLabel(searchLabelIndex));
                ImageGroup = Images.GroupBy(image => image.MonthYear, (key, list) => new ImageGroup(key, list));
            });
        }
    }
}