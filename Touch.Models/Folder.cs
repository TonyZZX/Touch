#region

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Image folder
    /// </summary>
    public class Folder : IEquatable<Folder>
    {
        /// <summary>
        ///     Folder path
        /// </summary>
        [Key]
        public string Path { get; set; }

        /// <summary>
        ///     Access token in FutureAccessList
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        ///     Symbol used in list
        /// </summary>
        [NotMapped]
        public string Symbol => "\uE8B7";

        public bool Equals(Folder other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Path, other.Path);
        }

        /// <summary>
        ///     Get relative path if exists
        /// </summary>
        /// <param name="filePath">File path</param>
        /// <returns>Return relative path if folder (or subfolders) contains the file, otherwise return empty string.</returns>
        public string GetRelativePath(string filePath)
        {
            // Path.Length + 1: Skip the first \
            return filePath.StartsWith(Path) ? filePath.Substring(Path.Length + 1) : "";
        }
    }
}