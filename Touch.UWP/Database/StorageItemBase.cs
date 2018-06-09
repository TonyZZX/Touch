#region

using System;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Touch.Database
{
    // Could be folder or file
    public class StorageItemBase : IEquatable<StorageItemBase>
    {
        public StorageItemBase()
        {
            StorageItemBaseId = Guid.NewGuid();
        }

        [Key] public Guid StorageItemBaseId { get; set; }

        public string Path { get; set; }

        /// <summary>
        ///     Access token in FutureAccessList
        /// </summary>
        public string Token { get; set; }

        public bool Equals(StorageItemBase other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Path, other.Path);
        }
    }
}