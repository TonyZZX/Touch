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
    public sealed partial class DateDetailPage
    {
        private Cover _cover;
        private DateDetailViewModel _viewModel;

        public DateDetailPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _cover = e.Parameter as Cover;
            _viewModel = new DateDetailViewModel(_cover);

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