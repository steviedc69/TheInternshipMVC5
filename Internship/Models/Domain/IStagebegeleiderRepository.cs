using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Models.Domain
{
    public interface IStagebegeleiderRepository
    {
        IQueryable<Stagebegeleider> FindAll();
        Stagebegeleider FindByEmail(string email);
        Stagebegeleider FindById(String id);
        IQueryable<Stagebegeleider> FindByName(string naam);
        void UpdateFirstTime(String username);
        void SaveChanges();
    }
}
