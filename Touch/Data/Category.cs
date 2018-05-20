#region

using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Touch.Data
{
    internal class Category
    {
        /// <summary>
        ///     23 kinds of different labels
        /// </summary>
        private readonly string[] _data =
        {
            "Building", "Grass", "Tree", "Cow", "Horse", "Sheep", "Sky", "Mountain", "Airplane", "Water", "Face", "Car",
            "Bicycle", "Flower", "Sign", "Bird", "Book", "Chair", "Road", "Cat", "Dog", "Body", "Boat"
        };

        /// <summary>
        ///     Get label in category based on index
        /// </summary>
        /// <param name="index">Label index</param>
        /// <returns>Return label if exists, otherwise return empty string</returns>
        public string Get(int index)
        {
            return index < _data.Length ? _data[index] : "";
        }

        /// <summary>
        ///     Get matched list based on text.
        /// </summary>
        /// <param name="query">Label text</param>
        /// <returns>Matched list</returns>
        public IList<string> GetMatchList(string query)
        {
            return _data.Where(item => item.IndexOf(query, StringComparison.CurrentCultureIgnoreCase) >= 0)
                .OrderBy(s => s).ToList();
        }

        /// <summary>
        ///     Check if text is in category
        /// </summary>
        /// <param name="text">Label text</param>
        /// <returns>Whether text is in category</returns>
        public bool IsInCategory(string text)
        {
            return _data.Contains(text);
        }
    }
}