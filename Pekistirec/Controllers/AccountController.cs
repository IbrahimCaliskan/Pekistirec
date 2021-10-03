using Microsoft.AspNet.Identity;
using Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Pekistirec.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();            
        }

        private Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers aspNetIdentityHelpers;

        public AccountController()
        {
            aspNetIdentityHelpers = new Pekistirec.Engine.AspNetIdentity.AspNetIdentityHelpers(this);
        }

        public string KullaniciAdiTavsiyesi(string kullaniciAdi)
        {
            string donecekKullaniciAdi = kullaniciAdi.ToLower();
            donecekKullaniciAdi = (donecekKullaniciAdi.IndexOf("@") > 0) ? donecekKullaniciAdi.Substring(0, kullaniciAdi.IndexOf("@")) : donecekKullaniciAdi; //@ işaretinden sonrası siliniyor...
            var stringTools = new Pekistirec.Engine.Toolbox.StringToolbox.StringTools();
            donecekKullaniciAdi = stringTools.RemoveSpecialCharacters(donecekKullaniciAdi);
            kullaniciAdi = donecekKullaniciAdi;

            using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
            {
                //Uygun bir kullanıcı adı aranıyor....
                int i = 0;

                List<string> userNameListe = new List<string>();

                userNameListe = repo.All.Where(u => u.UserName == kullaniciAdi).Select(u => u.UserName).ToList();
                if (userNameListe.Count < 1)
                {
                    //donecekKullaniciAdi = donecekKullaniciAdi;
                }
                else
                {
                    string un1 = ""; string un2 = ""; string un3 = ""; string un4 = ""; string un5 = "";
                    do
                    {
                        un1 = String.Format("{0}{1}", kullaniciAdi, i + 1);
                        un2 = String.Format("{0}{1}", kullaniciAdi, i + 2);
                        un3 = String.Format("{0}{1}", kullaniciAdi, i + 3);
                        un4 = String.Format("{0}{1}", kullaniciAdi, i + 4);
                        un5 = String.Format("{0}{1}", kullaniciAdi, i + 5);
                        i = i + 5;

                        userNameListe = repo.All.Where(u => u.UserName == un1
                                                    || u.UserName == un2
                                                    || u.UserName == un3
                                                    || u.UserName == un4
                                                    || u.UserName == un5
                                                    ).Select(u => u.UserName).ToList();

                        if (userNameListe.Count < 1)
                        {
                            donecekKullaniciAdi = un1;
                            break;
                        }

                        if (userNameListe.Count > 4)
                        {
                            continue;
                        }

                        string temp = "";
                        bool bulunduMu = false;
                        for (int j = 1; j < 6; j++)
                        {
                            temp = String.Format("{0}{1}", kullaniciAdi, i - 5 + j);
                            if (userNameListe.Where(u => u == temp).Count() < 1)
                            {
                                donecekKullaniciAdi = temp;
                                bulunduMu = true;
                                break;
                            }
                        }
                        if (bulunduMu) break;

                    } while (true);

                }
            }

            return donecekKullaniciAdi;
        }


        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ LOGIN ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region LOGIN

        private static bool LoginReCaptchaGerekliMi(string userIp)
        {
            bool reCaptchaGerekliMi = false;
            var eventLog = Pekistirec.Engine.EventLog.EventLogHelpers.Son24SaatIcindekiLoglariGetirIPveLogTipineGore(userIp, Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_HATA);
            reCaptchaGerekliMi = (eventLog.Count() > Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI.Deger);
            return reCaptchaGerekliMi;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var responseModel = new Pekistirec.Models.AccountModels.LoginResponseViewModel();
            responseModel.reCaptchaGerekliMi = LoginReCaptchaGerekliMi(this.GetUserIP());

            ViewBag.ReturnUrl = returnUrl;       
     
            return View(responseModel);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Models.AccountModels.LoginRequestViewModel model, string returnUrl)
        {
            var responseModel = new Pekistirec.Models.AccountModels.LoginResponseViewModel();
            responseModel.UserName = model.UserName;
            responseModel.RememberMe = model.RememberMe;

            bool reCaptchaGerekliMi = LoginReCaptchaGerekliMi(this.GetUserIP());

            responseModel.reCaptchaGerekliMi = reCaptchaGerekliMi;
            
            string EventLogErrorMessage = "";                                   

            if (reCaptchaGerekliMi)
            {
                if (Pekistirec.Engine.reCaptcha.reCaptchaHelpers.Verify(
                    Request.ServerVariables["REMOTE_ADDR"].ToString(),
                    model.recaptcha_challenge_field,
                    model.recaptcha_response_field)
                    .Success == false)
                {
                    ModelState.AddModelError("recaptcha_response_field", "Doğrulama kodu hatalı.");
                    EventLogErrorMessage += "Doğrulama kodu hatalı.";
                }
            }

            if (ModelState.IsValid)
            {
                var user = await aspNetIdentityHelpers.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await aspNetIdentityHelpers.SignInAsync(user, model.RememberMe);

                    this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_BASARILI);
                    this.SetEventLogMessageValue(user.UserName);

                    if (returnUrl == null)
                        return RedirectToAction(HomeControllerVeActionlari.Index, HomeControllerVeActionlari.ControllerName);


                    return RedirectToLocal(returnUrl);

                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı Adı veya Şifre hatalı.");
                }
            }
            
            var allErrors = String.Join("-", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToArray());
            this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_HATA);
            this.SetEventLogMessageValue(allErrors);            

            // If we got this far, something failed, redisplay form
            return View(responseModel);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ LOGIN ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 

        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ MANAGE ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region MANAGE

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }
        

        //
        // GET: /Account/Manage
        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Şifreniz değiştirildi."
                : message == ManageMessageId.SetPasswordSuccess ? "Şifreniz gönderildi."
                : message == ManageMessageId.RemoveLoginSuccess ? "Harici giriş biliniz silindi."
                : message == ManageMessageId.Error ? "Bekklenmedik bir hata ile karşılaşıldı."
                : "";
            var userId = this.GetUserId();
            ViewBag.HasLocalPassword =  aspNetIdentityHelpers.HasPassword(userId);
            ViewBag.ReturnUrl = Url.Action(AccountControllerVeActionlari.Manage);
            return View();
        }

        private IdentityResult ManageHatalariTurkcelestir(IdentityResult result)
        {
            Dictionary<string, string> EngTur = new Dictionary<string, string>() { 
                        {"Incorrect password", "Hatalı şifre"}
                    };

            List<string> cErrors = new List<string>();
            result.Errors.ToList().ForEach((e) =>
            {
                foreach (var ceviri in EngTur)
                    cErrors.Add((e.IndexOf(ceviri.Key) > 0) ? e : ceviri.Value);
            });

            IdentityResult tResult = new IdentityResult(cErrors);
            return tResult;
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(Pekistirec.Models.AccountModels.ManageUserViewModel model)
        {
            string userId = this.GetUserId();
            bool hasPassword = aspNetIdentityHelpers.HasPassword(userId);
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action(AccountControllerVeActionlari.Manage);
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = await aspNetIdentityHelpers.ChangePasswordAsync(userId, model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        var user = await aspNetIdentityHelpers.FindByIdAsync(userId);
                        await aspNetIdentityHelpers.SignInAsync(user, isPersistent: false);
                        this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_SIFRE);
                        return RedirectToAction(AccountControllerVeActionlari.Manage, new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_SIFRE_HATA);
                        this.SetEventLogMessageValue(model.ConfirmPassword);
                        aspNetIdentityHelpers.AddErrors(ManageHatalariTurkcelestir(result));
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    IdentityResult result = await aspNetIdentityHelpers.AddPasswordAsync(userId, model.NewPassword);
                    if (result.Succeeded)
                    {
                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_SIFRE);
                        this.SetEventLogMessageValue("Yeni Tanımlandı.");
                        return RedirectToAction(AccountControllerVeActionlari.Manage, new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_SIFRE_HATA);
                        this.SetEventLogMessageValue(model.ConfirmPassword);

                        aspNetIdentityHelpers.AddErrors(ManageHatalariTurkcelestir(result));
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ MANAGE ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            aspNetIdentityHelpers.SignOut();
            return RedirectToAction(HomeControllerVeActionlari.Index, HomeControllerVeActionlari.ControllerName);
        }

        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ REGISTER ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region REGISTER        
        private static bool RegisterReCaptchaGerekliMi(string userIp)
        {            
            bool reCaptchaGerekliMi = false;
            var eventLog = Pekistirec.Engine.EventLog.EventLogHelpers.Son24SaatIcindekiLoglariGetirIPveLogTipineGore(userIp, Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_BASARILI);
            reCaptchaGerekliMi = (eventLog.Count() > Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI.Deger);
            return reCaptchaGerekliMi;
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var responseModel = new Pekistirec.Models.AccountModels.RegisterResponseViewModel();
            responseModel.reCaptchaGerekliMi = RegisterReCaptchaGerekliMi(this.GetUserIP());
            return View(responseModel);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(Pekistirec.Models.AccountModels.RegisterRequestViewModel model)
        {
            var responseModel = new Pekistirec.Models.AccountModels.RegisterResponseViewModel();
            responseModel.UserName = model.UserName;
            responseModel.SozlesmeKabul = model.SozlesmeKabul;            


            bool reCaptchaGerekliMi = RegisterReCaptchaGerekliMi(this.GetUserIP());
            responseModel.reCaptchaGerekliMi = reCaptchaGerekliMi;

            string EventLogErrorMessage = "";
            if (model.SozlesmeKabul == false)
            { ModelState.AddModelError("SozlesmeKabul", "Kullanıcı Sözleşmesini kabul etmeden kullanıcı oluşturamazsınız.");
            EventLogErrorMessage += "Kullanıcı Sözleşmesini kabul etmeden kullanıcı oluşturamazsınız.";
            }            

            if (reCaptchaGerekliMi)
            {
                if (Pekistirec.Engine.reCaptcha.reCaptchaHelpers.Verify(
                    Request.ServerVariables["REMOTE_ADDR"].ToString(),
                    model.recaptcha_challenge_field,
                    model.recaptcha_response_field)
                    .Success == false)
                {
                    ModelState.AddModelError("recaptcha_response_field", "Doğrulama kodu hatalı.");
                    EventLogErrorMessage += "Doğrulama kodu hatalı.";
                }
            }
            

            if (ModelState.IsValid)
            {
                var user = new Pekistirec.Engine.AspNetIdentity.ApplicationUser() { UserName = model.UserName };

                IdentityResult result = await aspNetIdentityHelpers.CreateUserAsync(user, model.Password);

                if (result.Succeeded)
                {
                    this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_BASARILI);
                    this.SetEventLogMessageValue(model.UserName);

                    await aspNetIdentityHelpers.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
                }
                else
                {
                    // Hata Türkçeleştirme
                    Dictionary<string, string> EngTur = new Dictionary<string, string>() { 
                        {"is already taken", "Bu kullanıcı adı daha önceden alındı."}
                    };                    

                    List<string> cErrors = new List<string>();
                    result.Errors.ToList().ForEach((e) =>
                    {
                        foreach (var ceviri in EngTur)
                            cErrors.Add((e.IndexOf(ceviri.Key) > 0) ? ceviri.Value : e);
                    });

                    IdentityResult tResult = new IdentityResult(cErrors);                    

                    aspNetIdentityHelpers.AddErrors(tResult);

                    this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_HATA);
                    this.SetEventLogMessageValue(String.Join("", result.Errors.ToArray()));
                }
            }
            else
            {                
                this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_HATA);
                this.SetEventLogMessageValue(EventLogErrorMessage);
            }            
            return View(responseModel);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ REGISTER ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 



        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ FBCALLBACK ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region FBCALLBACK
        
        [HttpGet]
        [AllowAnonymous]        
        public async Task<ActionResult> FbCallback(string code, string error = null, string error_description = null)
        {            
            // Facebook Bilgileri Alınıyor...
            var facebookLoginResult = Pekistirec.Engine.FacebookAPI.FacebookAPIHelpers.Login(code);
            if (!facebookLoginResult.Success || String.IsNullOrEmpty(facebookLoginResult.User.id))
            {
                ViewBag.Message = "Facebook Login İşlemi Başarısız...";

                this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_EXTERNAL_FACEBOOK_HATA);
                this.SetEventLogMessageValue(code);

                return View(Pekistirec.Views.VIEW_NAMES.SharedViewNames.Error);
            }            

            // FacebookUserId ile daha önceden kullanıcı oluşturulmuşsa giriş yapılıyor...
            using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
            {
                var aspNetUser = repo.All.Where(u => u.FacebookUserId == facebookLoginResult.User.id).FirstOrDefault();
                if (aspNetUser != null)
                {
                    var user = await aspNetIdentityHelpers.FindByIdAsync(aspNetUser.Id);
                    if (user != null)
                    {

                        await aspNetIdentityHelpers.SignInAsync(user, false);

                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_EXTERNAL_FACEBOOK_BASARILI);
                        this.SetEventLogMessageValue(facebookLoginResult.User.id);

                        return RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
                    }
                    else
                    {
                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_EXTERNAL_FACEBOOK_HATA);
                        this.SetEventLogMessageValue(facebookLoginResult.User.id);

                        return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
                    }
                }

            }

            // Kullanıcı giriş yapmış ise FacebookUserId bilgisi ekleniyor...
            if (User.Identity.IsAuthenticated)
            {
                RedirectToRouteResult redirectResult;
                var userId = this.GetUserId();

                using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
                {                    
                    var aspnetUser = repo.All.Where(u => u.Id == userId).FirstOrDefault();

                    if (aspnetUser != null)
                    {
                        aspnetUser.FacebookUserId = facebookLoginResult.User.id;
                        repo.Save();

                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_FACEBOOK_USER_ID);
                        this.SetEventLogMessageValue(facebookLoginResult.User.id);

                        redirectResult = RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
                    }
                    else
                    {
                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_EXTERNAL_FACEBOOK_HATA);
                        this.SetEventLogMessageValue(code);

                        aspNetIdentityHelpers.SignOut();

                        redirectResult = RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
                    }
                }

                return redirectResult;
            }

            //FacebookUserId ile yeni kullanıcı oluşturulması için bilgiler gönderiliyor...                        
            var model = new Pekistirec.Models.AccountModels.FbCallbackResponseViewModel();
            model.UserName = KullaniciAdiTavsiyesi((!String.IsNullOrEmpty(facebookLoginResult.User.username)) ? facebookLoginResult.User.username : facebookLoginResult.User.email);
            model.FbUser = facebookLoginResult.User;
            model.UserIp = this.GetUserIP();
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> FbCallback(Models.AccountModels.FbCallbackRequestViewModel model, string __RequestVerificationToken)
        {
            var responseModel = new Models.AccountModels.FbCallbackResponseViewModel();
            responseModel.UserName = model.UserName;            

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
            }            

            var tamparedParameter = Pekistirec.Engine.ParameterTampering.ParameterTamperingHelpers.Getir(__RequestVerificationToken, this.GetUserIP());
            if (tamparedParameter == null)
            {
                return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
            }

            var fbUser = tamparedParameter.Parameter as Pekistirec.Engine.FacebookAPI.FacebookUser;
            if (fbUser == null)
            {
                return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
            }

            responseModel.FbUser = fbUser;
            responseModel.UserIp = tamparedParameter.Ip;            

            if (ModelState.IsValid)
            {                
                if (fbUser != null)
                {
                    var user = new Pekistirec.Engine.AspNetIdentity.ApplicationUser() { UserName = responseModel.UserName };

                    IdentityResult result = await aspNetIdentityHelpers.CreateUserAsync(user);

                    if (result.Succeeded)
                    {
                        await aspNetIdentityHelpers.SignInAsync(user, isPersistent: false);

                        RedirectToRouteResult redirectResult = null;

                        using (var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository())
                        {
                            var aspnetUser = repo.All.Where(u => u.Id == user.Id).FirstOrDefault();

                            if (aspnetUser != null)
                            {
                                aspnetUser.FacebookUserId = fbUser.id;                                
                                repo.Save();

                                this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_EXTERNAL_FACEBOOK_BASARILI);
                                this.SetEventLogMessageValue(responseModel.UserName + " FbId=" + fbUser.id);

                                redirectResult = RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
                            }
                            else
                            {
                                this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_EXTERNAL_FACEBOOK_HATA);
                                this.SetEventLogMessageValue(fbUser.id);

                                aspNetIdentityHelpers.SignOut();

                                redirectResult = RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
                            }
                        }                                                                     

                        return redirectResult;
                    }
                    else
                    {
                        // Hata Türkçeleştirme
                        Dictionary<string, string> EngTur = new Dictionary<string, string>() { 
                        {"is already taken", "Bu kullanıcı adı daha önceden alındı."}
                    };

                        List<string> cErrors = new List<string>();
                        result.Errors.ToList().ForEach((e) =>
                        {
                            foreach (var ceviri in EngTur)
                                cErrors.Add((e.IndexOf(ceviri.Key) > 0) ? ceviri.Value : e);
                        });

                        IdentityResult tResult = new IdentityResult(cErrors);

                        aspNetIdentityHelpers.AddErrors(tResult);

                        this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_EXTERNAL_FACEBOOK_HATA);
                        this.SetEventLogMessageValue(String.Join("", result.Errors.ToArray()));
                    }
                }
                else
                {
                    this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_EXTERNAL_FACEBOOK_HATA);
                    this.SetEventLogMessageValue("FbUser is Null");
                    return RedirectToAction(HomeControllerVeActionlari.Index, HomeControllerVeActionlari.ControllerName);
                }
            }
            return View(responseModel);
        }
        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ FBCALLBACK ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 



        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ GOOGLECALLBACK ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region GOOGLECALLBACK

        [AllowAnonymous]
        public async Task<ActionResult> GoogleCallback(string code, string state = null)
        {
            var responseModel = new Pekistirec.Models.AccountModels.GoogleCallbackResponseViewModel();

            ///////////// GOOGLE GİRİŞ İŞLEMİ YAPILIYOR... //////////////////////////
            GoogleAPI.GoogleAPICarrier googleUser = null;
            try
            {
                googleUser = new GoogleAPI.Utils().Login(code, state);
            }
            catch (Exception)
            {
                return RedirectToAction(HomeControllerVeActionlari.Index, HomeControllerVeActionlari.ControllerName);
            }


            if (String.IsNullOrEmpty(googleUser.Id))
            {
                this.SetEventLogMessageValue(code);
                this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_EXTERNAL_GOOGLE_HATA);

                ViewBag.Message = "Google Login İşlemi Başarısız...";
                return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
            }
            ///////////////////////////////////////////////////////////////////////////


            var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository();

            var aspNetUser = repo.All.Where(u => u.GoogleUserId == googleUser.Id).FirstOrDefault();

            if (aspNetUser != null) // Daha önceden GoogleId ile kayıt yapılmışsa....   
            {
                if (aspNetUser.GoogleRefreshToken == null)
                {
                    if (googleUser.RefreshToken == null)
                    {
                        //RefreshToken için Google yönlendirmesi yapılıyor...
                        repo.Dispose();
                        return RedirectPermanent(GoogleAPI.Utils.AuthorizationUrl(true));
                    }
                }
                else
                {
                    //RefreshToken kaydedilip Profil sayfasına yönlendiriliyor.
                    aspNetUser.GoogleRefreshToken = googleUser.RefreshToken;
                    repo.Save();

                    this.SetEventLogMessageValue(googleUser.RefreshToken);
                    this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_GOOGLE_REFRESH_TOKEN);

                    var user = await aspNetIdentityHelpers.FindByIdAsync(aspNetUser.Id);
                    if (user != null)
                    {

                        await aspNetIdentityHelpers.SignInAsync(user, false);
                        repo.Dispose();
                        return RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
                    }
                    else
                    {
                        repo.Dispose();
                        return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
                    }
                }                
            }
            else // Daha önceden GoogleId ile kayıt yapılmamışsa
            {
                if (User.Identity.IsAuthenticated) // Local Hesap İle Giriş Yapılmışmı...
                {
                    // Kullanıcı GoogleRefreshTokenAdmin ise Token Genel Kullanım İçin Kaydediliyor...
                    if (aspNetIdentityHelpers.IsInRole(this.GetUserId(), Pekistirec.Engine.AspNetIdentity.Roller.Admin.ToString()))
                    {
                        using (var googleRefreshTokenRepo = new DataLayer.Repositories.BaseRepositories.GoogleRefreshTokenRepository())
                        {
                            var refreshToken = googleRefreshTokenRepo.All.Where(grt => grt.GoogleId == googleUser.Id).FirstOrDefault();
                            if (refreshToken != null)
                            {
                                refreshToken.Mail = googleUser.Email;
                                refreshToken.RefreshToken = googleUser.RefreshToken;
                            }
                            else
                            {
                                googleRefreshTokenRepo.InsertOrUpdate(new DataLayer.DomainClasses.BaseDomainClasses.GoogleRefreshToken() { GoogleId = googleUser.Id, Mail = googleUser.Email, RefreshToken = googleUser.RefreshToken });
                            }
                            googleRefreshTokenRepo.Save();
                        }

                        return RedirectToAction(AdminControllerVeActionlari.Index, AdminControllerVeActionlari.ControllerName);
                    }

                    if (googleUser.RefreshToken == null)
                    {
                        //RefreshToken için Google yönlendirmesi yapılıyor...
                        repo.Dispose();
                        return RedirectPermanent(GoogleAPI.Utils.AuthorizationUrl(true));
                    }
                    else
                    {
                        //Local Kullanıcıya Google Hesap Bilgileri Ekleniyor....
                        var userId = this.GetUserId();
                        var tempUser = repo.All.Where(u => u.Id == userId).FirstOrDefault();
                        tempUser.GoogleRefreshToken = googleUser.RefreshToken;
                        tempUser.GoogleUserId = googleUser.Id;
                        tempUser.GoogleEmail = googleUser.Email;
                        repo.Save();

                        this.SetEventLogMessageValue(googleUser.Id);
                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_BILGI_DEGISTI_GOOGLE_USER_ID);

                        repo.Dispose();
                        return RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
                    }
                }
            }

            if (googleUser.RefreshToken == null)
            {
                //RefreshToken için Google yönlendirmesi yapılıyor...
                repo.Dispose();
                return RedirectPermanent(GoogleAPI.Utils.AuthorizationUrl(true));
            }

           //GoogleId ile oluşturulacak yeni kullanıcı için kullanıcı adı belirlenmesi için View gönderiliyor...

            responseModel.GoogleUser = googleUser;
            responseModel.UserIp = this.GetUserIP();
            responseModel.UserName = KullaniciAdiTavsiyesi(googleUser.Email);

            repo.Dispose();
            return View(responseModel);
        }


        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GoogleCallback(Models.AccountModels.GoogleCallbackRequestViewModel model, string __RequestVerificationToken)
        {
            var responseModel = new Pekistirec.Models.AccountModels.GoogleCallbackResponseViewModel();
            responseModel.UserName = model.UserName;

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
            }            

            var tamparedParameter = Pekistirec.Engine.ParameterTampering.ParameterTamperingHelpers.Getir(__RequestVerificationToken, this.GetUserIP());
            if (tamparedParameter == null)
            {
                this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_EXTERNAL_GOOGLE_HATA);
                this.SetEventLogMessageValue("tamparedParameter is Null");
                return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
            }

            var googleUser = tamparedParameter.Parameter as GoogleAPI.GoogleAPICarrier;
            if (googleUser == null)
            {
                this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_EXTERNAL_GOOGLE_HATA);
                this.SetEventLogMessageValue("googleUser is Null");
                return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
            }

            responseModel.GoogleUser = googleUser;
            responseModel.UserIp = tamparedParameter.Ip;

            var repo = new DataLayer.Repositories.BaseRepositories.AspNetUserRepository();

            var tempAspnetUser = repo.All.Where(u => u.GoogleUserId == googleUser.Id).FirstOrDefault();
            if (tempAspnetUser != null)
            {
                repo.Dispose();
                return RedirectPermanent(GoogleAPI.Utils.AuthorizationUrl(true));
            }

            if (ModelState.IsValid)
            {
                var user = new Pekistirec.Engine.AspNetIdentity.ApplicationUser() { UserName = model.UserName };

                IdentityResult result = await aspNetIdentityHelpers.CreateUserAsync(user);

                if (result.Succeeded)
                {
                    await aspNetIdentityHelpers.SignInAsync(user, isPersistent: false);                    

                    var aspnetUser = repo.All.Where(u => u.Id == user.Id).FirstOrDefault();

                    if (aspnetUser != null)
                    {
                        aspnetUser.GoogleUserId = googleUser.Id;
                        aspnetUser.GoogleRefreshToken = googleUser.RefreshToken;
                        aspnetUser.GoogleEmail = googleUser.Email;
                        repo.Save();

                        this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_EXTERNAL_GOOGLE_BASARILI);
                        this.SetEventLogMessageValue(model.UserName + " GoogleId=" + googleUser.Id);

                        repo.Dispose();
                        return RedirectToAction(ProfilControllerVeActionlari.Index, ProfilControllerVeActionlari.ControllerName);
                    }
                    else
                    {
                        this.SetEventLogTipValue(Pekistirec.Engine.EventLog.EventLogTipleri.KULLANICI_GIRIS_EXTERNAL_GOOGLE_HATA);
                        this.SetEventLogMessageValue(googleUser.Id);

                        aspNetIdentityHelpers.SignOut();

                        repo.Dispose();
                        return RedirectToAction(AccountControllerVeActionlari.Login, AccountControllerVeActionlari.ControllerName);
                    }

                }
                else
                {
                    // Hata Türkçeleştirme
                    Dictionary<string, string> EngTur = new Dictionary<string, string>() { 
                        {"is already taken", "Bu kullanıcı adı daha önceden alındı."}
                    };

                    List<string> cErrors = new List<string>();
                    result.Errors.ToList().ForEach((e) =>
                    {
                        foreach (var ceviri in EngTur)
                            cErrors.Add((e.IndexOf(ceviri.Key) > 0) ? ceviri.Value : e);
                    });

                    IdentityResult tResult = new IdentityResult(cErrors);

                    aspNetIdentityHelpers.AddErrors(tResult);

                    this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.KULLANICI_KAYIT_EXTERNAL_GOOGLE_HATA);
                    this.SetEventLogMessageValue(String.Join("", result.Errors.ToArray()));
                }
            }
            return View(responseModel);
        }
        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ GOOGLECALLBACK ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 



        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ HELPERS ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region HELPERS

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(HomeControllerVeActionlari.Index, HomeControllerVeActionlari.ControllerName);
            }
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ HELPERS ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 
    }
}