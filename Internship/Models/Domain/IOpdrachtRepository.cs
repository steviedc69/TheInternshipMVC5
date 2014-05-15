using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Internship.Models.Domain;

namespace Internship.Models.Domain
{
    public interface IOpdrachtRepository
    {
        Opdracht FindOpdracht(int id);
        IEnumerable<Opdracht> GeefStageOpdrachten();
        IEnumerable<Opdracht> GeefStageOpdrachtenMetZoekstring(String search);
        IEnumerable<Opdracht> GeefActieveOpdrachten(); 
        void SaveChanges();
        void RemoveOpdracht(Opdracht opdracht);
    }
}