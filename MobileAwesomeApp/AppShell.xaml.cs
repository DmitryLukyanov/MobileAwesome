using MobileAwesomeApp.Infrastructure;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileAwesomeApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RouteRegistration.RegisterRoutes();
        }
    }
}
