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
        List<User> Get();
        User Login(string phone, string password);
        User Get(string id);
        User GetbyPhone(string phone);
        User Create(User user);
        void Update(string id, User user);
        void Remove(string id);
        void RestorePassword(string phone);
        User FindIdUser(string id);
    }
}
