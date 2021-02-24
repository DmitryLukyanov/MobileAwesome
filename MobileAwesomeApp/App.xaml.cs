using System;
using Autofac;
using MobileAwesomeApp.Infrastructure;
using MobileAwesomeApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var res = DI.Container.Resolve<IRestaurantService>().GetRestaurants("Wild Asia");

            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
