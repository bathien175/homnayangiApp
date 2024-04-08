using homnayangiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ModelService
{
    public interface IUserService
    {
        Task<List<User>> Get();
        Task<User> Login(string phone, string password);
        Task<User> Get(string id);
        Task<User> GetbyPhone(string phone);
        Task<User> Create(User user);
        Task Update(string id, User user);
        Task Remove(string id);
        Task RestorePassword(string phone);
        Task<string> UploadUserImage(string userId, Stream imageStream);
    }
}
