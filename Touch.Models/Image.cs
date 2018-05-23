#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Image
    /// </summary>
    public class Image : IEquatable<Image>
    {
        /// <summary>
        ///     Image path
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Image size
        /// </summary>
        public ulong Size { get; set; }

        /// <summary>
        ///     Last modified date
        /// </summary>
        public DateTimeOffset DateModified { get; set; }

        /// <summary>
        ///     Multiple labels that image contains
        /// </summary>
        public IList<Label> Labels { get; set; }

        public bool Equals(Image other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Path, other.Path) && Size == other.Size && DateModified.Equals(other.DateModified);
        }

        /// <summary>
        ///     If contains specific label
        /// </summary>
        /// <param name="index">Label index in category</param>
        /// <returns>If contains</returns>
        public bool IfContainsLabel(int index)
        {
            return Labels != null && Labels.Any(label => label.Index == index);
        }

        /// <summary>
        ///     If contains specific labels
        /// </summary>
        /// <param name="index">Label index in category</param>
        /// <returns>If contains</returns>
        public bool IfContainsLabel(IList<int> index)
        {
            return Labels != null && !index.Except(Labels.Select(label=>label.Index)).Any();
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Image) obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            unchecked
            {
                var hashCode = Path != null ? Path.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ DateModified.GetHashCode();
                return hashCode;
            }
        }
    }
}