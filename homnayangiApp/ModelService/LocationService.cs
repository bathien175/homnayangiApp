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
            if (location.Images == null)
            {
                await _firebase.Child("locations").Child(id).PutAsync(location);
            }
            else
            {
                var listnew = await getListImageFromLink(location.Images, id);
                location.Images = listnew;
                await _firebase.Child("locations").Child(id).PutAsync(location);
            }
        }
        public async Task<List<string>> getListImageFromLink(List<string> links, string id)
        {
            List<string> listnew = new List<string>();
            int index = 1;
            foreach (var item in links)
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    // Tải hình ảnh từ URL
                    byte[] imageData = await httpClient.GetByteArrayAsync(item);
                    // Tạo một MemoryStream từ dữ liệu hình ảnh
                    using (MemoryStream memoryStream = new MemoryStream(imageData))
                    {
                        string s = await UploadLocationImage(id, index, memoryStream);
                        listnew.Add(s);
                    }
                }
            }
            return  listnew;
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

        public async Task<string> UploadCacheImage(string userId, int index, Stream imageStream)
        {
            try
            {
                var task = new FirebaseStorage(DataService.ConnectStringFirebaseStorage, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                })
                    .Child("image_cache")
                    .Child(userId)
                    .Child(userId)
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

        public async Task DeleteCacheImage(string userId)
        {
            try
            {
                var task = new FirebaseStorage(DataService.ConnectStringFirebaseStorage, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                })
                    .Child("image_cache")
                    .Child(userId)
                    .DeleteAsync();

                await task;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteImage(string locationID)
        {
            try
            {
                var task = new FirebaseStorage(DataService.ConnectStringFirebaseStorage, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                })
                    .Child("location_images")
                    .Child(locationID)
                    .DeleteAsync();
                await task;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Models.Location>> GetCreateShare(string id)
        {
            return (await _firebase.Child("locations").OnceAsync<Models.Location>())
                 .Select(x => x.Object)
                 .Where(x => x.Creator == id && x.IsShare == true)
                 .ToList();
        }
    }
}
