using System;
using System.Threading.Tasks;
using MobileAwesomeApp.Models;
using MobileAwesomeApp.Services;
using Xamarin.Essentials;

namespace MobileAwesomeApp.ViewModels
{
    public class JoinFeastViewModel
    {
        private readonly IFeastService _feastService;
        private string[] _foods = new string[4];

        public JoinFeastViewModel(IFeastService feastService)
        {
            _feastService = feastService;
        }

        public string[] Foods => _foods;

        public async Task JoinFeast()
        {
            var location = await Geolocation.GetLocationAsync();
            var user = new User {Email = "john@example.com", FirstName = "John", LastName = "Example", CurrentLocation = new Coordinates(location)};

            for (var i=0; i<_foods.Length; i++)
            {
                _foods[i] = _foods[i].ToLower();
            }

            var feastKey = string.Join("-", _foods);

            var feast = await _feastService.FindFeastByFeastKeyAsync(feastKey);
            if (feast == null)
            {
                throw new ArgumentException($"Unknown feast key: {feastKey}");
            }
            feast.AddParticipant(user);
            await _feastService.UpdateFeastPartipcantsAsync(feast);
        }
    }
}
