using System;
using MobileAwesomeApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JoinFeastPage : ContentPage
    {
        private JoinFeastViewModel _viewModel;

        public JoinFeastPage()
        {
            InitializeComponent();

            _viewModel = DependencyService.Resolve<JoinFeastViewModel>();
            BindingContext = _viewModel;
        }

        private async void JoinFeast(object sender, EventArgs e)
        {
            await _viewModel.JoinFeast();
            await DisplayAlert("Success", "Feast joined!", "OK");
            await Navigation.PopAsync();
        }
    }
}
