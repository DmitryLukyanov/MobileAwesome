using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MobileAwesomeApp.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync();
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string name);
        Task<IEnumerable<Restaurant>> GetRestaurantByNeighborhoodAsync(string neighborhood);
        Task<IEnumerable<Restaurant>> GetRestaurantByCuisineAsync(string cuisine);
        
        Task<IEnumerable<Neighbourhood>> GetNeighbourhoodsAsync(string name);
        Task<IEnumerable<Neighbourhood>> GetNeighbourhoodsAsync();
        
        Task<IEnumerable<Restaurant>> GetEntitiesByGeoWithinAsync();
    }

    public class RestaurantService : IRestaurantService
    {
        private readonly MongoClientWrapper _client;
        private readonly CollectionNamespace _neighbourhoodCollectionNamespace;
        private readonly CollectionNamespace _restaurantCollectionNamespace;
        private readonly CollectionNamespace _restaurantCollectionNamespace1; // temp

        public RestaurantService(MongoClientWrapper client)
        {
            _client = client;
            _neighbourhoodCollectionNamespace = CollectionNamespace.FromFullName("sample_restaurants.neighborhoods");
            _restaurantCollectionNamespace = CollectionNamespace.FromFullName("sample_restaurants.restaurants");
            _restaurantCollectionNamespace1 = CollectionNamespace.FromFullName("sample_restaurants.restaurants1");
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync()
        {
            var cursor = await _client.GetCollection<Restaurant>(_restaurantCollectionNamespace).FindAsync(FilterDefinition<Restaurant>.Empty).ConfigureAwait(false);
            var value = await cursor.ToListAsync().ConfigureAwait(false);
            return value.Take(10);  // temporary fix
        }

        public Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string name)
        {
            return GetEntitiesByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "name", value: name);
        }

        public Task<IEnumerable<Restaurant>> GetRestaurantByNeighborhoodAsync(string neighborhood)
        {
            return GetEntitiesByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "borough", value: neighborhood);
        }

        public Task<IEnumerable<Restaurant>> GetRestaurantByCuisineAsync(string cuisine)
        {
            return GetEntitiesByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "cuisine", value: cuisine);
        }

        public Task<IEnumerable<Neighbourhood>> GetNeighbourhoodsAsync(string name)
        {
            return GetEntitiesByFieldAsync<Neighbourhood>(_neighbourhoodCollectionNamespace, field: "name", value: name);
        }

        public async Task<IEnumerable<Neighbourhood>> GetNeighbourhoodsAsync()
        {
            var neighbourhoods = await _client.GetCollection<Neighbourhood>(_neighbourhoodCollectionNamespace).AsQueryable().ToListAsync();
            return neighbourhoods;
        }

        public async Task<IEnumerable<Restaurant>> GetEntitiesByGeoWithinAsync()
        {
            var indexName = "Search_Point";
            var must = BsonArray.Create(
                new[] 
                {
                    CreateTextFilter(field: "cuisine", value: "French"),
                    CreateTextFilter(field: "name", value: "Chez", maxEdits: 1),
                });
            var should =
                //Builders<BsonDocument>.Filter.GeoWithin
                BsonDocument.Parse(@"{
    geoWithin: {
                circle:
                {
                    center:
                    {
                        type: 'Point',
                coordinates:[-73.9740, 40.7813]
            },
            radius: 1609
                },
        path: 'location'
    }
        }
");
            var result = await GetEntitiesByCompoundQueryAsync<Restaurant>(_restaurantCollectionNamespace1, indexName, must, should).ConfigureAwait(false);
            return result;
        }


        // private fields
        private async Task<IEnumerable<TEntity>> GetEntitiesByCompoundQueryAsync<TEntity>(CollectionNamespace collection, string indexName, BsonArray must, BsonDocument should)
        {
            var searchStage = new BsonDocument
            {
                { "index", indexName },
                {
                    "compound",
                    new BsonDocument
                    {
                        { "must", must },
                        { "should", should }
                    }
                }
            };
            return await GetEntitiesByAggregateStage<TEntity>(collection, searchStage).ConfigureAwait(false);
        }

        private async Task<IEnumerable<TEntity>> GetEntitiesByFieldAsync<TEntity>(CollectionNamespace collection, string field, string value)
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
            return await GetEntitiesByAggregateStage<TEntity>(collection, searchStage).ConfigureAwait(false);
        }

        private async Task<IEnumerable<TEntity>> GetEntitiesByAggregateStage<TEntity>(CollectionNamespace collection, BsonDocument aggregateStage)
        {
            var pipeline = new EmptyPipelineDefinition<TEntity>()
                .AppendStage<TEntity, TEntity, TEntity>(aggregateStage);
            var entities = await _client
                .GetCollection<TEntity>(collection)
                .AggregateAsync(pipeline)
                .ConfigureAwait(false);

            return await entities.ToListAsync().ConfigureAwait(false);
        }

        private BsonDocument CreateTextFilter(string field, string value, int? maxEdits = null)
        {
            return new BsonDocument
            {
                {
                    "text",
                    new BsonDocument
                    {
                        { "query", value },
                        { "path", field },
                        {
                            "fuzzy",
                            new BsonDocument
                            {
                                { "maxEdits", () => maxEdits.Value, maxEdits.HasValue }
                            }
                        }
                    }
                }
            };
        }
    }
}
