using System.Collections.Generic;
using Newtonsoft.Json;

namespace Touch.Models
{
    internal class Label
    {
        public static readonly string[] Category =
        {
            "building", "grass", "tree", "cow", "horse", "sheep", "sky", "mountain", "airplane", "water", "face", "car",
            "bicycle", "flower", "sign", "bird", "book", "chair", "road", "cat", "dog", "body", "boat"
        };

        public Label(int num)
        {
            Num = num;
        }

        public int Num { get; set; }

        public string Name => Category[Num];

        public static List<List<Label>> FromJson(string json)
        {
            var jsonResult = JsonConvert.DeserializeObject<List<List<int>>>(json);
            var result = jsonResult.ConvertAll(intList => intList.ConvertAll(num => new Label(num)));
            return result;
        }
    }
}