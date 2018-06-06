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
        ///     Image date.
        ///     Most time it is the date that photo was took. If the taken time is empty,
        ///     it will be the date that file was last modified.
        /// </summary>
        public DateTimeOffset Date { get; set; }

        /// <summary>
        ///     Multiple labels that image contains
        /// </summary>
        public IList<Label> Labels { get; set; }

        public bool Equals(Image other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Path, other.Path) && Size == other.Size && Date.Equals(other.Date);
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
        public bool IfContainsLabel(IEnumerable<int> index)
        {
            return Labels != null && !index.Except(Labels.Select(label => label.Index)).Any();
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
                hashCode = (hashCode * 397) ^ Date.GetHashCode();
                return hashCode;
            }
        }
    }
}