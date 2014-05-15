using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Internship.Models.Domain
{
    public abstract class Sort
    {
        public IList<Opdracht> Opdrachten { get; set; } 
        public Sort(IList<Opdracht> opdrachten)
        {
            this.Opdrachten = opdrachten;
        }

        public abstract IList<Opdracht> Sorteer();

    }

    public class SortBySchoojaar : Sort
    {
        public SortBySchoojaar(IList<Opdracht> opdrachten) : base(opdrachten)
        {
        }

        public override IList<Opdracht> Sorteer()
        {
            return Opdrachten.OrderBy(o => o.Schooljaar).ToList();
        }
    }

    public class SortByTitle : Sort
    {
        public SortByTitle(IList<Opdracht> opdrachten) : base(opdrachten)
        {
        }

        public override IList<Opdracht> Sorteer()
        {
            return Opdrachten.OrderBy(o => o.Title).ToList();
        }
    }

    public class SortByBedrijf : Sort
    {
        public SortByBedrijf(IList<Opdracht> opdrachten) : base(opdrachten)
        {
        }

        public override IList<Opdracht> Sorteer()
        {
            return Opdrachten.OrderBy(o => o.Bedrijf.Bedrijfsnaam).ToList();
        }
    }

    public class SortBySpecialisatie : Sort
    {
        public SortBySpecialisatie(IList<Opdracht> opdrachten) : base(opdrachten)
        {
        }

        public override IList<Opdracht> Sorteer()
        {
            return Opdrachten.OrderBy(o => o.Specialisatie.Title).ToList();
        }
    }

    public class SortByDitSchooljaar : Sort
    {
        public SortByDitSchooljaar(IList<Opdracht> opdrachten) : base(opdrachten)
        {
        }

        public override IList<Opdracht> Sorteer()
        {
            int year = DateTime.Now.Year;
            int month = DateTime.Now.Month;

            if (month < 7)
            {
                return Opdrachten.Where(o => o.SchoolJaarSecondInt() == year && o.IsSemester2 == true).OrderBy(o=>o.Title).ToList();
            }
            return
                Opdrachten.Where(o => o.SchoolJaarFirstInt() == year)
                    .OrderBy(o => o.Title)
                    .OrderBy(o => o.Title)
                    .ToList();

        }
    }
}
