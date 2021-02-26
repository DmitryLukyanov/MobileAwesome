using Xamarin.Essentials;

namespace MobileAwesomeApp.Models
{
    public class Coordinates
    {
        public Coordinates(Location location)
        {
            if (location == null) return;

            Latitude = location.Latitude;
            Longitude = location.Longitude;
        }

        public double Latitude { get; private set; }
        public double Longitude { get; private set; }
    }
}
