#region

using System.Linq;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Database;
using Touch.Models;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    public sealed partial class TagDetailPage
    {
        private Cover _cover;
        private TagDetailViewModel _viewModel;

        public TagDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _cover = e.Parameter as Cover;
            _viewModel = new TagDetailViewModel(_cover);

            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                _viewModel.LoadImages();
                using (var db = new Context())
                {
                    var folders = db.Folders.Select(folder => new Folder(folder)).ToList();
                    await _cover.SetOriginalImageAsync(folders);
                }
            });
        }
    }
}