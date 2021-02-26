using System.Threading.Tasks;
using MobileAwesomeApp.Models;
using MobileAwesomeApp.Services;
using Xamarin.Essentials;

namespace MobileAwesomeApp.ViewModels
{
    public class CreateFeastViewModel
    {
        private readonly IFeastService _feastService;
        private string _feastKey;

        public CreateFeastViewModel(IFeastService feastService)
        {
            _feastService = feastService;
        }

        public string FeastKey => _feastKey;

        public async Task Render()
        {
            var location = await Geolocation.GetLocationAsync();
            var user = new User {Email = "sally@example.com", FirstName = "Sally", LastName = "Example", CurrentLocation = new Coordinates(location)};
            var feast = await _feastService.CreateNewFeastAsync(user);
            _feastKey = feast.FeastKey;
        }
    }
}
