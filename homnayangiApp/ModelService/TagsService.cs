
using homnayangiApp.Models;
using MongoDB.Driver;

namespace homnayangiApp.ModelService
{
    public class TagsService : ITagsService
    {
        private readonly IMongoCollection<Tags> _tags;
        public TagsService() 
        {
            string connectionUri = DataService.ConnectString;
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            MongoClient client = new MongoClient(settings);
            var datebase = client.GetDatabase(DataService.DatabaseName);
            _tags = datebase.GetCollection<Tags>(DataService.TagsCollection);
        }
        public Tags Create(Tags tag)
        {
            throw new NotImplementedException();
        }

        public List<Tags> Get()
        {
            return _tags.Find(x => true).ToList();
        }

        public Tags Get(string id)
        {
            return _tags.Find(x => x.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _tags.DeleteOne(x => x.Id == id);
        }

        public void Update(string id, Tags tag)
        {
            _tags.ReplaceOne(x => x.Id == id, tag);
        }
    }
}
