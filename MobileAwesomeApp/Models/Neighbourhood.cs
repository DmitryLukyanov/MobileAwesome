
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MobileAwesomeApp.Models
{
    public class Neighbourhood
    {
        public ObjectId Id { get; set; }
        [BsonElement("name")]
        public string Name { get; set; }
        [BsonElement("geometry")]
        public GeoJsonGeometry<GeoJson2DCoordinates> Geometry { get; set; }
    }
}
