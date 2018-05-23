#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Http;
using Newtonsoft.Json;
using Touch.Models;

#endregion

namespace Touch.Helpers
{
    /// <summary>
    ///     Get images' labels
    /// </summary>
    internal class ClassificationHelper
    {
        /// <summary>
        ///     Predict labels for one image based on MGML
        /// </summary>
        /// <param name="streamContent">Image file stream</param>
        /// <returns>Multi-Label of the iamge</returns>
        public static async Task<IEnumerable<Label>> GetLabelsOnMgmlAsync(HttpStreamContent streamContent)
        {
            using (var httpClient = new HttpClient())
            {
                // TODO: Single Instance
                using (var httpResponse =
                    await httpClient.PostAsync(new Uri("http://59.110.137.131:1696"), streamContent))
                {
                    var content = await httpResponse.Content.ReadAsStringAsync();
                    var trueContent = string.Join("", content.Split('\r', '\n').Skip(2));
                    var labels = JsonConvert.DeserializeObject<IEnumerable<IEnumerable<int>>>(trueContent)
                        .Select(intList => intList.Select(num => new Label {Index = num}));
                    return labels.First();
                }
            }
        }
    }
}