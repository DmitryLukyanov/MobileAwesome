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

        private async void ShowNeighbourhoods(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NeighbourhoodsPage));
        }
    }
}
