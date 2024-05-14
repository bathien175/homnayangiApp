using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ModelService
{
    public interface ILocationService
    {
        Task<List<Models.Location>> Get();
        Task<List<Models.Location>> GetIsShare();
        Task<List<Models.Location>> GetNear(string province, string district);
        Task<List<Models.Location>> GetCreate(string id);
        Task<List<Models.Location>> GetCreateShare(string id);
        Task<List<Models.Location>> GetByTag(List<String> listtag);
        Task<List<Models.Location>> GetSaveLocation(List<String> listSave);
        Task<List<Models.Location>> Search(string name);
        Task<Models.Location> Get(string id);
        Task<Models.Location> Create(Models.Location location);
        Task<Models.Location> CreateClone(Models.Location location);
        Task Update(string id, Models.Location location);
        Task Remove(string id);
        Task<string> UploadLocationImage(string locateId, int index, Stream imageStream);
        Task<string> UploadCacheImage(string userId, int index, Stream imageStream);
        Task DeleteCacheImage(string userId);
        Task DeleteImage(string locationID);
    }
}
