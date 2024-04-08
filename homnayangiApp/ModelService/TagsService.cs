
using Firebase.Database;
using Firebase.Database.Query;
using homnayangiApp.Models;
using MongoDB.Driver;

namespace homnayangiApp.ModelService
{
    public class TagsService : ITagsService
    {
        private readonly FirebaseClient _firebase;
        public TagsService() 
        {
            _firebase = new FirebaseClient(DataService.ConnectStringFirebase);
        }
        public async Task<Tags> Create(Tags tag)
        {
            var result = await _firebase.Child("tags").PostAsync(tag);
            if (!String.IsNullOrEmpty(result.Key))
            {
                tag.Id = result.Key;
                await Update(result.Key, tag);
            }
            return tag;
        }

        public async Task<List<Tags>> Get()
        {
            return (await _firebase.Child("tags").OnceAsync<Tags>()).Select(x => x.Object).ToList();
        }

        public async Task<Tags> Get(string id)
        {
            var result = await _firebase.Child("tags").OnceAsync<Tags>();
            var tag = result.Where(x => x.Object.Id == id).FirstOrDefault();
            if (tag == null)
                return null;
            return tag.Object;
        }

        public async Task Remove(string id)
        {
            await _firebase.Child("tags").Child(id).DeleteAsync();
        }

        public async Task Update(string id, Tags tag)
        {
            await _firebase.Child("tags").Child(id).PutAsync(tag);
        }
    }
}
