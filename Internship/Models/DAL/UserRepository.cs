using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Internship.Models.Domain;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Internship.Models.DAL
{
    public class UserRepository : IUserRepository
    {
        private IDbSet<ApplicationUser> Users;
        private InternshipContext Context;
        
        
        public bool UserExist(String userName)
        {
            return Context.Users.Any(u => u.UserName.Equals(userName));
        }

        public UserManager<ApplicationUser> Manager { get; set; }
        public RoleManager<IdentityRole> RoleManager { get; set; }
       
        public UserRepository(InternshipContext context)
        {
            this.Context = context;
            this.Users = context.Users;
            this.Manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            this.RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            UserValidator<ApplicationUser> userValidator = Manager.UserValidator as UserValidator<ApplicationUser>;
            userValidator.AllowOnlyAlphanumericUserNames = false;
        }


        public IQueryable<ApplicationUser> GetAllUsers()
        {
            return Users.OrderBy(u => u.UserName);
        }

        public ApplicationUser FindUser(string email)
        {
            return Users.SingleOrDefault(u => u.UserName.Equals(email));
        }

        public Task<ApplicationUser> FindAsyncUser(string username, string paswoord)
        {
            return Manager.FindAsync(username, paswoord);
        }

        public Task<IdentityResult> CreateAsyncUser(ApplicationUser username, string paswoord)
        {
            return Manager.CreateAsync(username, paswoord);
        }

        public Task<IdentityResult> ChangePaswordAsync(string userId, String oldP, String newP)
        {
            return Manager.ChangePasswordAsync(userId, oldP, newP);
        }

        public Task<IdentityResult> AddAsyncPassword(string user, string newP)
        {
            return Manager.AddPasswordAsync(user, newP);
        }
        public bool CreateRole(string role)
        {
            var roleCreated = RoleManager.Create(new IdentityRole(role));
            return roleCreated.Succeeded;
        }

        public bool AddUserToRole(string id, string role)
        {
            var userAdded = Manager.AddToRole(id, role);
            return userAdded.Succeeded;
        }
    }
}