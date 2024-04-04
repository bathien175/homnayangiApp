using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ModelService
{
    public interface ILocationService
    {
        List<Models.Location> Get();
        List<Models.Location> GetIsShare();
        List<Models.Location> GetNear(string province, string district);
        List<Models.Location> GetCreate(string id);
        List<Models.Location> GetByTag(List<String> listtag);
        List<Models.Location> Search(string name);
        Models.Location Get(string id);
        Models.Location Create(Models.Location location);
        void Update(string id, Models.Location location);
        void Remove(string id);
    }
}
