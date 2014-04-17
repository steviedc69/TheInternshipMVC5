using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Internship.Models.Domain
{
    public interface IGemeenteRepository
    {
        IEnumerable<Gemeente> GetAlleGemeentes();
        Gemeente FindGemeenteWithId(int id);
        Gemeente FindGemeenteWithStructuur(String structuur);
    }
}