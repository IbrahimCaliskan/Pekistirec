using LinqKit;
using Pekistirec.Engine.PekistirecConsts;
using Pekistirec.Engine.PekistirecConsts.ControllerlarVeActionlari;
using Pekistirec.Models.DokumanModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec.Controllers
{
    public class DokumanIndexParameter
    {
        public int Sayfa { get; set; }
        public string Etiket { get; set; }
    }

    [Authorize]
    public class DokumanController : Controller
    {
        [AllowAnonymous]
        public ActionResult Index(int Sayfa = 1, string Etiket = "", string Ara = "",bool AramaTipi = false)
        {            
            var model = new Models.DokumanModels.DokumanIndexViewModel();
            List<string> Etiketler = new List<string>();

            if (!String.IsNullOrEmpty(Etiket))
            {
                Etiket = Etiket.Trim();
                if (!String.IsNullOrEmpty(Etiket) && !String.IsNullOrWhiteSpace(Etiket))
                {
                    Etiketler = Etiket.Split(',').ToList();
                    var tempEtiketler = Etiketler.ToList();
                    Etiketler.Clear();
                    foreach (var item in tempEtiketler)
                    {
                        Etiketler.Add(item.Trim());
                    }
                }
            }

            if (Etiketler.Count > 0)
            {
                this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.DOKUMAN_FILTRELE);
                this.SetEventLogMessageValue(Etiket);

                using (var uow = new DataLayer.UnitOfWorks.DokumanUnitOfWork())
                {
                    using (var etiketRepo = new DataLayer.Repositories.BaseRepositories.EtiketRepository(uow))
                    {
                        var dbEtiketIdleri = etiketRepo.All.Where(e => Etiketler.Contains(e.Ad)).Distinct().Select(e => e.Id);
                        using (var etiketDokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanEtiketRepository(uow))
                        {
                            using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository(uow))
                            {
                                var query = dokumanRepo.All
                                            .Where(d => d.Ozel != true)                                          
                                            .Where
                                            (d =>
                                                etiketDokumanRepo.All
                                                .Where(ed => dbEtiketIdleri.Contains(ed.EtiketId))
                                                .GroupBy(ed => ed.DokumanId)
                                                .Where(ed => ed.Count() == dbEtiketIdleri.Count())
                                                .Select(ed => ed.Key)
                                                .Contains(d.Id)
                                            )
                                            .Distinct()
                                            .OrderByDescending(ed => ed.EklenmeTarihi)
                                            .Skip((Sayfa - 1) * DokumanConsts.SayfadaListelenecekDokumanSayisi)
                                            .Take(DokumanConsts.SayfadaListelenecekDokumanSayisi);

                                model.dokumanlar = query.ToList();
                            }
                        }
                    }
                }

                return View(model);
            }

            Ara = Ara.Trim();
            List<string> Arananlar = new List<string>();
            if (!String.IsNullOrEmpty(Ara) && !String.IsNullOrWhiteSpace(Ara))
            {                                
                Ara.Split(' ').ToList().ForEach(t => Arananlar.Add(t.Trim()));
            }
            if (Arananlar.Count > 0)
            {
                if (AramaTipi == true) // İçeriklerde Ara
                {
                    this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.DOKUMAN_ARA_ICERIK);
                    this.SetEventLogMessageValue(Ara);

                    List<Guid> dokumanIdleri = new List<Guid>();
                    using (var ftsDokumanRepo = new DataLayer.Repositories.FtsRepositories.FtsDokumanRepository())
                    {
                        string sqlQuery = "SELECT DokumanId FROM FREETEXTTABLE(FtsDokumanlar, doccontent, @param1, 20) AS CT  INNER JOIN FtsDokumanlar AS D ON CT.[KEY] = D.Id  ORDER BY CT.[RANK] DESC;";                        
                        dokumanIdleri = ftsDokumanRepo.SqlQuery<Guid>(sqlQuery, Ara).ToList();
                    }

                    using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
                    {
                        model.dokumanlar = dokumanRepo.All.Where(d => dokumanIdleri.Contains(d.Id)).ToList();
                    }

                    return View(model);
                }
                else
                {
                    this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.DOKUMAN_ARA_BASLIK);
                    this.SetEventLogMessageValue(Ara);

                    using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
                    {
                        var araPredicate = PredicateBuilder.True<DataLayer.DomainClasses.DokumanDomainClasses.Dokuman>();

                        foreach (string aranan in Arananlar)
                        {
                            string temp = aranan;
                            araPredicate = araPredicate.And(p => p.Baslik.Contains(temp));
                        }
                        model.dokumanlar = dokumanRepo.All.AsExpandable().Where(araPredicate).OrderBy(d => d.EklenmeTarihi).Skip((Sayfa - 1) * DokumanConsts.SayfadaListelenecekDokumanSayisi).Take(DokumanConsts.SayfadaListelenecekDokumanSayisi).ToList();
                    }
                    return View(model);
                }
            }
            

            List<DataLayer.DomainClasses.DokumanDomainClasses.Dokuman> dokumanlar = new List<DataLayer.DomainClasses.DokumanDomainClasses.Dokuman>();
            using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
            {
                model.dokumanlar = dokumanRepo.All.Where(d=>d.Ozel != true).OrderBy(d => d.EklenmeTarihi).Skip((Sayfa - 1) * DokumanConsts.SayfadaListelenecekDokumanSayisi).Take(DokumanConsts.SayfadaListelenecekDokumanSayisi).ToList();
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Rehber()
        {
            return View();
        }

        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ YÜKLEDİKLERİM ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region YÜKLEDİKLERİM        
        public ActionResult Yuklediklerim()
        {           
            string userId = this.GetUserId();

            List<DataLayer.DomainClasses.DokumanDomainClasses.Dokuman> dokumanlar = new List<DataLayer.DomainClasses.DokumanDomainClasses.Dokuman>();
            using (var repo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
            {
                dokumanlar = repo.AllIncluding(d => d.Etiketler,d => d.DokumanDegiskenleri).Where(d => d.EkleyenAspNetUserId == userId).ToList();
            }

            var model = new Models.DokumanModels.DokumanYuklediklerimViewModel()
            {
                Dokumanlar = dokumanlar,
                SiteRootURL = this.GetRootURL()
            };

            return View(model);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ YÜKLEDİKLERİM  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 

        //
        // GET: /Dokuman/
        //id DokumanId

        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ D ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region D

        [AllowAnonymous]
        [HttpGet]
        public ActionResult d(string id)
        {
            #region Id Gerçekten Bir String Değer Taşıyor mu?

            if (id == null)
            {
                return RedirectToAction(DokumanControllerVeActionlari.Index, DokumanControllerVeActionlari.ControllerName);
            }

            id = id.Trim();

            if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction(DokumanControllerVeActionlari.Index, DokumanControllerVeActionlari.ControllerName);
            }

            #endregion

            var model = new Pekistirec.Models.DokumanModels.DokumanDViewModel();
            model.DegiskenDegerleri = new List<Models.DokumanModels.DokumanDegiskenlerinDegerleri>();

            DataLayer.DomainClasses.DokumanDomainClasses.Dokuman dokuman = null;

            using (var uow = new DataLayer.UnitOfWorks.DokumanUnitOfWork())
            {
                using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository(uow))
                {
                    dokuman = dokumanRepo.AllIncluding(d => d.DokumanDegiskenleri).Where(d => d.Id == new Guid(id)).FirstOrDefault();

                    if (dokuman == null)
                    {
                        #region Döküman Liste Ekranınıa Yönlendirliyor...

                        uow.Dispose();
                        dokumanRepo.Dispose();
                        return RedirectToAction(DokumanControllerVeActionlari.Index, DokumanControllerVeActionlari.ControllerName);

                        #endregion
                    }

                    model.Dokuman = dokuman;

                    foreach (var dokumanDegisken in dokuman.DokumanDegiskenleri)
                    {
                        
                        model.DegiskenDegerleri.Add(new Models.DokumanModels.DokumanDegiskenlerinDegerleri() { Ad = dokumanDegisken.Ad, Deger = "", Aciklama = dokumanDegisken.Aciklama });
                    }

                    if (User.Identity.IsAuthenticated)
                    {
                        var userId = this.GetUserId();

                        using (var dokumanLinkRepo = new DataLayer.Repositories.DokumanRepositories.DokumanLinkRepository(uow))
                        {
                            var linkler = dokumanLinkRepo.AllIncluding(dkl => dkl.Dokuman).Where(dkl => dkl.DokumanId == dokuman.Id && dkl.AspNetUserId == userId).ToList();
                            if (linkler != null)
                                model.OncedenKaydedilmisDokumanlar = linkler;
                        }
                    }
                }
            }
            return View(model);
        }

        private static Object RefreshTokenGetirLock = new Object();
        private static int RefreshTokenIndex = -1;
        private static string RefreshTokenGetir()
        {
            lock (RefreshTokenGetirLock)
            {
                if (DataLayer.Buffers.GoogleRefreshTokenlari.Count > 0)
                {
                    RefreshTokenIndex++;
                    if (RefreshTokenIndex >= DataLayer.Buffers.GoogleRefreshTokenlari.Count)
                    {
                        RefreshTokenIndex = 0;
                    }

                    return DataLayer.Buffers.GoogleRefreshTokenlari[RefreshTokenIndex].RefreshToken;
                }
                else
                {
                    return null;
                }                               
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult d(Pekistirec.Models.DokumanModels.DokumanDViewModel model, string id)
        {
          
            if (ModelState.IsValid)
            {               
                bool userIsAuthenticated = User.Identity.IsAuthenticated;
                string userId = this.GetUserId();

                #region Id Kontrol

                if (id == null)
                {
                    return RedirectToAction(DokumanControllerVeActionlari.Index, DokumanControllerVeActionlari.ControllerName);
                }

                id = id.Trim();

                if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
                {
                    return RedirectToAction(DokumanControllerVeActionlari.Index, DokumanControllerVeActionlari.ControllerName);
                }

                DataLayer.DomainClasses.DokumanDomainClasses.Dokuman dokuman = null;
                using (var uow = new DataLayer.UnitOfWorks.DokumanUnitOfWork())
                {
                    using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository(uow))
                    {
                        dokuman = dokumanRepo.AllIncluding(d => d.Etiketler, d => d.DokumanDegiskenleri).Where(d => d.Id == new Guid(id)).FirstOrDefault();

                        if (dokuman == null)
                        {
                            dokumanRepo.Dispose();
                            uow.Dispose();
                            return RedirectToAction(DokumanControllerVeActionlari.Index, DokumanControllerVeActionlari.ControllerName);
                        }

                        model.Dokuman = dokuman;
                    }
                }

                #endregion

                #region benimDosyaYolum=> Content Klasöründe Döküman Yoksa İndirilip Ardından Kullanıcı İçin Unique İsimle Kopyalanıyor

                string dokumanDosyaYolu = Pekistirec.Engine.Toolbox.DosyaVeDizinToolbox.DosyaVeDizinTools.DokumanFolder.GercekPath + "\\" + dokuman.Id.ToString() + "." + dokuman.Uzanti;
                if (!System.IO.File.Exists(dokumanDosyaYolu))
                {
                    new AzureAPI.Implementation.DokumanBlob().Download(dokuman.Id.ToString(), dokumanDosyaYolu);
                }

                string benimDosyaYolum = Pekistirec.Engine.Toolbox.DosyaVeDizinToolbox.DosyaVeDizinTools.DosyaAdiTureterekKaydetmeYoluGetir(Pekistirec.Engine.Toolbox.DosyaVeDizinToolbox.DosyaVeDizinTools.TempFolder) + "." + dokuman.Uzanti;
                System.IO.File.Copy(dokumanDosyaYolu, benimDosyaYolum);

                #endregion

                if (dokuman.DokumanDegiskenleri != null)
                {
                    #region hataVar => Gelen Değişkenler Kontrol Ediliyor

                    bool hataVar = false;

                    var gelenDegiskenDegerleri = model.DegiskenDegerleri;
                    var dokumanDegiskenleri = dokuman.DokumanDegiskenleri;

                    var liste = new List<Pekistirec.Models.DokumanModels.DokumanDegiskenlerinDegerleri>();

                    for (int i = 0; i < dokumanDegiskenleri.Count; i++)
                    {
                        var gelenDegisken = gelenDegiskenDegerleri.Where(dd => dd.Ad == dokumanDegiskenleri[i].Ad).FirstOrDefault();
                        if (gelenDegisken == null)
                        {
                            liste.Add(new Models.DokumanModels.DokumanDegiskenlerinDegerleri() { Ad = dokumanDegiskenleri[i].Ad, Deger = dokumanDegiskenleri[i].Ad, Aciklama = dokumanDegiskenleri[i].Aciklama });
                            ModelState.AddModelError(String.Format("DegiskenDegerleri[{0}].Deger", i), "Bir değer girmelisiniz.");
                            hataVar = true;
                        }                        
                    }

                    #endregion

                    if (hataVar)
                    {
                        model.DegiskenDegerleri = liste;
                        return View(model);
                    }

                    #region bosDegiskenDegeriVar Boş Değişken Değeri Var Mı?

                    bool bosDegiskenDegeriVar = false;
                    for (int i = 0; i < model.DegiskenDegerleri.Count; i++)
                    {                        
                        string tempGelenDegiskenDeger = model.DegiskenDegerleri[i].Deger != null ? model.DegiskenDegerleri[i].Deger.Trim() : null;

                        if (String.IsNullOrEmpty(tempGelenDegiskenDeger) || string.IsNullOrWhiteSpace(tempGelenDegiskenDeger))
                        {
                            ModelState.AddModelError(String.Format("DegiskenDegerleri[{0}].Deger", i), "Bir değer girmelisiniz.");
                            bosDegiskenDegeriVar = true;
                        }
                    }

                    #endregion

                    if (bosDegiskenDegeriVar)
                    {
                        foreach (var item in dokuman.DokumanDegiskenleri)
                        {
                            model.DegiskenDegerleri.Where(gd => gd.Ad == item.Ad).First().Aciklama = item.Aciklama;
                        }
                        return View(model);
                    }

                    #region FindAndReplace ve DokumanKullaniciDegiskenDeger Kaydetme

                    Dictionary<string, string> bulDegistirListe = new Dictionary<string, string>();

                    using (var dokumanKullaniciDegiskenRepo = new DataLayer.Repositories.DokumanRepositories.DokumanDegiskenKullaniciDegerRepository())
                    {
                        bool dokumanKullaniciDegiskenRepoCallSave = false;

                        foreach (var gelenDegiskenDeger in gelenDegiskenDegerleri)
                        {
                            #region Kayıtlı Kullanıcıysa Değişken Değerlerini Kaydet

                            if (userIsAuthenticated)
                            {
                                dokumanKullaniciDegiskenRepo.InsertOrUpdate(new DataLayer.DomainClasses.DokumanDomainClasses.DokumanDegiskenKullaniciDeger()
                                {
                                    AspNetUserId = userId,
                                    Degisken = gelenDegiskenDeger.Ad,
                                    Deger = gelenDegiskenDeger.Deger,
                                    DokumanDegiskenId = dokuman.DokumanDegiskenleri.Where(dd=>dd.Ad == gelenDegiskenDeger.Ad).First().Id                                    
                                });
                                dokumanKullaniciDegiskenRepoCallSave = true;
                            }

                            #endregion

                            bulDegistirListe.Add(gelenDegiskenDeger.Ad, gelenDegiskenDeger.Deger);
                        }

                        if (dokumanKullaniciDegiskenRepoCallSave)
                        {
                            dokumanKullaniciDegiskenRepo.Save();
                        }
                    }



                    var tool = new Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools().GetDokumanFileTool(dokuman.Uzanti);
                    tool.FindAndReplace(benimDosyaYolum, bulDegistirListe);

                    #endregion
                }

                #region googleLink => Google Upload

                var googleUtils = new GoogleAPI.Utils();
                GoogleAPI.GoogleAPICarrier googleApiCarrier = null;
                bool kullanicinKendiHesabıVar = false;

                if (User.Identity.IsAuthenticated)
                {
                    if (!String.IsNullOrEmpty(this.GetUser().GoogleRefreshToken))
                    {
                        googleApiCarrier = googleUtils.Login(this.GetUser().GoogleRefreshToken);
                        if (googleApiCarrier != null) kullanicinKendiHesabıVar = true;
                    }
                }

                if (googleApiCarrier == null)
                {
                    googleApiCarrier = googleUtils.Login(RefreshTokenGetir());
                }

                if (googleApiCarrier == null)
                {
                    return View(Pekistirec.Views.VIEW_NAMES.SharedViewNames.Error);
                }

                string title = Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools.DokumanAdiOlustur(dokuman);
                string mime = Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools.GetDokumanUzantiMime(dokuman.Uzanti);
                string googleLink = "";

                if (kullanicinKendiHesabıVar)
                {
                    googleLink = googleUtils.Upload(googleApiCarrier, benimDosyaYolum, title, mime, GoogleAPI.Utils.UploadPermission.OnlyMe);
                }
                else
                {
                    googleLink = googleUtils.Upload(googleApiCarrier, benimDosyaYolum, title, mime, GoogleAPI.Utils.UploadPermission.EveryoneWrite);
                }

                #endregion

                string dbLinkId = null;

                using (var repo = new DataLayer.Repositories.DokumanRepositories.DokumanLinkRepository())
                {
                    var dbLink = new DataLayer.DomainClasses.DokumanDomainClasses.DokumanLink()
                    {
                        AspNetUserId = userId,
                        DokumanId = new Guid(id),
                        Link = googleLink,
                        EklenmeTarihi = DateTime.Now
                    };

                    repo.InsertOrUpdate(dbLink);
                    repo.Save();
                    dbLinkId = dbLink.Id.ToString();
                }


                return RedirectToAction(DokumanControllerVeActionlari.g, new { id = dbLinkId });
            }

            return View(model);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ D  ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 



        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ YUKLE ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region YUKLE        

        private static bool YukleReCaptchaGerekliMi(string userIp)
        {
            bool reCaptchaGerekliMi = false;
            var eventLog = Pekistirec.Engine.EventLog.EventLogHelpers.Son24SaatIcindekiLoglariGetirIPveLogTipineGore(userIp, Pekistirec.Engine.EventLog.EventLogTipleri.DOKUMAN_YUKLENDI);
            reCaptchaGerekliMi = (eventLog.Count() > Pekistirec.Engine.PekistirecAyarlar.Ayarlar.RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI.Deger);
            return reCaptchaGerekliMi;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Yukle()
        {
            var model = new Pekistirec.Models.DokumanModels.DokumanYukleViewModel()
            {
                reCaptchaGerekliMi = YukleReCaptchaGerekliMi(this.GetUserIP())
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Yukle(DokumanYukleViewModel model)
        {
            this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.DOKUMAN_YUKLENEMEDI_HATA);
            this.SetEventLogMessageValue("ModelStateIsValid = false");

            bool reCaptchaGerekliMi = YukleReCaptchaGerekliMi(this.GetUserIP());

            model.reCaptchaGerekliMi = reCaptchaGerekliMi;

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
                       
            //if (model.Ozel)
            //{
            //    model.Baslik = Pekistirec.Engine.PekistirecConsts.DokumanConsts.OzelDokumanBaslik;
            //    if (ModelState.ContainsKey("Baslik"))
            //        ModelState["Baslik"].Errors.Clear();
            //}

            if (ModelState.IsValid)
            {

                string filePath = Pekistirec.Engine.Toolbox.DosyaVeDizinToolbox.DosyaVeDizinTools.TempUpload(Request.Files[0]);

                string extension = new System.IO.FileInfo(filePath).Extension.ToString().Substring(1);

                var dt = new Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools();
                MyInterfaces.DokumanInterfaces.IDokumanReadAndEditTool dokumanFileTool = dt.GetDokumanFileTool(extension);

                #region DOSYA TÜR KONTROL => dosyaTuruGecerli

                this.SetEventLogMessageValue("Dosya Turu Kontrol Hata");

                bool dosyaTuruGecerli = dokumanFileTool.Kontrol(filePath);

                #endregion

                if (dosyaTuruGecerli == false)
                {
                    #region DOSYA FORMATI GEÇERSİZ , UPLOAD EDİLEN DOSYAYI SİL VE HATA GÖNDER

                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }

                    ModelState.AddModelError(
                        Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetCorrectPropertyName<DokumanYukleViewModel>(p => p.Dosya)
                        , "Geçersiz dosya türü.");

                    #endregion
                }
                else
                {
                    #region DOKUMAN VERİTABANINA KAYDEDİLİYOR => dokuman

                    this.SetEventLogMessageValue("Dookuman DB Kayıt Hata");

                    string dokumanEditKey = null;
                    if (!User.Identity.IsAuthenticated)
                    {
                        var dokumanEditKeyDateTime = DateTime.Now;
                        string md5Source = String.Format("{0}{1}{2}{3}{4}{5}{6}", dokumanEditKeyDateTime.Year, dokumanEditKeyDateTime.Month, dokumanEditKeyDateTime.Day, dokumanEditKeyDateTime.Hour, dokumanEditKeyDateTime.Minute, dokumanEditKeyDateTime.Second, dokumanEditKeyDateTime.Millisecond);
                        dokumanEditKey = Engine.Toolbox.GenelToolbox.GenelTools.GetMd5Hash(md5Source);
                    }

                    var dokuman = new DataLayer.DomainClasses.DokumanDomainClasses.Dokuman()
                    {
                        EklenmeTarihi = DateTime.Now,
                        EkleyenAspNetUserId = this.GetUserId(),
                        IsDeleted = false,
                        Ozel = model.Ozel,
                        Uzanti = extension,
                        EditKey = dokumanEditKey,
                        Baslik = model.Baslik,
                        OnayDurumu = (model.Ozel) ? DataLayer.DomainClasses.DokumanDomainClasses.DokumanOnayDurumu.Gerekmiyor : DataLayer.DomainClasses.DokumanDomainClasses.DokumanOnayDurumu.Bekliyor
                    };

                    using (var repo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
                    {
                        repo.InsertOrUpdate(dokuman);
                        repo.Save();
                    }

                    #endregion

                    #region DOKUMAN AZURE BLOB STORAGE EKLENİYOR

                    this.SetEventLogMessageValue("Azure Yükleme Hata");

                    var azure = new AzureAPI.Implementation.DokumanBlob().Upload(dokuman.Id.ToString(), filePath);

                    #endregion

                    #region DOKUMAN ETİKETLERİ EKLENİYOR

                    if (model.Ozel == false)
                    {
                        if (!String.IsNullOrEmpty(model.Etiketler) && !String.IsNullOrWhiteSpace(model.Etiketler))
                        {
                            if (model.Etiketler.Split(',').Length > 0)
                            {
                                this.SetEventLogMessageValue("Dokuman Etiket Hata");

                                var postedEtiketler = model.Etiketler.Split(',').Distinct().Take(10).ToList();
                                for (int i = 0; i < postedEtiketler.Count; i++) postedEtiketler[i] = postedEtiketler[i].Trim();

                                List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> dokumanaEklenmisDBdeOlmayanlaridaDByeEklenmisEtiketler;
                                using (var etiketRepo = new DataLayer.Repositories.BaseRepositories.EtiketRepository())
                                {
                                    List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> haliHazirdaDBdeMevcutEtiketler = new List<DataLayer.DomainClasses.BaseDomainClasses.Etiket>();

                                    var predicate = PredicateBuilder.False<DataLayer.DomainClasses.BaseDomainClasses.Etiket>();

                                    foreach (string postedEtiket in postedEtiketler)
                                    {
                                        string temp = postedEtiket;
                                        predicate = predicate.Or(p => p.Ad == temp);
                                    }

                                    haliHazirdaDBdeMevcutEtiketler = etiketRepo.All.AsExpandable().Where(predicate).ToList();

                                    #region VERİTABANINDA OLMAYAN ETİKETLER VERİTABANINA EKLENİYOR

                                    if (haliHazirdaDBdeMevcutEtiketler.Count != postedEtiketler.Count)
                                    {
                                        bool callSave = false;
                                        foreach (var etiket in postedEtiketler)
                                        {
                                            if (haliHazirdaDBdeMevcutEtiketler.Where(e => e.Ad == etiket).Count() < 1)
                                            {
                                                callSave = true;
                                                etiketRepo.InsertOrUpdate(new DataLayer.DomainClasses.BaseDomainClasses.Etiket() { Ad = etiket });
                                            }
                                        }
                                        if (callSave) etiketRepo.Save();
                                    }

                                    #endregion

                                    dokumanaEklenmisDBdeOlmayanlaridaDByeEklenmisEtiketler = etiketRepo.All.AsExpandable().Where(predicate).ToList();
                                }

                                #region DOKUMANETIKETLERI TABLOSUNA DOKUMAN<->ETIKET EKLEMESİ YAPILIYOR

                                using (var dokumanEtiketRepo = new DataLayer.Repositories.DokumanRepositories.DokumanEtiketRepository())
                                {
                                    foreach (var dbEtiket in dokumanaEklenmisDBdeOlmayanlaridaDByeEklenmisEtiketler)
                                    {
                                        dokumanEtiketRepo.InsertOrUpdate(new DataLayer.DomainClasses.DokumanDomainClasses.DokumanEtiket()
                                        {
                                            DokumanId = dokuman.Id,
                                            EtiketId = dbEtiket.Id
                                        });
                                    }
                                    dokumanEtiketRepo.Save();
                                }

                                #endregion

                            }
                        }
                    }
                    #endregion

                    #region DEĞİŞKENLER TARANIP EKLENİYOR

                    this.SetEventLogMessageValue("Dokuman Değişken Hata");

                    string docText = "";
                    List<string> dokumanDegiskenleri = dokumanFileTool.ParametreAra(filePath, DokumanConsts.DokumanParametreBaslangicText, DokumanConsts.DokumanParametreBitisText, out docText);
                    dokumanDegiskenleri = dokumanDegiskenleri.Distinct().ToList();

                    using (var dokumanDegiskenRepo = new DataLayer.Repositories.DokumanRepositories.DokumanDegiskenRepository())
                    {
                        bool callSave = false;
                        foreach (var p in dokumanDegiskenleri)
                        {
                            dokumanDegiskenRepo.InsertOrUpdate(new DataLayer.DomainClasses.DokumanDomainClasses.DokumanDegisken() { Ad = p, Aciklama = p.Replace(DokumanConsts.DokumanParametreBaslangicText, "").Replace(DokumanConsts.DokumanParametreBitisText, ""), DokumanId = dokuman.Id });
                            callSave = true;
                        }
                        if (callSave) dokumanDegiskenRepo.Save();
                    }

                    #endregion

                    #region FULL TEXT SEARCH İÇİN VERİTABANINA EKLENİYOR => FTS_HATA

                    bool FTS_HATA = false;

                    if (!model.Ozel)
                    {                        
                        try
                        {
                            using (var repo = new DataLayer.Repositories.FtsRepositories.FtsDokumanRepository())
                            {
                                docText = docText.Replace(Pekistirec.Engine.PekistirecConsts.DokumanConsts.DokumanParametreBaslangicText, "");
                                docText = docText.Replace(Pekistirec.Engine.PekistirecConsts.DokumanConsts.DokumanParametreBitisText, "");


                                repo.InsertOrUpdate(new DataLayer.DomainClasses.FtsDomainClasses.FtsDokuman()
                                    {
                                        Baslik = dokuman.Baslik,
                                        DokumanId = dokuman.Id,
                                        docexcerpt = docText,
                                        doctype = extension,
                                        Etiket = model.Etiketler,
                                        doccontent = System.IO.File.ReadAllBytes(filePath)
                                    });
                                repo.Save();
                            }
                        }
                        catch (Exception)
                        {
                            FTS_HATA = true;
                        }
                    }


                    #endregion

                    if (FTS_HATA)
                    {
                        this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.DOKUMAN_YUKLENDI_ANCAK_FTS_EKLENMEDI);
                    }
                    else
                    {
                        this.SetEventLogTipValue(Engine.EventLog.EventLogTipleri.DOKUMAN_YUKLENDI);
                    }

                    this.SetEventLogMessageValue(dokuman.Id.ToString());                    

                    if (User.Identity.IsAuthenticated)
                    {
                        return RedirectToAction(DokumanControllerVeActionlari.Duzenle,DokumanControllerVeActionlari.ControllerName,
                            new { id = dokuman.Id.ToString() }
                            );
                    }
                    else
                    {
                        return RedirectToAction(DokumanControllerVeActionlari.Duzenle, DokumanControllerVeActionlari.ControllerName,
                            new { id = dokuman.Id.ToString(), editkey = dokuman.EditKey }
                            );                        
                    }
                }
            }

            return View(model);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ YUKLE ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 



        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ DUZENLE ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓


        #region DUZENLE

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Duzenle(string id, string editkey = null)
        {
            var controllerAndActionNameDokumanYuklediklerim = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetControllerAndActionName<DokumanController>(c => c.Yuklediklerim());
            ActionResult redirectToDokumanYuklediklerim = RedirectToAction(controllerAndActionNameDokumanYuklediklerim.ActionName, controllerAndActionNameDokumanYuklediklerim.ControllerName);

            #region Id gerçekten string mi?

            if (id != null)
            {
                id = id.Trim();
                if (String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
                {
                    return redirectToDokumanYuklediklerim;
                }
            }
            else
            {
                return redirectToDokumanYuklediklerim;
            }

            #endregion

            DataLayer.DomainClasses.DokumanDomainClasses.Dokuman dokuman = null;

            using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
            {
                dokuman = dokumanRepo.AllIncluding(d => d.DokumanDegiskenleri, d => d.Etiketler).Where(d => d.Id == new Guid(id)).FirstOrDefault();
            }

                        
            if (dokuman == null)
            {
                #region Id geçersiz YÖNLENDİR

                return redirectToDokumanYuklediklerim;

                #endregion
            }

            if (String.IsNullOrEmpty(dokuman.EditKey))
            {
                if (dokuman.EkleyenAspNetUserId != this.GetUserId())
                {
                    return redirectToDokumanYuklediklerim;
                }
            }
            else
            {
                if (dokuman.EditKey != editkey)
                {
                    return RedirectToAction(DokumanControllerVeActionlari.Index);
                }
            }

            DokumanDuzenleViewModel responseModel = new DokumanDuzenleViewModel()
            {
                Degiskenler = new List<DokumanDuzenleDegiskenViewModel>(),
                Dokuman = dokuman
            };            
           
            if (dokuman.DokumanDegiskenleri != null)
            {
                #region Döküman Değişkenleri ResponseModel değişkenine aktarılıyor

                foreach (var p in dokuman.DokumanDegiskenleri)
                {
                    responseModel.Degiskenler.Add(new DokumanDuzenleDegiskenViewModel() { Ad = p.Ad, Aciklama = p.Aciklama });
                }

                #endregion
            }            

            return View(responseModel);
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Duzenle(string id, string editkey,DokumanDuzenleViewModel model )
        {
            if (ModelState.IsValid)
            {
                var controllerAndActionNameDokumanYuklediklerim = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetControllerAndActionName<DokumanController>(c => c.Yuklediklerim());
                ActionResult redirectToDokumanYuklediklerim = RedirectToAction(controllerAndActionNameDokumanYuklediklerim.ActionName, controllerAndActionNameDokumanYuklediklerim.ControllerName);

                #region id Gerçekten string mi? requestModel null mu?

                if (id == null)
                {
                    return redirectToDokumanYuklediklerim;
                }

                id = id.Trim();

                if (model == null || String.IsNullOrEmpty(id) || String.IsNullOrWhiteSpace(id))
                {
                    return redirectToDokumanYuklediklerim;
                }

                #endregion

                DataLayer.DomainClasses.DokumanDomainClasses.Dokuman dokuman = null;

                using (var dokumanRepo = new DataLayer.Repositories.DokumanRepositories.DokumanRepository())
                {
                    dokuman = dokumanRepo.AllIncluding(d => d.DokumanDegiskenleri, d => d.Etiketler).Where(d => d.Id == new Guid(id)).FirstOrDefault();
                   
                    if (dokuman == null)
                    {
                        #region dokuman id geçersiz YÖNLENDİRİLİYOR...

                        dokumanRepo.Dispose();
                        return redirectToDokumanYuklediklerim;

                        #endregion
                    }
                    
                    #region DokumanSanaMıAit

                    if (String.IsNullOrEmpty(dokuman.EditKey))
                    {
                        if (dokuman.EkleyenAspNetUserId != this.GetUserId())
                        {
                            dokumanRepo.Dispose();
                            return redirectToDokumanYuklediklerim;
                        }
                    }
                    else
                    {
                        if (dokuman.EditKey != editkey)
                        {
                            dokumanRepo.Dispose();
                            return RedirectToAction(DokumanControllerVeActionlari.Index);
                        }
                    }                    

                    #endregion

                    model.Dokuman = dokuman;

                    if (model.Sil)
                    {
                        #region Siliniyor ve Yönlendiriliyor...

                        using (var dokumanFtsRepo = new DataLayer.Repositories.FtsRepositories.FtsDokumanRepository())
                        {
                            var silinecekDokumanFtsIdler = dokumanFtsRepo.All.Where(d => d.DokumanId == dokuman.Id).Select(d => d.Id).ToList();

                            foreach (var silinecekDokumanFtsId in silinecekDokumanFtsIdler)
                            {
                                dokumanFtsRepo.Delete(silinecekDokumanFtsId);
                            }
                            
                            dokumanFtsRepo.Save();
                        }

                        dokuman.IsDeleted = true;
                        dokumanRepo.InsertOrUpdate(dokuman);
                        dokumanRepo.Save();
                        dokumanRepo.Dispose();
                        return redirectToDokumanYuklediklerim;

                        #endregion
                    }
                }                

                #region Boş Değişken Açıklaması Varsa Hata Döndür

                bool hataVar = false;

                string errorModelNameFormat = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetCorrectPropertyName<DokumanDuzenleViewModel>(ddd => ddd.Degiskenler) + "[{0}]." + Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetCorrectPropertyName<DokumanDuzenleDegiskenViewModel>(ddd => ddd.Aciklama);
                for (int i = 0; i < model.Degiskenler.Count; i++)
                {                    
                    if (String.IsNullOrEmpty((model.Degiskenler[i].Aciklama == null) ? null : model.Degiskenler[i].Aciklama.Trim()))
                    {
                        ModelState.AddModelError(String.Format(errorModelNameFormat, i), "Parametre boş geçilemez.");
                        hataVar = true;
                    }
                }

                if (hataVar) return View(model);

                #endregion
                
                using (var repo = new DataLayer.Repositories.DokumanRepositories.DokumanDegiskenRepository())
                {
                    var dokumanDegiskenleri = repo.All.Where(dd => dd.DokumanId == new Guid(id)).ToList();

                    bool callSave = false;

                    for (int i = 0; i < model.Degiskenler.Count; i++)
                    {
                        var dokumanDegiskeni = dokumanDegiskenleri.Where(dd => dd.Ad == model.Degiskenler[i].Ad).FirstOrDefault();

                        if (dokumanDegiskeni == null)
                        {
                            #region Gerçekte olmayan bir değişken post edilmiş. Silinip devam ediliyor...

                            model.Degiskenler.Remove(model.Degiskenler[i]);
                            i--;

                            #endregion
                        }
                        else
                        {
                            if (model.Degiskenler[i].Sil == true)
                            {
                                #region Değişken Siliniyor...

                                repo.Delete(dokumanDegiskeni.Id);
                                callSave = true;
                                model.Degiskenler.Remove(model.Degiskenler[i]);
                                i--;

                                #endregion
                            }
                            else
                            {
                                #region Yeni Değer Yazılıyor.

                                if (dokumanDegiskeni.Aciklama != model.Degiskenler[i].Aciklama)
                                {
                                    dokumanDegiskeni.Aciklama = model.Degiskenler[i].Aciklama;
                                    callSave = true;
                                }

                                #endregion
                            }
                        }
                    }

                    #region Veritananında olupta post edilmeyen değişkenler modele ekleniyor...

                    foreach (var dd in dokumanDegiskenleri)
                    {
                        if (model.Degiskenler.Where(p => p.Ad == dd.Ad).FirstOrDefault() == null)
                        {
                            model.Degiskenler.Add(new DokumanDuzenleDegiskenViewModel() { Ad = dd.Ad, Aciklama = dd.Aciklama });
                        }
                    }

                    #endregion

                    if (callSave) repo.Save();

                    #region Veritabanından Değişkenler tekrar çekilip modele aktarılıyor...

                    dokumanDegiskenleri = repo.All.Where(dd => dd.DokumanId == new Guid(id)).ToList();

                    if (dokumanDegiskenleri != null)
                    {
                        model.Degiskenler = new List<DokumanDuzenleDegiskenViewModel>();
                        foreach (var dd1 in dokumanDegiskenleri)
                        {
                            model.Degiskenler.Add(new DokumanDuzenleDegiskenViewModel() { Ad = dd1.Ad, Aciklama = dd1.Aciklama });
                        }
                    }

                    #endregion
                }


                if (String.IsNullOrEmpty(dokuman.EditKey))
                {
                    return RedirectToAction( DokumanControllerVeActionlari.Duzenle, DokumanControllerVeActionlari.ControllerName,
                                        new { id = id });
                }
                else
                {
                    return RedirectToAction(DokumanControllerVeActionlari.Duzenle, DokumanControllerVeActionlari.ControllerName,
                                        new { id = id, editkey = editkey });
                }                
            }

            return View(model);
        }

        #endregion


        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ DUZENLE ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 

      

        //↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓ g ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

        #region g

        [AllowAnonymous]
        public ActionResult g(string id)
        {

            var model = new Pekistirec.Models.DokumanModels.DokumanGViewModel();

            using (var repo = new DataLayer.Repositories.DokumanRepositories.DokumanLinkRepository())
            {
                var link = repo.All.Where(l => l.Id == new Guid(id)).FirstOrDefault();
                if (link == null)
                {
                    return RedirectToAction(DokumanControllerVeActionlari.Index);
                }

                model.Link = link.Link;
            }

            return View(model);
        }

        #endregion

        //↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ g ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑ 
    }
}