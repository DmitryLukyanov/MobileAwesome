using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MobileAwesomeApp.Models
{
    public class Restaurant
    {
        public ObjectId Id { get; set; }
        [BsonElement("restaurant_id")]
        public string Restaurantid { get; set; }
        public string Borough { get; set; }
        public string Cuisine { get; set; }
        public string Name { get; set; }
        public Grade[] Grades { get; set; }
        public Address Address { get; set; }
        public GeoJsonPoint<GeoJson2DCoordinates> Location { get; set; }
    }

    public class Grade
    {
        public DateTime Date { get; set; }
        [BsonElement("grade")]
        public string GradeSign { get; set; }
        public int? Score { get; set; }
    }

    public class Address
    {
        public string Building { get; set; }
        public string Street { get; set; }
        [BsonElement("zipcode")]
        public string ZipCode { get; set; }
        [BsonElement("coord")]
        [BsonDefaultValue(null)]
        public double[] Coordinates { get; set; }
    }
}
