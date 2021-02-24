using System.Collections.Generic;
using System.Linq;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Models;
using MongoDB.Driver;

namespace MobileAwesomeApp.Services
{
    public interface IRestaurantService
    {
        Restaurant GetRestaurant(string name);
        Neighbourhood GetNeighbourhood(string name);
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly MongoClientWrapper _client;
        private readonly CollectionNamespace _neighbourhoodCollectionNamespace;
        private readonly CollectionNamespace _restaurantCollectionNamespace;

        public RestaurantService(MongoClientWrapper client)
        {
            _client = client;
            _neighbourhoodCollectionNamespace = CollectionNamespace.FromFullName("sample_restaurants.neighborhoods");
            _restaurantCollectionNamespace = CollectionNamespace.FromFullName("sample_restaurants.restaurants");
        }

        public Restaurant GetRestaurant(string name)
        {
            var restaurant = _client.GetCollection<Restaurant>(_restaurantCollectionNamespace).Find(c=>c.Name == name).Single();
            return restaurant;
        }

        public IEnumerable<Restaurant> GetRestaurantByCuisine(string cuisine)
        {
            var restaurants = _client.GetCollection<Restaurant>(_restaurantCollectionNamespace).Find(c => c.Cuisine == cuisine).ToList();
            return restaurants;
        }

        public Neighbourhood GetNeighbourhood(string name)
        {
            var neighbourhoodCollection = _client.GetCollection<Neighbourhood>(_neighbourhoodCollectionNamespace);
            var neighbourhood = neighbourhoodCollection.Find(c => c.Name == name).SingleOrDefault();
            return neighbourhood;
        }
    }
}
