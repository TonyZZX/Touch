#region

using System.Collections.ObjectModel;
using System.Linq;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Touch.Database;
using Touch.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public class SettingsViewModel : PropertyChangedHelper
    {
        private ObservableCollection<Folder> _folders;

        public SettingsViewModel()
        {
            Folders = new ObservableCollection<Folder>();
        }

        public ObservableCollection<Folder> Folders
        {
            get => _folders;
            set => SetProperty(ref _folders, value);
        }

        public void LoadFolders()
        {
            using (var db = new Context())
            {
                var folders = db.Folders.Select(folder => new Folder(folder));
                Folders = new ObservableCollection<Folder>(folders);
            }
        }

        public void AddFolder(StorageFolder storageFolder)
        {
            if (Folders.Any(folder => folder.Path == storageFolder.Path)) return;
            var token = StorageApplicationPermissions.FutureAccessList.Add(storageFolder);
            var newFolder = new Folder {Path = storageFolder.Path, Token = token};
            using (var db = new Context())
            {
                db.Folders.Add(newFolder.ToStorageItemBase());
                db.SaveChanges();
            }

            Folders.Add(newFolder);
        }

        public void RemoveFolder(Folder folder)
        {
            using (var db = new Context())
            {
                db.Folders.Remove(folder.ToStorageItemBase());
                db.SaveChanges();
            }

            StorageApplicationPermissions.FutureAccessList.Remove(folder.Token);
            Folders.Remove(folder);
        }
    }
}