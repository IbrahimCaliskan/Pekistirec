using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pekistirec.Engine.Toolbox.DokumanToolbox;

namespace Pekistirec.Views.Dokuman
{
    public class YuklediklerimCodebehind
    {
        public List<string> DokumanIsimleri = new List<string>();
        public List<string> DokumanDuzenleLinkleri = new List<string>();
        public List<string> DokumanPaylasimLinkleri = new List<string>();
        public List<string> DokumanOnayDurumlari = new List<string>();
        public List<string> DokumanOnayDurumlariClass = new List<string>();
        public YuklediklerimCodebehind(Pekistirec.Models.DokumanModels.DokumanYuklediklerimViewModel model)
        {
            if (model.Dokumanlar != null)
            {
                foreach (var dokuman in model.Dokumanlar)
                {
                    string dokumanId = dokuman.Id.ToString();
                    DokumanIsimleri.Add(DokumanTools.DokumanAdiOlustur(dokuman));
                    DokumanDuzenleLinkleri.Add(DokumanTools.DokumanDuzenleLinkiOlustur(dokumanId));
                    DokumanPaylasimLinkleri.Add(DokumanTools.DokumanPaylasimLinkiOlustur(model.SiteRootURL, dokumanId));

                    string dokumanOnayDurumu = "Bekliyor";
                    string dokumanOnayDurumuClass = "label-warning";
                    if (dokuman.OnayDurumu == DataLayer.DomainClasses.DokumanDomainClasses.DokumanOnayDurumu.Onaylandi)
                    {
                        dokumanOnayDurumu = "Onaylandi";
                        dokumanOnayDurumuClass = "label-success";
                    }
                    if (dokuman.OnayDurumu == DataLayer.DomainClasses.DokumanDomainClasses.DokumanOnayDurumu.Red)
                    {
                        dokumanOnayDurumu = "Reddedildi";
                        dokumanOnayDurumuClass = "label-inverse";
                    }

                    if (dokuman.OnayDurumu == DataLayer.DomainClasses.DokumanDomainClasses.DokumanOnayDurumu.Gerekmiyor)
                    {
                        dokumanOnayDurumu = "Gerekmiyor";
                        dokumanOnayDurumuClass = "label-info";
                    }
                    DokumanOnayDurumlari.Add(dokumanOnayDurumu);
                    DokumanOnayDurumlariClass.Add(dokumanOnayDurumuClass);
                }
            }
        }
    }
}