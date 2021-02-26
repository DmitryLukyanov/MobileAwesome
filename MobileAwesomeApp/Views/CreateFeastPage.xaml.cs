using MobileAwesomeApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CreateFeastPage : ContentPage
    {
        public CreateFeastPage()
        {
            InitializeComponent();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = DependencyService.Resolve<CreateFeastViewModel>();
            BindingContext = viewModel;
            await viewModel.Render();
            FeastKeyLabel.Text = viewModel.FeastKey;
        }
    }
}
