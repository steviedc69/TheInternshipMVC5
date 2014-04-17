using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Internship.Models.Domain
{
    public interface IUserRepository
    {
        //lijn
        bool UserExist(String userName);
        UserManager<ApplicationUser> Manager { get; set; }
        IQueryable<ApplicationUser> GetAllUsers();
        ApplicationUser FindUser(String email);
        Task<IdentityResult> CreateAsyncUser(ApplicationUser username, string paswoord);
        Task<ApplicationUser> FindAsyncUser(string username, string paswoord);
        Task<IdentityResult> ChangePaswordAsync(string user, String oldP, String newP);
        Task<IdentityResult> AddAsyncPassword(string user, string newP);
    }
}