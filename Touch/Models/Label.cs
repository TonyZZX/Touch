#region

using System.Collections.Generic;
using Newtonsoft.Json;
using Touch.Data;

#endregion

namespace Touch.Models
{
    /// <summary>
    ///     Image label
    /// </summary>
    internal class Label
    {
        /// <summary>
        ///     Image label
        /// </summary>
        /// <param name="num">Label number in category</param>
        public Label(int num)
        {
            Num = num;
        }

        /// <summary>
        ///     Label number in category
        /// </summary>
        public int Num { get; set; }

        /// <summary>
        ///     Label name in category
        /// </summary>
        public string Name => new Category().Get(Num);

        /// <summary>
        ///     Generate labels from JSON
        /// </summary>
        /// <param name="json">JSON formate string</param>
        /// <returns>Labels for each image</returns>
        public static List<List<Label>> FromJson(string json)
        {
            return JsonConvert.DeserializeObject<List<List<int>>>(json)
                .ConvertAll(intList => intList.ConvertAll(num => new Label(num)));
        }
    }
}