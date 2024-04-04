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
        List<Tags> Get();
        Tags Get(string id);
        Tags Create(Tags tag);
        void Update(string id, Tags tag);
        void Remove(string id);
    }
}
