using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Profil
{
    public class IndexCodeBehind
    {
        public string FacebookLoginURL = Pekistirec.Engine.FacebookAPI.FacebookAPIHelpers.LoginURL;
        public string GoogleLoginURL = GoogleAPI.Utils.AuthorizationUrl(true);
        public int AvatarMaxBoyut = 110000;
        public int DokumanSayisi = 0;
        
        public string userAvatarUrl = "";
        public string YuklediklerimURL = "";


        public Dictionary<string, string> tumBranslar = new Dictionary<string, string>();

        public IndexCodeBehind(DataLayer.DomainClasses.BaseDomainClasses.AspNetUser user,System.Web.Mvc.UrlHelper urlHelper)
        {                        
            userAvatarUrl = Pekistirec.Engine.Toolbox.UserManagementToolbox.UserManagementTools.GetUserProfilPictureLink(user);

            using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
            {
                DokumanSayisi = dokumanRepo.All.Where(d => d.EkleyenAspNetUserId == user.Id).Count();
            }

            YuklediklerimURL = "/" + Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari.DokumanControllerVeActionlari.ControllerName + "/" + Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari.DokumanControllerVeActionlari.Yuklediklerim;
        }     
                    
    }
}