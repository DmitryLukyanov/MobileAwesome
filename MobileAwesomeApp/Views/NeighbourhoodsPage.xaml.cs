using MobileAwesomeApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NeighbourhoodsPage : ContentPage
    {
        public NeighbourhoodsPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = DependencyService.Resolve<NeighbourhoodListViewModel>();
            BindingContext = viewModel;
            await viewModel.Render();
        }
    }
}
