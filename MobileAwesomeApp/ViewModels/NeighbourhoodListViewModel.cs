using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MobileAwesomeApp.Services;

namespace MobileAwesomeApp.ViewModels
{
    public class NeighbourhoodListViewModel
    {
        private readonly IRestaurantService _restaurantService;
        private readonly ObservableCollection<NeighbourhoodViewModel> _neighbourhoods = new ObservableCollection<NeighbourhoodViewModel>();

        public NeighbourhoodListViewModel(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        public ObservableCollection<NeighbourhoodViewModel> Neighbourhoods => _neighbourhoods;

        public async Task Render()
        {
            var results = await _restaurantService.GetNeighbourhoodsAsync();
            Neighbourhoods.Clear();
            foreach (var result in results)
            {
                Neighbourhoods.Add(new NeighbourhoodViewModel(result));
            }
        }
    }
}
