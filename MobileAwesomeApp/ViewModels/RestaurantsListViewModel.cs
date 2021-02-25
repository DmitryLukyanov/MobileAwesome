using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MobileAwesomeApp.Services;

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
