#region

using System;
using Touch.Database;

#endregion

namespace Touch.Models
{
    public class Tag : IEquatable<Tag>
    {
        private readonly Guid _id;

        public Tag()
        {
        }

        public Tag(TagBase tagBase)
        {
            _id = tagBase.TagBaseId;
            Name = tagBase.Name;
        }

        public string Name { get; set; }

        public bool Equals(Tag other)
        {
            if (other is null) return false;
            return ReferenceEquals(this, other) || string.Equals(Name, other.Name);
        }

        public TagBase ToTagBase()
        {
            var tag = new TagBase
            {
                Name = Name
            };
            if (_id != Guid.Empty) tag.TagBaseId = _id;
            return tag;
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((Tag) obj);
        }

        // ReSharper disable once NonReadonlyMemberInGetHashCode
        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }
    }
}