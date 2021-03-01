using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MobileAwesomeApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowFeastsPage : ContentPage
    {
        private const int PADDING_WIDTH = 0;

        private int paddingHeight;

        public ShowFeastsPage()
        {
            InitializeComponent();

            CheckDevice();

            stackLayout.Padding = new Thickness(PADDING_WIDTH, GetPaddingHeight());
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

            var viewModel = DependencyService.Resolve<ShowFestsViewModel>();
            BindingContext = viewModel;
        }
    }
}