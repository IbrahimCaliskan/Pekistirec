using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec.Controllers
{
    [Authorize]
    public class ProfilController : Controller
    {
        // GET: Profil
        public ActionResult Index()
        {                        
            return View(this.GetUser(true));
        }


        public enum DegistirilebilecekAlanlar {  googleid, facebookid }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult Guncelle(DegistirilebilecekAlanlar alan, string data)
        {           
            switch (alan)
            {                
                case DegistirilebilecekAlanlar.googleid:
                    {
                        bool baskaGirisYoluVar = false;
                        using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
                        {
                            var userId = this.GetUserId();
                            var aspNetUser = repo.All.Where(u => u.Id == userId).FirstOrDefault();

                            baskaGirisYoluVar = new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(this).HasPassword(userId)
                                                ||
                                                aspNetUser.FacebookUserId != null;

                            if (baskaGirisYoluVar)
                            {
                                this.SetEventLogMessageValue(aspNetUser.GoogleUserId);
                                this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_GOOGLE_USER_ID);
                                aspNetUser.GoogleUserId = null;
                                aspNetUser.GoogleRefreshToken = null;
                                aspNetUser.GoogleEmail = null;
                                repo.Save();
                            }
                        }

                        if (baskaGirisYoluVar)
                        {
                            return Json(new Pekistirec.Engine.PekistirecTypes.JsonReturnResult("OK", ""));
                        }
                        else
                        {
                            return Json(new Pekistirec.Engine.PekistirecTypes.JsonReturnResult("BaskaGirisYoluYok", ""));
                        }
                    }               
                case DegistirilebilecekAlanlar.facebookid:
                    {
                        bool baskaGirisYoluVar = false;
                        using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
                        {
                            var userId = this.GetUserId();                            
                            var aspNetUser = repo.All.Where(u => u.Id == userId).FirstOrDefault();

                            baskaGirisYoluVar = new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(this).HasPassword(userId)
                                                ||
                                                aspNetUser.GoogleUserId != null;

                            if (baskaGirisYoluVar)
                            {
                                this.SetEventLogMessageValue(aspNetUser.FacebookUserId);
                                this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_FACEBOOK_USER_ID);
                                aspNetUser.FacebookUserId = null;
                                repo.Save();                                
                            }
                        }

                        if (baskaGirisYoluVar)
                        {
                            return Json(new Pekistirec.Engine.PekistirecTypes.JsonReturnResult("OK", ""));
                        }
                        else
                        {
                            return Json(new Pekistirec.Engine.PekistirecTypes.JsonReturnResult("BaskaGirisYoluYok", ""));                               
                        }
                    }
                default: return Json(new Pekistirec.Engine.PekistirecTypes.JsonReturnResult("FAIL", ""));
            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]       
        public JsonResult GuncelleHakkimda(string data)
        {
            data = HttpUtility.HtmlDecode(data);
            data = data.Trim();
            if (data.Length > DataLayer.DomainClasses.BaseDomainClasses.PROPERTY_ATTRIBUTE_VALUES.ASPNETUSER_Hakkimda_MAXLENGTH)
            {
                data = data.Substring(0, DataLayer.DomainClasses.BaseDomainClasses.PROPERTY_ATTRIBUTE_VALUES.ASPNETUSER_Hakkimda_MAXLENGTH);
            }

            var colorStartIndex = 0;
            do
            {
                int color = data.IndexOf("<font color=\"", colorStartIndex);
                if (color < 0)
                    break;
                data = data.Remove(color + 20, 2);
                colorStartIndex = color + 20;

            } while (true);

            Dictionary<string, string> HtmlTagToEncodedTag = new Dictionary<string, string>()
            {
                {"<font color=\"","[/font color="},
                {"</font>","[/font]"},
                {"<u>","[u]"},
                {"</u>","[/u]"},
                {"<strike>","[strike]"},
                {"</strike>","[/strike]"},
                {"<i>","[i]"},
                {"</i>","[/i]"},
                {"<b>","[b]"},
                {"</b>","[/b]"},
                {"<br>","[br]"}
            };

            foreach (var item in HtmlTagToEncodedTag)
            {
                data = data.Replace(item.Key, item.Value);
            }

            data = HttpUtility.HtmlEncode(data);

            foreach (var item in HtmlTagToEncodedTag)
            {
                data = data.Replace(item.Value, item.Key);
            }

            colorStartIndex = 0;
            do
            {
                int color = data.IndexOf("<font color=\"", colorStartIndex);
                if (color < 0)
                    break;
                data = data.Insert(color + 20, "\">");
                colorStartIndex = color + 20;

            } while (true);

            using (var uow = new DataLayer.UnitOfWorks.BaseUnitOfWork())
            {
                using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository(uow))
                {
                    var userId = this.GetUserId();
                    repo.All.Where(u => u.Id == userId).FirstOrDefault().Hakkimda = data;
                    repo.Save();
                    this.GetUser(true, uow); //SESSION GUNCELLEMESI ICIN
                }
            }
            
            this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_HAKKIMDA);
            this.SetEventLogMessageValue(data);
            
            return Json(new Pekistirec.Engine.PekistirecTypes.JsonReturnResult("OK", ""));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult GuncelleAvatar(string filename)
        {           
            var returnValue = new Pekistirec.Engine.PekistirecTypes.JsonReturnResult("FAIL", "Resim dosyası yüklenirken bir hata oluştu.");
            var file = Request.Files["avatar"];

            if (file == null)
            {
                returnValue.message = "Bir dosya seçmelisiniz.";
                return Json(returnValue);
            }

            //if (!file.ContentType.StartsWith("image/"))
            //{
            //    returnValue.message = "Resim dosyası seçmelisiniz.";
            //    return Json(returnValue);
            //}

            if (file.ContentLength > 110000)
            {
                returnValue.message = "Dosya boyutu 100kb'tan büyük olmamalı.";
                return Json(returnValue);
            }

            var fileInfo = new System.IO.FileInfo(file.FileName);
            if (!file.IsImage())
            {
                returnValue.message = "Resim dosyası seçmelisiniz.";
                return Json(returnValue);
            }
            try
            {                
                var userId = this.GetUserId();
                DateTime Now = DateTime.Now;
                string tempFolderPath = Pekistirec.Engine.Toolbox.DosyaVeDizinToolbox.DosyaVeDizinTools.TempFolder.GercekPath;
                if (!new System.IO.DirectoryInfo(tempFolderPath).Exists)
                {
                    System.IO.Directory.CreateDirectory(tempFolderPath);
                }

                string fileNameWithoutExtension = String.Format("{0}-{1}{2}{3}{4}{5}{6}{7}{8}", userId, Now.Day, Now.Month, Now.Year, Now.Hour, Now.Minute, Now.Second, Now.Millisecond, new Random().Next(0, 1000));

                System.Security.Cryptography.MD5CryptoServiceProvider pwd = new System.Security.Cryptography.MD5CryptoServiceProvider();
                string hashedFileNameWithoutExtension = Convert.ToBase64String(pwd.ComputeHash(System.Text.Encoding.UTF8.GetBytes(fileNameWithoutExtension)));
                hashedFileNameWithoutExtension = new Pekistirec.Engine.Toolbox.StringToolbox.StringTools().RemoveSpecialCharacters(hashedFileNameWithoutExtension);

                string avatarFileName = String.Format("{0}{1}", hashedFileNameWithoutExtension, fileInfo.Extension);
                string avatarFilePath = tempFolderPath + "\\" + avatarFileName;
                file.SaveAs(avatarFilePath);



                var azureBlob = new AzureAPI.Implementation.ProfilAvatarBlob();
                azureBlob.Upload(avatarFileName, avatarFilePath);

                using (var uow = new DataLayer.UnitOfWorks.BaseUnitOfWork())
                {
                    using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository(uow))
                    {                        
                        repo.All.Where(u => u.Id == userId).FirstOrDefault().Avatar = avatarFileName;
                        repo.Save();
                        this.GetUser(true, uow);//SESSION GUNCELLEMESI ICIN

                        returnValue.status = "OK";
                        returnValue.message = Pekistirec.Engine.Toolbox.UserManagementToolbox.UserManagementTools.GetUserProfilPictureLink(this.GetUser());
                    }
                }                              

                this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_AVATAR);
                this.SetEventLogMessageValue(avatarFileName);                
            }
            catch (Exception) 
            { 
                returnValue.status = "FAIL"; 
                returnValue.message = "Beklenmedik bir hata ile karşılaşıldı."; 
            }

            return Json(returnValue);
        }
    }
}