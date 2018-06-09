#region

using System;
using Windows.Globalization.DateTimeFormatting;

#endregion

namespace Touch.Models
{
    public class MonthYear : IEquatable<MonthYear>, IComparable<MonthYear>
    {
        private readonly int _month;
        private readonly DateTimeOffset _offset;
        private readonly int _year;

        public MonthYear(DateTimeOffset offset)
        {
            _offset = offset;
            _month = offset.Month;
            _year = offset.Year;
        }

        public int CompareTo(MonthYear other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (other is null) return 1;
            var yearComparison = _year.CompareTo(other._year);
            return yearComparison != 0 ? yearComparison : _month.CompareTo(other._month);
        }

        public bool Equals(MonthYear other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            return _month == other._month && _year == other._year;
        }

        public override string ToString()
        {
            return new DateTimeFormatter("month year").Format(_offset);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((MonthYear) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (_month * 397) ^ _year;
            }
        }
    }
}