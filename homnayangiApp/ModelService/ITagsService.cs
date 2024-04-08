using homnayangiApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace homnayangiApp.ModelService
{
    public interface ITagsService
    {
        Task<List<Tags>> Get();
        Task<Tags> Get(string id);
        Task<Tags> Create(Tags tag);
        Task Update(string id, Tags tag);
        Task Remove(string id);
    }
}
