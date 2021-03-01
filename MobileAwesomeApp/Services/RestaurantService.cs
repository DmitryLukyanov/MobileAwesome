using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using MobileAwesomeApp.Infrastructure.Mongo;
using MobileAwesomeApp.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MobileAwesomeApp.Services
{
    public interface IRestaurantService
    {
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync(int page, int pageSize);
        Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string name);
        Task<IEnumerable<Restaurant>> GetRestaurantByBoroughAsync(string borough);
        Task<IEnumerable<Restaurant>> GetRestaurantByCuisineAsync(string cuisine);

        Task<IEnumerable<Neighbourhood>> GetNeighbourhoodsAsync(string name);
        Task<IEnumerable<Neighbourhood>> GetNeighbourhoodsAsync();
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

        public async Task<IEnumerable<Restaurant>> GetRestaurantsByNeighbourhood(string neighbourhoodName)
        {
            var neighbourhoods = await GetNeighbourhoodsAsync(neighbourhoodName).ConfigureAwait(false);

            var neighbourhoodGeometry = neighbourhoods.SingleOrDefault().Geometry;
            var filter = Builders<Restaurant>.Filter.GeoWithin(r => r.Location, neighbourhoodGeometry);
            var restaurantsCollection = _client.GetCollection<Restaurant>(_restaurantCollectionNamespace);
            var restaurantsCursor = await restaurantsCollection .FindAsync(filter).ConfigureAwait(false);
            return await restaurantsCursor.ToListAsync().ConfigureAwait(false);
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsAsync(int page, int pageSize)
        {
            var aggegateBuilder = new EmptyPipelineDefinition<Restaurant>()
                .Skip(page * pageSize)
                .Limit(pageSize);
            var cursor = await _client
                .GetCollection<Restaurant>(_restaurantCollectionNamespace1)
                .AggregateAsync(aggegateBuilder)
                .ConfigureAwait(false);
            var result = await cursor.ToListAsync().ConfigureAwait(false);
            return result.Take(10);
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsGeoWithinAsync(
            FieldDefinition<Restaurant> locationFieldDefinition,
            double x,
            double y,
            double radius,
            params (Expression<Func<Restaurant, string>> Field, string Value, int? MaxEdits)[] fieldMustFilters)
        {
            return await GetEntitiesByGeoWithinAsync<Restaurant>(
                _restaurantCollectionNamespace1,
                "Search_Point",
                BsonDocument.Parse($@"
{{
    geoWithin: 
    {{
        circle: 
        {{
            center:
            {{
                type: 'Point',
                coordinates:[{x}, {y}]
            }},
            radius: {radius}
        }},
        path: '{GetFieldName<Restaurant>(locationFieldDefinition)}'
    }}
}}"),
                 fieldMustFilters.Select(c =>
                 {
                     var fieldDefinition = (FieldDefinition<Restaurant, string>)new ExpressionFieldDefinition<Restaurant, string>(c.Field);
                     return (fieldDefinition, c.Value, c.MaxEdits);
                 }).ToArray()
            ).ConfigureAwait(false);

        }

        public Task<IEnumerable<Restaurant>> GetRestaurantsAsync(string name)
        {
            return GetEntitiesByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "name", value: name);
        }

        public Task<IEnumerable<Restaurant>> GetRestaurantByBoroughAsync(string borough)
        {
            return GetEntitiesByFieldAsync<Restaurant>(_restaurantCollectionNamespace, field: "borough", value: borough);
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

        public async Task<IEnumerable<TEntity>> GetEntitiesByGeoWithinAsync<TEntity>(
            CollectionNamespace collectionNamespace,
            string indexName,
            BsonDocument shouldAggregateStage,
            params (FieldDefinition<TEntity, string> Field, string Value, int? MaxEdits)[] mustFilters)
        {
            var mustBsonArray = new BsonArray();
            foreach (var mustFilter in mustFilters)
            {
                var field = GetFieldName<TEntity>(mustFilter.Field);
                mustBsonArray.Add(CreateTextFilter(field, value: mustFilter.Value, mustFilter.MaxEdits));
            }

            var should = shouldAggregateStage;
            var result = await GetEntitiesByCompoundQueryAsync<TEntity>(collectionNamespace, indexName, mustBsonArray, should).ConfigureAwait(false);
            return result;
        }


        // private fields
        private async Task<IEnumerable<TEntity>> GetEntitiesByCompoundQueryAsync<TEntity>(CollectionNamespace collection, string indexName, BsonArray must, BsonDocument should)
        {
            var searchStage = new BsonDocument
            {
                {
                    "$search",
                    new BsonDocument
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

        private BsonDocument CreateTextFilter(BsonValue field, string value, int? maxEdits = null)
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
                            },
                            maxEdits.HasValue
                        }
                    }
                }
            };
        }

        private BsonValue GetFieldName<TEntity>(FieldDefinition<TEntity> field)
        {
            var fieldSerializer = BsonSerializer.LookupSerializer<TEntity>();
            return field.Render(fieldSerializer, BsonSerializer.SerializerRegistry).FieldName;
        }
    }
}
