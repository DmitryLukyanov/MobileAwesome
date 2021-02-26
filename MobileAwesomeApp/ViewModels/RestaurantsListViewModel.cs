using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MobileAwesomeApp.Services;
using Xamarin.Forms;

namespace MobileAwesomeApp.ViewModels
{
    public class RestaurantsListViewModel
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ObservableCollection<RestaurantViewModel> _restaurants = new ObservableCollection<RestaurantViewModel>();

        public RestaurantsListViewModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public ObservableCollection<RestaurantViewModel> Restaurants => _restaurants;

        public HtmlWebViewSource Html
        {
            get
            {
                var source = @"<iframe style=""background: #FFFFFF;border: none;border-radius: 2px;box-shadow: 0 2px 10px 0 rgba(70, 76, 79, .2);"" width=""640"" height=""480"" src=""https://charts.mongodb.com/charts-mobileawesome-ldwiw/embed/charts?id=1a56c5d5-0a18-4f5f-9adf-2aef9b453e8d&theme=light""></iframe>";
                var htmlSource = new HtmlWebViewSource();
                htmlSource.Html = source;
                return htmlSource;
            }
        }

        public async Task Render()
        {
            var results = await _restaurantService.GetRestaurantsAsync();
            Restaurants.Clear();
            foreach (var result in results)
            {
                Restaurants.Add(new RestaurantViewModel(result));
            }
        }
    }
}
