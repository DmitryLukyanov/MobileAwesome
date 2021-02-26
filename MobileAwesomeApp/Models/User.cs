using MongoDB.Bson;

namespace MobileAwesomeApp.Models
{
    public class User
    {
        public ObjectId Id { get; private set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Coordinates CurrentLocation { get; set; }
    }
}
