using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MobileAwesomeApp.Models;
using MobileAwesomeApp.Services;
using Xamarin.Forms.Extended;

namespace MobileAwesomeApp.ViewModels
{
    public class RestaurantsListViewModel : INotifyPropertyChanged
    {
        private bool _isBusy;
        private const int PageSize = 10;

        private readonly IRestaurantService _restaurantService;

        public RestaurantsListViewModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
            
            Items = new InfiniteScrollCollection<RestaurantViewModel>
            {
                OnLoadMore = async () =>
                {
                    IsBusy = true;

                    // load the next page
                    var page = Items.Count / PageSize;

                    var items = await _restaurantService.GetRestaurantsAsync(page: page, pageSize : PageSize);

                    IsBusy = false;

                    // return the items that need to be added
                    return CastRestaurantsToViewModel(items);
                },
                OnCanLoadMore = () =>
                {
                    return true;
                }
            };
        }

        public InfiniteScrollCollection<RestaurantViewModel> Items { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                OnPropertyChanged();
            }
        }

        public async Task RenderAsync()
        {
            var items = await _restaurantService.GetRestaurantsAsync(page: 0, pageSize: PageSize);

            Items.AddRange(CastRestaurantsToViewModel(items));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private IEnumerable<RestaurantViewModel> CastRestaurantsToViewModel(IEnumerable<Restaurant> restaurants)
        {
            return restaurants.Select(r => new RestaurantViewModel(r));
        }
    }
}
