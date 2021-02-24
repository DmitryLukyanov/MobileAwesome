using MobileAwesomeApp.Infrastructure;
using Xamarin.Forms;

namespace MobileAwesomeApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DI.Initialize();

            MainPage = new AppShell();
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
