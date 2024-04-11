using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;

namespace homnayangiApp.ModelService
{
    public class LocationService : ILocationService
    {
        private readonly FirebaseClient _firebase;

        public LocationService()
        {
            _firebase = new FirebaseClient(DataService.ConnectStringFirebase);
        }
        public async Task<Models.Location> Create(Models.Location location)
        {
            var result = await _firebase.Child("locations").PostAsync(location);
            if (!String.IsNullOrEmpty(result.Key))
            {
                location.Id = result.Key;
                int index = 1;
                if (location.Images != null)
                {
                    List<string> strings = [];
                    foreach (var item in location.Images)
                    {
                        byte[] imageBytes = Convert.FromBase64String(item);
                        MemoryStream stream = new MemoryStream(imageBytes);
                        stream.Position = 0;
                        string s = await UploadLocationImage(location.Id, index, stream);
                        strings.Add(s);
                        index++;
                    }
                    location.Images = strings;
                }
                await Update(result.Key, location);
            }
            return location;
        }

        public async Task<List<Models.Location>> Get()
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>()).Select(x => x.Object).ToList();
        }
        public async Task<List<Models.Location>> GetIsShare()
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>()).Select(x => x.Object).Where(x => x.IsShare == true && x.Creator == null).ToList();
        }
        public async Task<Models.Location> Get(string id)
        {
            var result = await _firebase.Child("locations").OnceAsync<Models.Location>();
            var lot = result.Where(x => x.Object.Id == id).FirstOrDefault();
            if (lot == null)
                return null;
            return lot.Object;
        }

        public async Task<List<Models.Location>> GetByTag(List<string> listtag)
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>())
                .Select(x => x.Object)
                .Where(x => x.Creator == null && x.IsShare == true && x.Tags.Any(tag => listtag.Contains(tag)))
                .ToList();
        }

        public async Task<List<Models.Location>> GetCreate(string id)
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>())
                .Select(x => x.Object)
                .Where(x => x.Creator == id)
                .ToList();
        }

        public async Task<List<Models.Location>> GetNear(string province, string district)
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>())
                .Select(x => x.Object)
                .Where(x => x.Creator == null && x.Province == province)
                .ToList();
        }

        public async Task Remove(string id)
        {
            await _firebase.Child("locations").Child(id).DeleteAsync();
        }

        public async Task<List<Models.Location>> Search(string name)
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>())
                .Select(x => x.Object)
                .Where(x => x.Creator == null && x.Name.Contains(name) && x.IsShare == true)
                .ToList();
        }

        public async Task Update(string id, Models.Location location)
        {
            await _firebase.Child("locations").Child(id).PutAsync(location);
        }
        public async Task<string> UploadLocationImage(string locateId,int index, Stream imageStream)
        {
            try
            {
                var task = new FirebaseStorage(DataService.ConnectStringFirebaseStorage, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                })
                    .Child("location_images")
                    .Child(locateId)
                    .Child($"Image{index}.jpg")
                    .PutAsync(imageStream);

                var downloadlink = await task;
                return downloadlink;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Models.Location>> GetSaveLocation(List<string> listSave)
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>())
                .Select(x => x.Object)
                .Where(x =>  x.IsShare == true && listSave.Contains(x.Id))
                .ToList();
        }
    }
}
