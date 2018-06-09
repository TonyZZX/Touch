#region

using System;

#endregion

namespace Touch.ViewModels
{
    public class SearchViewModel : BaseImageGroupsViewModel
    {
        private readonly string[] _tags;

        public SearchViewModel(string queryStr)
        {
            _tags = queryStr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        }

        public void LoadImageGroups()
        {
            LoadImageGroups(image => image.IfContainsTags(_tags));
        }
    }
}