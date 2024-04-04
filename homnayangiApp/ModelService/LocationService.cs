using MongoDB.Driver;
using System.Linq;

namespace homnayangiApp.ModelService
{
    public class LocationService : ILocationService
    {
        private readonly IMongoCollection<Models.Location> _location;

        public LocationService()
        {
            string connectionUri = DataService.ConnectString;
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            MongoClient client = new MongoClient(settings);
            var datebase = client.GetDatabase(DataService.DatabaseName);
            _location = datebase.GetCollection<Models.Location>(DataService.LocationCollection);
        }
        public Models.Location Create(Models.Location location)
        {
            _location.InsertOne(location);
            return location;
        }

        public List<Models.Location> Get()
        {
            return _location.Find(x => true).ToList();
        }
        public List<Models.Location> GetIsShare()
        {
            return _location.Find(x => x.IsShare == true).ToList();
        }
        public Models.Location Get(string id)
        {
            return _location.Find(x => x.Id == id).FirstOrDefault();
        }

        public List<Models.Location> GetByTag(List<string> listtag)
        {

            var filter = Builders<Models.Location>.Filter.AnyIn("tags", listtag);
            var result = _location.Find(filter).ToList();
            return result;
        }

        public List<Models.Location> GetCreate(string id)
        {
            var s = _location.Find(x => x.Creator == id).ToList();
            return s;
        }

        public List<Models.Location> GetNear(string province, string district)
        {
            var listcity = _location.Find(x => x.IsShare == true && x.Creator == null && x.Province == province).ToList();
            var listdistrict = listcity.OrderBy(x => x.District == district).ToList();
            return listdistrict;
        }

        public void Remove(string id)
        {
            _location.DeleteOne(x => x.Id == id);
        }

        public List<Models.Location> Search(string name)
        {
            return _location.Find(x => x.Name.Contains(name)).ToList();
        }

        public void Update(string id, Models.Location location)
        {
            _location.ReplaceOne(x => x.Id == id, location);
        }
    }
}
