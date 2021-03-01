using System;
using System.Windows.Input;
using MobileAwesomeApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantsPage : ContentPage
    {
        private readonly RestaurantsListViewModel _viewModel;

        public RestaurantsPage()
        {
            InitializeComponent();
            _viewModel = DependencyService.Resolve<RestaurantsListViewModel>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            BindingContext = _viewModel;

            await _viewModel.RenderAsync();
        }
    }
}