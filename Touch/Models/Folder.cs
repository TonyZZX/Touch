#region

using System;
using Windows.Storage.AccessCache;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Image folder
    /// </summary>
    internal class Folder : IEquatable<Folder>
    {
        /// <summary>
        ///     Folder path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Access token in <see cref="StorageApplicationPermissions.FutureAccessList" />
        /// </summary>
        public string Token { get; set; }

        public bool Equals(Folder other)
        {
            return Path == other.Path;
        }
    }
}