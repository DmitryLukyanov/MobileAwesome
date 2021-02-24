using MobileAwesomeApp.Models;
using MongoDB.Bson;

namespace MobileAwesomeApp.ViewModels
{
    public class NeighbourhoodViewModel
    {
        public ObjectId Id { get; private set; }
        public string Name { get; private set; }

        public NeighbourhoodViewModel(Neighbourhood neighbourhood)
        {
            Id = neighbourhood.Id;
            Name = neighbourhood.Name;
        }
    }
}
