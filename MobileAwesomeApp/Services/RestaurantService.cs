using System.Linq;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Models;
using MongoDB.Driver;

namespace MobileAwesomeApp.Services
{
    public interface IRestaurantService
    {
        Restaurant GetRestaurant(string name);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly MongoClientWrapper _client;
        private readonly CollectionNamespace _restaurantCollection;

        public RestaurantService(MongoClientWrapper client)
        {
            _client = client;
            _restaurantCollection = CollectionNamespace.FromFullName("sample_restaurants.restaurants");
        }

        public Restaurant GetRestaurant(string name)
        {
            var allRestaurants = _client.GetCollection<Restaurant>(_restaurantCollection).Find(FilterDefinition<Restaurant>.Empty).ToList();

            var restaurants = _client.GetCollection<Restaurant>(_restaurantCollection).Find(c=>c.Name == name).ToList();
            return restaurants.Single();
        }
    }
}
