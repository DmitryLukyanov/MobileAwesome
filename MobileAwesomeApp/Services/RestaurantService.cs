using System.Collections.Generic;
using System.Threading.Tasks;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MobileAwesomeApp.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string name);
        Task<IEnumerable<Restaurant>> GetRestaurantByNeighborhoodAsync(string neighborhood);
        Task<IEnumerable<Restaurant>> GetRestaurantByCuisineAsync(string cuisine);
        Task<IEnumerable<Neighbourhood>> GetNeighbourhoodAsync(string name);
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

        public Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string name)
        {
            return GetEntityByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "name", value: name);
        }

        public Task<IEnumerable<Restaurant>> GetRestaurantByNeighborhoodAsync(string neighborhood)
        {
            return GetEntityByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "borough", value: neighborhood);
        }

        public Task<IEnumerable<Restaurant>> GetRestaurantByCuisineAsync(string cuisine)
        {
            return GetEntityByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "cuisine", value: cuisine);
        }

        public Task<IEnumerable<Neighbourhood>> GetNeighbourhoodAsync(string name)
        {
            return GetEntityByFieldAsync<Neighbourhood>(_neighbourhoodCollectionNamespace, field: "name", value: name);
        }

        private async Task<IEnumerable<TEntity>> GetEntityByFieldAsync<TEntity>(CollectionNamespace collection, string field, string value)
        {
            var searchStage = BsonDocument.Parse(
                $@"{{
                    '$search': {{
                        'text': {{
                            'query': '{value}', 
                            'path': '{field}', 
                            'fuzzy': {{
                                'maxEdits': 1
                            }}
                        }}
                    }}
                }}");
            var pipeline = new EmptyPipelineDefinition<TEntity>()
                .AppendStage<TEntity, TEntity, TEntity>(searchStage);
            var entities = await _client
                .GetCollection<TEntity>(collection)
                .AggregateAsync(pipeline)
                .ConfigureAwait(false);

            return await entities.ToListAsync().ConfigureAwait(false);
        }
    }
}
