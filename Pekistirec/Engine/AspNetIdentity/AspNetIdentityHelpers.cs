using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Pekistirec.Engine.AspNetIdentity
{
    public class AspNetIdentityHelpers
    {
        private System.Web.Mvc.Controller controller;
        private ApplicationUserManager _userManager;

        public ApplicationUserManager UserManager
        {
            get
            {                
                return _userManager ?? controller.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            protected set
            {
                _userManager = value;
            }
        }

        private RoleManager<IdentityRole> _roleManager;
        public RoleManager<IdentityRole> RoleManager
        {
            get
            {
                if (_roleManager == null) _roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
                return _roleManager;

            }
            set
            {
                _roleManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return controller.HttpContext.GetOwinContext().Authentication;
            }
        }        

        public AspNetIdentityHelpers( System.Web.Mvc.Controller Controller)
        {
            controller = Controller;
        }

        public  void RolleriGuncelle()
        {
            List<string> roller = Enum.GetNames(typeof(Pekistirec.Engine.AspNetIdentity.Roller)).ToList();

            foreach (var item in roller)
            {
                var roleresult = RoleManager.Create(new IdentityRole(item));
            }
        }


        public Task<IdentityResult> CreateUserAsync(ApplicationUser applicationUser, string password)
        {                        
            return UserManager.CreateAsync(applicationUser, password);            
        }

        public IdentityResult CreateUser(ApplicationUser applicationUser, string password)
        {
            return UserManager.Create(applicationUser, password);
        }

        public Task<IdentityResult> CreateUserAsync(ApplicationUser applicationUser)
        {
            return UserManager.CreateAsync(applicationUser);
        }

        public async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {            
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));                
        }

        public Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {            
            return UserManager.ChangePasswordAsync(userId, currentPassword, newPassword);
        }

        public Task<IdentityResult> AddPasswordAsync(string userId, string password)
        {
            return UserManager.AddPasswordAsync(userId, password);
        }

        public void SignOut()
        {
            AuthenticationManager.SignOut();
        }

        public void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                controller.ModelState.AddModelError("", error);
            }
        }

        public Task<ApplicationUser> FindAsync(string userName, string password)
        {            
            return UserManager.FindAsync(userName, password);
        }

        public Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return UserManager.FindByIdAsync(userId);
        }

        private static Object UserNameDegistirLock = new Object();
        public static string UserNameDegistir(string userId, string userName)
        {
            string r = DateTime.Now.ToLongTimeString() + "     ";
            lock (UserNameDegistirLock)
            {
                System.Threading.Thread.Sleep(10000);
            }
            r += DateTime.Now.ToLongTimeString();
            return r;
        }

        public List<string> GetRoles(string userId)
        {
            return UserManager.GetRoles(userId).ToList();
        }
        
        public bool AddToRole(string userId,string role)
        {
            return UserManager.AddToRole(userId, role).Succeeded;
        }

        public bool IsInRole(string userId, string role)
        {
            return UserManager.IsInRole(userId, role);
        }

        public IdentityResult RemoveFromRole(string userId, string role)
        {
            return UserManager.RemoveFromRole(userId, role);
        }

        private static Object ChangeUserNameLock = new Object();
        public void ChangeUserName(string userId, string userName)
        {
            if (userId != null && userName != null)
            {
                userName = userName.Trim();
                userId = userId.Trim();

                if (!String.IsNullOrEmpty(userName) && !String.IsNullOrWhiteSpace(userName) && !String.IsNullOrEmpty(userId) && !String.IsNullOrWhiteSpace(userId))
                {
                    if (userName.Length <= IdentitConfig.UserNameMaxLength
                     && userName.Length >= IdentitConfig.UserNameMinLength
                     && System.Text.RegularExpressions.Regex.IsMatch(userName, IdentitConfig.UserNameRegex))
                    {
                        lock (ChangeUserNameLock)
                        {                            
                            using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
                            {
                                var user = repo.All.Where(u => u.Id == userId).FirstOrDefault();

                                if (user != null)
                                { 
                                    if (repo.All.Where(u=>u.UserName == userName).Count() < 1)
                                    {
                                        user.UserName = userName;
                                        repo.Save();
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public bool HasPassword(string userId)
        {
            return UserManager.HasPassword(userId);
        }

    }
}