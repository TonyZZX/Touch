#region

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Touch.Database;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public class PlacesViewModel : BaseClassificationViewModel
    {
        public void LoadCovers()
        {
            using (var db = new Context())
            {
                var placeSet = db.Images.Include(image => image.Tags).AsEnumerable()
                    .Select(image => new Image(image).Place).ToHashSet();
                // Remove empty place info
                placeSet.Remove(", ");
                LoadCovers(placeSet, (image, place) => image.Place == place);
            }
        }
    }
}