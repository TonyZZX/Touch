#region

using System.Linq;
using Microsoft.EntityFrameworkCore;
using Touch.Database;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    public class DatesViewModel : BaseClassificationViewModel
    {
        public void LoadCovers()
        {
            using (var db = new Context())
            {
                var dateSet = db.Images.Include(image => image.Tags).AsEnumerable()
                    .Select(image => new Image(image).MonthYearDate.ToString()).ToHashSet();
                LoadCovers(dateSet, (image, date) => image.MonthYearDate.ToString() == date);
            }
        }
    }
}