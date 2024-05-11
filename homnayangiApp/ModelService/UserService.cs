using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Storage;
using homnayangiApp.Models;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace homnayangiApp.ModelService.StoreSetting
{
    public class UserService : IUserService
    {
        private readonly FirebaseClient _firebase;

        public UserService()
        {
            _firebase = new FirebaseClient(DataService.ConnectStringFirebase);
        }
        public async Task<User> Create(User user)
        {
            var result = await _firebase.Child("users").PostAsync(user);
            if (!String.IsNullOrEmpty(result.Key))
            {
                user.Id = result.Key;
                if(user.ImageData!= null)
                {
                    byte[] imageBytes = Convert.FromBase64String(user.ImageData);
                    MemoryStream stream = new MemoryStream(imageBytes);
                    stream.Position = 0;
                    user.ImageData = await UploadUserImage(user.Id,stream);
                }
                await Update(result.Key, user);
            }
            return user;
        }

        public async Task<List<User>> Get()
        {
            return (await _firebase.Child("users").OnceAsync<User>()).Select(x => x.Object).Where(x => x.IDUser!= "@admin@").ToList();
        }

        public async Task<User> Get(string id)
        {
            var result = await _firebase.Child("users").OnceAsync<User>();
            var user = result.Where(x => x.Object.IDUser == id).FirstOrDefault();
            if (user == null)
                return null;
            return user.Object;
        }

        public async Task<User> Login(string phone, string password)
        {
            String hashedPassword = GetMD5Hash(password);
            var users = await _firebase.Child("users").OnceAsync<User>();
            var result = users.Where(x => x.Object.Phone == phone && x.Object.Password == hashedPassword).FirstOrDefault();
            if (result == null)
                return null;
            return result.Object;
        }

        public async Task Remove(string id)
        {
            await _firebase.Child("users").Child(id).DeleteAsync();
        }

        public async Task RestorePassword(string phone)
        {
            var users = (await _firebase.Child("users").OnceAsync<User>());
            var user = users.Where(x => x.Object.Phone == phone).FirstOrDefault();
            if (user != null)
            {
                var u = user.Object;
                u.Password = GetMD5Hash("1");
                await _firebase.Child("users").Child(user.Key).PutAsync(JsonConvert.SerializeObject(u));
            }
        }

        public async Task Update(string id, User user)
        {
            await _firebase.Child("users").Child(id).PutAsync(user);
        }
        public string GetMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] inputBytes = Encoding.UTF8.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public async Task<User> GetbyPhone(string phone)
        {
            var result = (await _firebase.Child("users")
                .OrderByKey()
                .OnceAsync<User>()).Select(x => x.Object);

            var u = result.Where(x => x.Phone == phone).FirstOrDefault();
            return u;
        }

        public async Task<string> UploadUserImage(string userId, Stream imageStream)
        {
            try
            {
                var task = new FirebaseStorage(DataService.ConnectStringFirebaseStorage, new FirebaseStorageOptions
                {
                    ThrowOnCancel = true,
                })
                    .Child("user_images")
                    .Child(userId)
                    .Child("avatarImage.jpg")
                    .PutAsync(imageStream);

                var downloadlink = await task;
                return downloadlink;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<User>> SearchUser(string idU, string idCurrent)
        {
            return (await _firebase.Child("users")
                .OnceAsync<User>())
                .Select(x => x.Object)
                .Where(x => x.IDUser != "@admin@" && x.IDUser != idCurrent && x.IDUser.ToLower().Contains(idU.ToLower()))
                .ToList();
        }
    }
}
