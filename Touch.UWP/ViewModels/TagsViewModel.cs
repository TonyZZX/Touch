#region

using System.Linq;
using Touch.Database;

#endregion

namespace Touch.ViewModels
{
    public class TagsViewModel : BaseClassificationViewModel
    {
        public void LoadCovers()
        {
            using (var db = new Context())
            {
                var tagSet = db.Tags.Select(tag => tag.Name).ToHashSet();
                LoadCovers(tagSet, (image, tagName) => image.IfContainsTag(tagName));
            }
        }
    }
}