using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Internship.ViewModels;
using WebGrease.Css.Extensions;

namespace Internship.Models.Domain
{
   public interface ISearchStragegy
    {
        IList<Opdracht> Search(IList<Opdracht>opdrachten, String search = null);
    }

    public class SearchByAll : ISearchStragegy
    {
        public IList<Opdracht> Search(IList<Opdracht> opdrachten, string search = null)
        {
            IList<Opdracht>result = new List<Opdracht>();
            if (search==null)
            {
                return opdrachten;
            }
            foreach (Opdracht opdracht in opdrachten)
            {
                if (opdracht.ToString().Contains(search))
                {
                    result.Add(opdracht);
                }
            }
            return result.OrderBy(o=>o.Title).ToList();
        }
    }

    public class SearchByContact : ISearchStragegy
    {
        public IList<Opdracht> Search(IList<Opdracht> opdrachten, string search = null)
        {
            IList<Opdracht> result = new List<Opdracht>();
            if (search == null)
            {
                return opdrachten;
            }
            foreach (Opdracht opdracht in opdrachten)
            {
                if ((opdracht.Ondertekenaar != null && opdracht.StageMentor != null) && (opdracht.Ondertekenaar.ToString().ToLower().Contains(search.ToLower())||opdracht.StageMentor.ToString().ToLower().Contains(search.ToLower())
                    ||opdracht.ToString().ToLower().Contains(search.ToLower())))
                {
                    result.Add(opdracht);
                }
            }

            return result.OrderBy(o=>o.Ondertekenaar.ToString()).ToList();
        }
    }

    public class SearchBySchooljaar : ISearchStragegy
    {
        public IList<Opdracht> Search(IList<Opdracht> opdrachten, string search = null)
        {
            IList<Opdracht> result = new List<Opdracht>();
            
            if (search == null)
            {
                return opdrachten.OrderBy(o=>o.Schooljaar).ToList();
            }
            foreach (Opdracht opdracht in opdrachten)
            {
                search = search.ToLower();
                if (opdracht.Schooljaar.ToLower().Contains(search))
                {
                    result.Add(opdracht);
                }
            }

            return result.OrderBy(o=>o.Schooljaar).ToList();
           }
        }

    public class SearchByStatus : ISearchStragegy
    {
        private int statusId;
        public SearchByStatus(int statusId)
        {
            this.statusId = statusId;
        }

        public IList<Opdracht> Search(IList<Opdracht> opdrachten, string search = null)
        {
  
            if (search == null)
            {
                return opdrachten.Where(o=>o.Status.Id == statusId).OrderBy(o => o.Schooljaar).ToList();
            }
            else
            {
                search = search.ToLower();
                return
                    opdrachten.Where(o => o.Status.Id == statusId && o.ToString().ToLower().Contains(search))
                        .OrderBy(o => o.Title)
                        .ToList();
            }

        }

        public class SearchBySpecialisatie : ISearchStragegy
        {
            public IList<Opdracht> Search(IList<Opdracht> opdrachten, string search = null)
            {
                if (search == null)
                {
                    return opdrachten.OrderBy(o => o.Specialisatie.Title).ToList();
                }
                else
                {
                    search = search.ToLower();
                    return
                        opdrachten.Where(o => o.Specialisatie.Title.ToLower().Equals(search))
                            .OrderBy(o => o.Specialisatie.Title)
                            .ToList();
                }
            }
        }
        public class SearchWithSearchModel : ISearchStragegy
        {
            private SearchModel Model { get; set; }

            public SearchWithSearchModel(SearchModel model)
            {
                this.Model = model;
            }

            public IList<Opdracht> Search(IList<Opdracht> opdrachten, string search = null)
            {
                List<Opdracht> temp = opdrachten.ToList();
                
                if (Model.Bedrijven!=null && temp.Count != 0)
                {
                    temp.RemoveAll(o => !(o.Bedrijf.ToString().Equals(Model.Bedrijven)));
                }
                if (Model.Schooljaar != null && temp.Count != 0)
                {
                    temp.RemoveAll(o => !(o.Schooljaar.Equals(Model.Schooljaar)));
                }
                if (Model.Gemeente != null && temp.Count != 0)
                {
                    temp.RemoveAll(o => !(o.Adres.Gemeente.Structuur.Contains(Model.Gemeente)));
                }
                if (Model.Trefwoord != null && temp.Count != 0)
                {
                    temp.RemoveAll(o => !(o.ToString().Contains(Model.Trefwoord)));
                }
                return temp;
            }
        }
    }

    }
    

