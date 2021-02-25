
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;

namespace MobileAwesomeApp.Models
{
    public class Neighbourhood
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public GeoJsonGeometry<GeoJson2DCoordinates> Geometry { get; set; }
    }
}
