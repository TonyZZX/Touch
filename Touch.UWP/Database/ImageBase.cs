#region

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Touch.Database
{
    public class ImageBase : IEquatable<ImageBase>
    {
        public ImageBase()
        {
            ImageBaseId = Guid.NewGuid();
        }

        [Key] public Guid ImageBaseId { get; set; }

        public string Name { get; set; }

        public string FileType { get; set; }

        public string Path { get; set; }

        public ulong Size { get; set; }

        public DateTimeOffset DateModified { get; set; }

        public uint Height { get; set; }

        public uint Width { get; set; }

        public DateTimeOffset DateTaken { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public string Town { get; set; }

        public string District { get; set; }

        public string Region { get; set; }

        public IList<TagBase> Tags { get; set; }

        public bool Equals(ImageBase other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Path, other.Path) && Size == other.Size && DateModified.Equals(other.DateModified) &&
                   Height == other.Height && Width == other.Width && DateTaken.Equals(other.DateTaken) &&
                   Latitude.Equals(other.Latitude) && Longitude.Equals(other.Longitude);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ImageBase) obj);
        }

        public override int GetHashCode()
        {
            // ReSharper disable NonReadonlyMemberInGetHashCode
            unchecked
            {
                var hashCode = Path != null ? Path.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Size.GetHashCode();
                hashCode = (hashCode * 397) ^ DateModified.GetHashCode();
                hashCode = (hashCode * 397) ^ (int) Height;
                hashCode = (hashCode * 397) ^ (int) Width;
                hashCode = (hashCode * 397) ^ DateTaken.GetHashCode();
                hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Longitude.GetHashCode();
                return hashCode;
            }
        }
    }
}