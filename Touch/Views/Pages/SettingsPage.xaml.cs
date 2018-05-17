#region

using System;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Touch.Models;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class SettingsPage
    {
        private readonly SettingsViewModel _viewModel = new SettingsViewModel();

        public SettingsPage()
        {
            InitializeComponent();
        }

        private void SettingsPage_OnLoaded(object sender, RoutedEventArgs e)
        {
            _viewModel.LoadFolders();
        }

        private async void AddFolderBtn_OnClickAsync(object sender, RoutedEventArgs e)
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();
            if (folder == null) return;
            var token = StorageApplicationPermissions.FutureAccessList.Add(folder);
            _viewModel.AddFolder(new Folder {Path = folder.Path, Token = token});
        }

        private void DeleteBtn_OnClick(object sender, RoutedEventArgs e)
        {
            if (!((sender as Button)?.DataContext is Folder clickedfolder)) return;
            _viewModel.RemoveFolder(clickedfolder);
            StorageApplicationPermissions.FutureAccessList.Remove(clickedfolder.Token);
        }
    }
}