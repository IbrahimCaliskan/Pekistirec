using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{    
    public static class Buffers
    {
        public static void Yenile()
        {
            OzellikleriYenile();

            _GoogleRefreshTokenlari = null;                    
        }

        #region Ozellikler

        public static event Action OzelliklerYenilendi;

        public static void OzellikleriYenile()
        {
            _Etiketler = null;
            var temp = Etiketler.ToList();
        }

        private static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> _Etiketler = null;
        public static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> Etiketler
        {
            get
            {
                if (_Etiketler == null)
                {
                    using (var repo = new Repositories.BaseRepositories.EtiketRepository())
                    {
                        _Etiketler = repo.All.ToList();
                    }

                    _DokumanTurleri = null;
                    var t1 = DokumanTurleri.ToList();

                    _EgitimYillari = null;
                    var t2 = EgitimYillari.ToList();

                    _Kulupler = null;
                    var t3 = Kulupler.ToList();

                    _Branslar = null;
                    var t4 = Branslar.ToList(); 

                    Action del = OzelliklerYenilendi as Action;
                    if (OzelliklerYenilendi != null)
                    {
                        OzelliklerYenilendi();
                    }
                }

                return _Etiketler;
            }
        }

        private static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> _Branslar = null;
        public static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> Branslar
        {
            get
            {
                if (_Branslar == null)
                {
                    _Branslar = Etiketler.Where(o => o.Tur == DomainClasses.BaseDomainClasses.EtiketTurleri.Brans).ToList();

                }
                return _Branslar;
            }
        }

        private static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> _EgitimYillari = null;
        public static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> EgitimYillari
        {
            get
            {
                if (_EgitimYillari == null)
                {
                    _EgitimYillari = Etiketler.Where(o => o.Tur == DomainClasses.BaseDomainClasses.EtiketTurleri.EgitimYili).ToList();
                }
                return _EgitimYillari;
            }
        }

        private static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> _Kulupler = null;
        public static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> Kulupler
        {
            get
            {
                if (_Kulupler == null)
                {
                    _Kulupler = Etiketler.Where(o => o.Tur == DomainClasses.BaseDomainClasses.EtiketTurleri.Kulup).ToList();
                }
                return _Kulupler;
            }
        }
        private static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> _DokumanTurleri = null;
        public static List<DataLayer.DomainClasses.BaseDomainClasses.Etiket> DokumanTurleri
        {
            get
            {
                if (_DokumanTurleri == null)
                {
                    _DokumanTurleri = Etiketler.Where(o => o.Tur == DomainClasses.BaseDomainClasses.EtiketTurleri.DokumanTuru).ToList();
                }
                return _DokumanTurleri;
            }
        }

        #endregion


        private static List<DataLayer.DomainClasses.BaseDomainClasses.GoogleRefreshToken> _GoogleRefreshTokenlari = null;
        public static List<DataLayer.DomainClasses.BaseDomainClasses.GoogleRefreshToken> GoogleRefreshTokenlari
        {
            get
            {
                if (_GoogleRefreshTokenlari == null)
                {
                    using (var repo = new DataLayer.Repositories.BaseRepositories.GoogleRefreshTokenRepository())
                    {
                        _GoogleRefreshTokenlari = repo.All.ToList();
                    }
                }
                return _GoogleRefreshTokenlari;
            }
        }


    }
}
