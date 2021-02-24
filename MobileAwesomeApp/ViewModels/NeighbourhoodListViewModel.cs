using System.Collections.ObjectModel;
using System.Linq;
using MobileAwesomeApp.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace MobileAwesomeApp.ViewModels
{
    public class NeighbourhoodListViewModel
    {
        private readonly IMongoClient _mongoClient;

        public NeighbourhoodListViewModel(IMongoClient mongoClient)
        {
            _mongoClient = mongoClient;
        }

        public ObservableCollection<NeighbourhoodViewModel> Neighbourhoods => GetNeighbourhoods();

        private ObservableCollection<NeighbourhoodViewModel> GetNeighbourhoods()
        {
            var db = _mongoClient.GetDatabase("sample_restaurants");
            var coll = db.GetCollection<Neighbourhood>("neighborhoods");
            var query = from neighbourhood in coll.AsQueryable()
                        orderby neighbourhood.Name
                        select neighbourhood;
            var results = IAsyncCursorSourceExtensions.ToList(query);
            var viewModels = new ObservableCollection<NeighbourhoodViewModel>();
            results.ForEach(result => viewModels.Add(new NeighbourhoodViewModel(result)));
            return viewModels;
        }
    }
}
