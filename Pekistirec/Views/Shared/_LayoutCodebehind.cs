using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Shared
{
    public class _LayoutCodebehind
    {
        public DataLayer.DomainClasses.BaseDomainClasses.AspNetUser user;
        public string userProfilPicture;

        public _LayoutCodebehind(System.Security.Principal.IPrincipal User)
        {
            user = Pekistirec.Engine.Toolbox.UserManagementToolbox.UserManagementTools.GetUser(User);
            userProfilPicture = Pekistirec.Engine.Toolbox.UserManagementToolbox.UserManagementTools.GetUserProfilPictureLink(user);
        }
    }
}