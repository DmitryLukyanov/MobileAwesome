using MobileAwesomeApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RestaurantsPage : ContentPage
    {
        //public RestaurantsPage()
        //{
        //    InitializeComponent();
        //}

        //protected override async void OnAppearing()
        //{
        //    base.OnAppearing();

        //    var viewModel = DependencyService.Resolve<RestaurantsListViewModel>();
        //    BindingContext = viewModel;
        //    await viewModel.Render();
        //}
        //private const string URL = "https://www.google.com";
        private const int PADDING_WIDTH = 0;

        private int paddingHeight;

        private WebView webView;
        public HtmlWebViewSource Html
        {
            get
            {
                var source = @"<iframe style=""background: #FFFFFF;border: none;border-radius: 2px;box-shadow: 0 2px 10px 0 rgba(70, 76, 79, .2);"" width=""640"" height=""480"" src=""https://charts.mongodb.com/charts-mobileawesome-ldwiw/embed/charts?id=1a56c5d5-0a18-4f5f-9adf-2aef9b453e8d&theme=light""></iframe>";
                //"<html><body><p>yyy</p></body></html>";
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = source;
                return htmlSource;
            }
        }
        public RestaurantsPage()
        {
            webView = new WebView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            CheckDevice();

            Content = new StackLayout
            {
                Padding = new Thickness(PADDING_WIDTH, GetPaddingHeight()),
                Children = { webView }
            };
        }

        public int GetPaddingHeight()
        {
            return paddingHeight;
        }

        /// <summary>
        /// This will set the padding height for the webview when displayed
        /// <summary>
        /// <param name="pHeight">Set integer value for the padding height.</param>
        public void SetPaddingHeight(int pHeight)
        {
            paddingHeight = pHeight;
        }

        private void CheckDevice()
        {
            if (Device.OS == TargetPlatform.Android)
            {
                SetPaddingHeight(0);
            }
            else if (Device.OS == TargetPlatform.iOS)
            {
                SetPaddingHeight(20);
            }
        }
        
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            var viewModel = DependencyService.Resolve<RestaurantsListViewModel>();
            BindingContext = viewModel;
            await viewModel.Render();

            webView.Source = Html;
        }
    }
}