using MobileAwesomeApp.Views;
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
            //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            Routing.RegisterRoute(nameof(NeighbourhoodsPage), typeof(NeighbourhoodsPage));
        }
    }
}
