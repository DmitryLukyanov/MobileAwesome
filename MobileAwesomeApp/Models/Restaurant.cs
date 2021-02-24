using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MobileAwesomeApp.Models
{
    // TODO: immutable?
    // TODO: not mongo specific?
    public class Restaurant
    {
        public ObjectId Id { get; set; }
        [BsonElement("restaurant_id")]
        public string Restaurantid { get; set; }
        [BsonElement("borough")]
        public string Borough { get; set; }
        [BsonElement("cuisine")]
        public string Cuisine { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("grades")]
        public Grade[] Grades { get; set; }
        [BsonElement("address")]
        public Address Address { get; set; }
    }

    public class Grade
    {
        [BsonElement("date")]
        public DateTime Date { get; set; }
        [BsonElement("grade")]
        public string GradeSign { get; set; }
        [BsonElement("score")]
        public int? Score { get; set; }
    }

    public class Address
    {
        [BsonElement("building")]
        public string Building { get; set; }
        [BsonElement("street")]
        public string Street { get; set; }
        [BsonElement("zipcode")]
        public string ZipCode { get; set; }
        [BsonElement("coord")]
        public double[] Coordinates { get; set; }
    }
}
