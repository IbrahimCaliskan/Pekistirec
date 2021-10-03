using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.PekistirecAyarlar
{
    public enum PekistirecAyarAdlari
    {
        EVENTLOG_ENABLED,
        EVENTLOG_BUFFER_SIZE,
        RECAPTCHA_ENABLED,
        RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI,
        RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI,
        RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI,
        RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI
    }

    public abstract class absAyar<T>
    {
        public PekistirecAyarAdlari Ad { get; private set; }
        public T DefaultDeger { get; private set; }

        protected static List<DataLayer.DomainClasses.BaseDomainClasses.Ayar> DbAyarlar = new DataLayer.Repositories.BaseRepositories.AyarlarRepository().All.ToList();
        protected static void InsertOrUpdate(string ad , string deger)
        {            
            using (var repo = new DataLayer.Repositories.BaseRepositories.AyarlarRepository())
            {
                var dbAyar = repo.All.Where(a => a.AyarAdi == ad).FirstOrDefault();
                bool forceInsert = dbAyar == null;

                if (dbAyar == null)
                {
                    repo.InsertOrUpdate(new DataLayer.DomainClasses.BaseDomainClasses.Ayar() { AyarAdi = ad, Deger = deger }, forceInsert);
                    repo.Save();

                    DbAyarlar = repo.All.ToList();
                }
                else
                    if (dbAyar.Deger != deger)
                    {
                        dbAyar.Deger = deger;
                        repo.Save();

                        DbAyarlar = repo.All.ToList();
                    }
            }
        }

        public absAyar(PekistirecAyarAdlari Ad, T DefaultDeger)
        {
            this.Ad = Ad;
            this.DefaultDeger = DefaultDeger;
        }
    }

    public class BoolAyar : absAyar<bool>
    {
        public BoolAyar(PekistirecAyarAdlari Ad, bool DefaultDeger)
            : base(Ad, DefaultDeger)
        {

        }

        public bool Deger
        {
            get
            {
                var ayar = DbAyarlar.Where(a => a.AyarAdi == Ad.ToString()).FirstOrDefault();
                if (ayar == null)
                {
                    InsertOrUpdate(Ad.ToString(), DefaultDeger.ToString());
                    return DefaultDeger;
                }

                if (string.IsNullOrEmpty(ayar.Deger))
                {
                    InsertOrUpdate(Ad.ToString(), DefaultDeger.ToString());
                    return DefaultDeger;
                }

                bool donecekDeger = DefaultDeger;

                if (!bool.TryParse(ayar.Deger, out donecekDeger))
                {
                    InsertOrUpdate(Ad.ToString(), DefaultDeger.ToString());
                    return DefaultDeger;
                }
                else
                {
                    return donecekDeger;
                }

            }
            set
            {
                InsertOrUpdate(Ad.ToString(), value.ToString());
            }

        }
    }

    public class IntAyar : absAyar<int>
    {
        public int MinDeger { get; private set; }
        public int MaxDeger { get; private set; }

        public IntAyar(PekistirecAyarAdlari Ad, int DefaultDeger, int MinDeger, int MaxDeger)
            : base(Ad, DefaultDeger)
        {
            this.MinDeger = MinDeger;
            this.MaxDeger = MaxDeger;
        }

        public int Deger
        {
            get
            {
                var ayar = DbAyarlar.Where(a => a.AyarAdi == Ad.ToString()).FirstOrDefault();
                if (ayar == null)
                {
                    InsertOrUpdate(Ad.ToString(), DefaultDeger.ToString());
                    return DefaultDeger;
                }

                if (string.IsNullOrEmpty(ayar.Deger))
                {
                    InsertOrUpdate(Ad.ToString(), DefaultDeger.ToString());
                    return DefaultDeger;
                }

                int donecekDeger = DefaultDeger;
                if (!int.TryParse(ayar.Deger, out donecekDeger))
                {
                    InsertOrUpdate(Ad.ToString(), DefaultDeger.ToString());
                    return DefaultDeger;
                }
                else
                {
                    if (donecekDeger < MinDeger || donecekDeger > MaxDeger)
                    {
                        InsertOrUpdate(Ad.ToString(), DefaultDeger.ToString());
                        return DefaultDeger;
                    }
                    else
                    {
                        return donecekDeger;
                    }
                }
            }

            set
            {
                if (value < MinDeger || value > MaxDeger)
                {
                    value = DefaultDeger;
                }

                InsertOrUpdate(Ad.ToString(), value.ToString());
            }
        }
    }

    public class Ayarlar
    {

        public static BoolAyar EVENTLOG_ENABLED = new BoolAyar(PekistirecAyarAdlari.EVENTLOG_ENABLED, true);

        public static IntAyar EVENTLOG_BUFFER_SIZE = new IntAyar(PekistirecAyarAdlari.EVENTLOG_BUFFER_SIZE, 3, 1, 99);

        public static BoolAyar RECAPTCHA_ENABLED = new BoolAyar(PekistirecAyarAdlari.RECAPTCHA_ENABLED, true);

        public static IntAyar RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI = new IntAyar(PekistirecAyarAdlari.RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI, 3, 1, 99);

        public static IntAyar RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI = new IntAyar(PekistirecAyarAdlari.RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI, 3, 1, 99);

        public static IntAyar RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI = new IntAyar(PekistirecAyarAdlari.RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI, 10, 1, 99);

        public static IntAyar RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI = new IntAyar(PekistirecAyarAdlari.RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI, 10, 1, 99);

    }
}