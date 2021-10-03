using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.IO;
using DataLayer.Repositories.BaseRepositories;
using DataLayer.DomainClasses.BaseDomainClasses;

namespace Pekistirec.Engine.Toolbox.UserManagementToolbox
{
    public class UserManagementTools
    {
        public static string GetUserId(IPrincipal User)
        {
            if (User.Identity.IsAuthenticated)
            {
                return User.Identity.GetUserId();
            }

            return null;
        }

        public static AspNetUser GetUser(IPrincipal User, bool VeritabanindanGuncelle = false, DataLayer.UnitOfWorks.IBaseUnitOfWork uow = null)
        {
            if (User.Identity.IsAuthenticated)
            {
                var userid = User.Identity.GetUserId();

                var user = HttpContext.Current.Session[Pekistirec.Engine.PekistirecConsts.SessionNamesConsts.SESSION_USER] as AspNetUser;

                if (user != null)
                {
                    if (User.Identity.GetUserId() != user.Id)
                    {
                        user = null;                        
                    }
                }

                if (VeritabanindanGuncelle || user == null)
                {

                    if (uow != null)
                    {
                        using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository(uow))
                        {
                            user = repo.All.Where(u => u.Id == userid).FirstOrDefault();
                        }
                    }
                    else
                    {
                        using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
                        {
                            user = repo.All.Where(u => u.Id == userid).FirstOrDefault();
                        }
                    }

                    HttpContext.Current.Session[Pekistirec.Engine.PekistirecConsts.SessionNamesConsts.SESSION_USER] = user;
                }
                return user;
            }
            else
            {
                HttpContext.Current.Session[Pekistirec.Engine.PekistirecConsts.SessionNamesConsts.SESSION_USER] = null;
                return null;
            }
        }

        public static string GetUserProfilPictureLink(AspNetUser user)
        {
            if (user != null)
            {
                if (!String.IsNullOrWhiteSpace(user.Avatar) && !String.IsNullOrEmpty(user.Avatar))
                {                                        
                    var directory = Pekistirec.Engine.Toolbox.DosyaVeDizinToolbox.DosyaVeDizinTools.UserAvatarFolder.GercekPath;
                    var filePath = directory + "\\" + user.Avatar;
                    if (!File.Exists(filePath))
                    {
                        new AzureAPI.Implementation.ProfilAvatarBlob().Download(user.Avatar, filePath);
                    }

                    return Pekistirec.Engine.PekistirecConsts.UserManagementConsts.USER_AVATAR_ROOT_URL + user.Avatar;
                }
            }
            return PekistirecConsts.UserManagementConsts.DEFAULT_AVATAR_URL;
        }
    }
}