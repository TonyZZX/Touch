#region

using System;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Touch.Database;

#endregion

namespace Touch.Models
{
    public class Folder : IEquatable<Folder>
    {
        private readonly Guid _id;

        public Folder()
        {
        }

        public Folder(StorageItemBase storageItem)
        {
            _id = storageItem.StorageItemBaseId;
            Path = storageItem.Path;
            Token = storageItem.Token;
        }

        public string Path { get; set; }

        public string Token { get; set; }

        public bool Equals(Folder other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Path, other.Path);
        }

        public StorageItemBase ToStorageItemBase()
        {
            var storageItem = new StorageItemBase
            {
                Path = Path,
                Token = Token
            };
            if (_id != Guid.Empty) storageItem.StorageItemBaseId = _id;
            return storageItem;
        }

        /// <summary>
        ///     Get relative path of a file, if exists
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>Return relative path if folder (or subfolders) contains the file, otherwise return empty string.</returns>
        public string GetRelativePath(string filePath)
        {
            // Path.Length + 1: Skip the starting \
            return filePath.StartsWith(Path) ? filePath.Substring(Path.Length + 1) : "";
        }

        /// <summary>
        ///     Get StorageFolder from <see cref="StorageApplicationPermissions.FutureAccessList" />
        /// </summary>
        public async Task<StorageFolder> GetStorageFolderAsync()
        {
            return await StorageApplicationPermissions.FutureAccessList.GetFolderAsync(Token);
        }
    }
}