#region

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Touch.Services;
using Touch.ViewModels;

#endregion

namespace Touch.Views.Pages
{
    internal sealed partial class DatesPage : IPageWithViewModel<DatesViewModel>
    {
        public DatesPage()
        {
            InitializeComponent();
        }

        public DatesViewModel ViewModel { get; set; }

        public void UpdateBindings()
        {
            Bindings?.Update();
        }

        private async void DatesPage_OnLoadedAsync(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadObjectsAsync();
        }

        private void GridViewControl_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ViewModel.NavigateToDetailsage(e.ClickedItem);
        }
    }
}