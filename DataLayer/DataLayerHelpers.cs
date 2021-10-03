using DataLayer.Context;
using DataLayer.DomainClasses.BaseDomainClasses;
using DataLayer.DomainClasses.DokumanDomainClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class DataLayerHelpers
    {
        static public void YeniVeritabani()
        {
            FullContext context = new FullContext();
            context.Etiketler.Add(new Etiket() { Ad = "Bilişim Teknolojileri", Tur = EtiketTurleri.Brans });
            context.Etiketler.Add(new Etiket() { Ad = "Türkçe", Tur = EtiketTurleri.Brans });
            context.Etiketler.Add(new Etiket() { Ad = "Bilim Fen ve Teknoloji", Tur = EtiketTurleri.Kulup });
            context.Etiketler.Add(new Etiket() { Ad = "2014-2015", Tur = EtiketTurleri.EgitimYili });
            context.Etiketler.Add(new Etiket() { Ad = "Yıllık Plan", Tur = EtiketTurleri.DokumanTuru });
            context.Etiketler.Add(new Etiket() { Ad = "Zümre", Tur = EtiketTurleri.DokumanTuru });
            //context.Ayarlar.Add(new Ayarlar() { reCaptchaEnabled = true });            
            context.SaveChanges();
        }
    }
}
