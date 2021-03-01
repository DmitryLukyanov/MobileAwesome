using MobileAwesomeApp.Models;
using Xamarin.Forms;

namespace MobileAwesomeApp.ViewModels
{
    public class RestaurantViewModel
    {
        public string Cuisine { get; private set; }
        public string Name { get; private set; }

        public RestaurantViewModel(Restaurant restaurant)
        {
            Cuisine = restaurant.Cuisine;
            Name = restaurant.Name;
        }
    }
}
