#region

using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Touch.Models;

#endregion

namespace Touch.ViewModels
{
    internal class ObjectDetailsViewModel : SelectedImagesViewModel
    {
        public ObjectDetailsViewModel(LabelObject labelObject)
        {
            LabelObject = labelObject;
        }

        /// <summary>
        ///     Classification object
        /// </summary>
        public LabelObject LabelObject { get; }

        public override async Task LoadImagesAsync()
        {
            var labelIndex = new Category().Get(LabelObject.Name);
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                await LoadImagesAsync(labelIndex,
                    (image, searchLabelIndex) => image.IfContainsLabel(searchLabelIndex));
            });
        }
    }
}