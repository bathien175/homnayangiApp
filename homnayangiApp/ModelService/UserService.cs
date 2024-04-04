using homnayangiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using System.Security.Cryptography;

namespace homnayangiApp.ModelService.StoreSetting
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _user;

        //public UserService(IUserStoreDatabaseSettings settings, IMongoClient mongoClient) 
        //{
        //    var datebase = mongoClient.GetDatabase(settings.DatabaseName);
        //    _user = datebase.GetCollection<User>(settings.UserCoursesCollectionName);
        //}
        public UserService()
        {
            string connectionUri = DataService.ConnectString;
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            MongoClient client = new MongoClient(settings);
            var datebase = client.GetDatabase(DataService.DatabaseName);
            _user = datebase.GetCollection<User>(DataService.UserCollection);
        }
        public User Create(User user)
        {
            _user.InsertOne(user);
            return user;
        }

        public List<User> Get()
        {
            return _user.Find(x => true).ToList();
        }

        public User Get(string id)
        {
            return _user.Find(x => x.Id == id).FirstOrDefault();
        }

        public User Login(string phone, string password)
        {
            String pass = GetMD5Hash(password);
            return _user.Find(x => x.Phone == phone && 
            x.Password == pass).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _user.DeleteOne(x => x.Id == id);
        }

        public void RestorePassword(string phone)
        {
            var u = _user.Find(x => x.Phone == phone).FirstOrDefault();
            if (u != null)
            {
                u.Password = GetMD5Hash("1");
                _user.ReplaceOne(x => x.Id == u.Id, u);
            }
        }

        public void Update(string id, User user)
        {
            _user.ReplaceOne(x => x.Id == id, user);
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

        public User GetbyPhone(string phone)
        {
            return _user.Find(x => x.Phone == phone).FirstOrDefault();
        }

        public User FindIdUser(string id)
        {
            return _user.Find(x => x.IDUser == id).FirstOrDefault();
        }
    }
}
