using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ShowRestaurants(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(RestaurantsPage));
        }

        private async void ShowNeighbourhoods(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NeighbourhoodsPage));
        }

        private async void CreateFeast(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(CreateFeastPage));
        }

        private async void JoinFeast(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(JoinFeastPage));
        }

        private async void ShowFeasts(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ShowFeastsPage));
        }
    }
}
