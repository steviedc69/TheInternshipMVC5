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

    }
}