using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Internship.Models.Domain;

namespace Internship.Models.DAL
{
    public interface IStatusRepository
    {
        Status FindStatusWithId(int id);
        void SaveChanges();
        Status FindStatusWithNaam(String naam);
    }
}
