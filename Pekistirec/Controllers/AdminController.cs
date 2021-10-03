using Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec.Controllers
{
    [Authorize(Roles="Admin")]
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DbBufferReset()
        {
            DataLayer.Buffers.Yenile();

            return RedirectToAction("Index");
        }

        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ AYARLAR ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region AYARLAR

        [ValidateAntiForgeryToken]
        public ActionResult AyarDegistir(Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari ayar, string deger)
        {
            bool basarili = false;

            switch (ayar)
            {
                case Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari.EVENTLOG_ENABLED:
                    {
                        bool temp = false;
                        if (bool.TryParse(deger, out temp))
                        {
                            Pekistirec.Engine.PekistirecAyarlar.Ayarlar.EVENTLOG_ENABLED.Deger = temp;
                            basarili = true;
                        }
                    }
                    break;
                case Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari.EVENTLOG_BUFFER_SIZE:
                    {
                        int temp = 1;
                        if (int.TryParse(deger, out temp))
                        {
                            Pekistirec.Engine.PekistirecAyarlar.Ayarlar.EVENTLOG_BUFFER_SIZE.Deger = temp;
                            basarili = true;
                        }
                    }
                    break;
                case Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari.RECAPTCHA_ENABLED:
                    {
                        bool temp = false;
                        if (bool.TryParse(deger, out temp))
                        {
                            Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_ENABLED.Deger = temp;
                            basarili = true;
                        }
                    }
                    break;
                case Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari.RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI:
                    {
                        int temp = 1;
                        if (int.TryParse(deger, out temp))
                        {
                            Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI.Deger = temp;
                            basarili = true;
                        }
                    }
                    break;
                case Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari.RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI:
                    {
                        int temp = 1;
                        if (int.TryParse(deger, out temp))
                        {
                            Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI.Deger = temp;
                            basarili = true;
                        }
                    }
                    break;
                case Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari.RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI:
                    {
                        int temp = 1;
                        if (int.TryParse(deger, out temp))
                        {
                            Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI.Deger = temp;
                            basarili = true;
                        }
                    }
                    break;
                case Pekistirec.Engine.PekistirecAyarlar.PekistirecAyarAdlari.RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI:
                    {
                        int temp = 1;
                        if (int.TryParse(deger, out temp))
                        {
                            Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI.Deger = temp;
                            basarili = true;
                        }
                    }
                    break;
            }

            if (basarili)
            {
                this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.ADMIN_AYARLAR_DEGISTI);                
            }
            else
            {
                this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.ADMIN_AYARLAR_DEGISTI_HATA);    
            }
            this.SetEventLogMessageValue(ayar.ToString() + "=" + deger);

            return Json(new { succeded = basarili });
        }


        public ActionResult Ayarlar()
        {
            Pekistirec.Models.AdminModels.AyarlarViewModel model = new Models.AdminModels.AyarlarViewModel();
            model.EVENTLOG_ENABLED = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.EVENTLOG_ENABLED.Deger;
            model.EVENTLOG_BUFFER_SIZE = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.EVENTLOG_BUFFER_SIZE.Deger;
            model.RECAPTCHA_ENABLED = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_ENABLED.Deger;
            model.RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI.Deger;
            model.RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI.Deger;
            model.RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI.Deger;
            model.RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI = Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI.Deger;

            return View(model);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ AYARLAR ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 



        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ KULLANICI İŞLEMLERİ ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region KULLANICI İŞLEMLERİ

        [HttpGet]
        public ActionResult KullaniciAra()
        {
            return View();
        }

        [HttpPost]
        public ActionResult KullaniciAra(string text)
        {
            List<DataLayer.DomainClasses.BaseDomainClasses.AspNetUser> kullaniciListesi = null;

            using (var kullaniciRepo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
            {
                kullaniciListesi = kullaniciRepo.All.Where(u => u.UserName.Contains(text)).ToList();
            }

            return View(kullaniciListesi);
        }

        public ActionResult KullaniciAyrinti(string id)
        {
            var model = new Pekistirec.Models.AdminModels.KullaniciAyrintiViewModel();

            DataLayer.DomainClasses.BaseDomainClasses.AspNetUser user = null;

            using (var userRepo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
            {
                user = userRepo.All.Where(u => u.Id == id).FirstOrDefault();
            }

            if (user == null)
            {
                return RedirectToAction(AdminControllerVeActionlari.KullaniciAra);
            }

            model.User = user;

            var aspNetIdentityHelper = new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(this);
            model.Roller = aspNetIdentityHelper.GetRoles(id);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KullaniciBilgiDuzenleRol(List<string> rol, string userId)
        {
            var aspNetIdentityHelper = new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(this);

            List<string> gelenRoller = (rol == null) ? null : rol.Distinct().ToList();
            List<string> mevcutRoller = aspNetIdentityHelper.GetRoles(userId);
            List<string> butunRoller = Enum.GetNames(typeof(Pekistirec.Engine.AspNetIdentity.Roller)).ToList();

            if (gelenRoller == null) // Kullanıcı hiçbir role dahil olmayacak
            {
                foreach (var mevcutRol in mevcutRoller)
                {
                    aspNetIdentityHelper.RemoveFromRole(userId, mevcutRol);
                }
            }
            else
            {
                //Gelen Roller Listesindeki Geçersiz Roller Siliniyor
                foreach (var item in gelenRoller)
                {
                    if (butunRoller.Where(br => br == item).Count() < 0)
                    {
                        gelenRoller.Remove(item);
                    }
                }

                foreach (var item in butunRoller)
                {
                    bool kullaniciZatenBuRoleSahip = mevcutRoller.Where(mr => mr == item).Count() > 0;

                    if (gelenRoller.Where(gr => gr == item).Count() > 0)
                    {
                        if (!kullaniciZatenBuRoleSahip)
                        {
                            aspNetIdentityHelper.AddToRole(userId, item);
                        }
                    }
                    else
                    {
                        if (kullaniciZatenBuRoleSahip)
                        {
                            aspNetIdentityHelper.RemoveFromRole(userId, item);
                        }
                    }
                }
            }


            return RedirectToAction(AdminControllerVeActionlari.KullaniciAyrinti, AdminControllerVeActionlari.ControllerName, new { id = userId });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KullaniciBilgiDuzenleUserName(string userName,string userId)
        {
            new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(this).ChangeUserName(userId, userName);

            return RedirectToAction(AdminControllerVeActionlari.KullaniciAyrinti, AdminControllerVeActionlari.ControllerName, new { id = userId });
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ KULLANICI İŞLEMLERİ ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 
    }
}