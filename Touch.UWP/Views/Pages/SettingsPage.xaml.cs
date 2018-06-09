#region

using System;
using Windows.ApplicationModel;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Autofac;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Models;
using Touch.Services;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    public sealed partial class SettingsPage
    {
        private readonly SettingsViewModel _viewModel;

        private bool _ifContentChanged;

        public SettingsPage()
        {
            InitializeComponent();

            _viewModel = new SettingsViewModel();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                _viewModel.LoadFolders();

                // Update "About this app" section
                var package = Package.Current;
                var version = package.Id.Version;
                VersionText.Text =
                    $"{package.DisplayName} {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            });
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            if (_ifContentChanged)
                using (var scope = App.Container.BeginLifetimeScope())
                {
                    scope.Resolve<IScanImageTask>().Start();
                }

            base.OnNavigatedFrom(e);
        }

        private async void FolderList_OnItemClickAsync(object sender, ItemClickEventArgs e)
        {
            if (!(e.ClickedItem is Folder folder)) return;
            var storageFolder = await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(folder.Token);
            await Launcher.LaunchFolderAsync(storageFolder);
        }

        private async void AddFolderList_OnItemClickAsync(object sender, ItemClickEventArgs e)
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            folderPicker.FileTypeFilter.Add("*");
            var storageFolder = await folderPicker.PickSingleFolderAsync();
            if (storageFolder == null) return;
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => { _viewModel.AddFolder(storageFolder); });
            _ifContentChanged = true;
        }

        private async void DeleteBtn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            if (!((sender as Button)?.DataContext is Folder clickedfolder)) return;
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => { _viewModel.RemoveFolder(clickedfolder); });
            _ifContentChanged = true;
        }

        private async void FeedbackBtn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(Constants.IssueUrl));
        }
    }
}