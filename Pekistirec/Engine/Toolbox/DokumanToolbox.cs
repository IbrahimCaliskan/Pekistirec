using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.Toolbox.DokumanToolbox
{
    public enum DokumanUzantilari
    {
        dummy,
        docx,
        xlsx
    }

    public class DummyDokumanFileTool : MyInterfaces.DokumanInterfaces.IDokumanReadAndEditTool
    {

        public bool Kontrol(string FilePath)
        {
            return false;
        }

        public List<string> ParametreAra(string FilePath, string baslangicText, string bitisText, out string docText)
        {
            docText = "DummyDokumanTool";
            return new List<string>() { "DummyDokumanTool" };
        }

        public void FindAndReplace(string FilePath, Dictionary<string, string> findReplaceList)
        {
            
        }
    }

    public class DokumanTools
    {

        public static Dictionary<string, DokumanUzantilari> DokumanUzantiList = new Dictionary<string, DokumanUzantilari>()
        {            
            {"docx",DokumanUzantilari.docx},
            {"xlsx",DokumanUzantilari.xlsx}
        };

        public static DokumanUzantilari GetDokumanUzanti(string uzanti)
        {

            if (DokumanUzantiList.Keys.Where(k => k == uzanti).FirstOrDefault() != null)
            {
                return DokumanUzantiList.Where(u => u.Key == uzanti).First().Value;
            }

            return DokumanUzantilari.dummy;
        }

        public static Dictionary<DokumanUzantilari, string> DokumanUzantiMimeList = new Dictionary<DokumanUzantilari, string>() {
            {DokumanUzantilari.dummy,"application/dummy"},
            {DokumanUzantilari.docx,"application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            {DokumanUzantilari.xlsx,"application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"}
        };

        public static string GetDokumanUzantiMime(DokumanUzantilari uzanti)
        {
            return DokumanUzantiMimeList.Where(l => l.Key == uzanti).First().Value;
        }

        public static string GetDokumanUzantiMime(string uzanti)
        {
            var dokumanUzanti = GetDokumanUzanti(uzanti);
            return DokumanUzantiMimeList.Where(l => l.Key == dokumanUzanti).First().Value;
        }

        public MyInterfaces.DokumanInterfaces.IDokumanReadAndEditTool GetDokumanFileTool(string uzanti)
        {
            var dokumanUzanti = GetDokumanUzanti(uzanti);

            switch (dokumanUzanti)
            {
                case DokumanUzantilari.dummy: return new DummyDokumanFileTool();
                case DokumanUzantilari.docx: return new OpenXML.Word();
                case DokumanUzantilari.xlsx: return new OpenXML.Excel();
                default: return new DummyDokumanFileTool();
            }
        }

        public MyInterfaces.DokumanInterfaces.IDokumanReadAndEditTool GetDokumanFileTool(DokumanUzantilari uzanti)
        {
            switch (uzanti)
            {
                case DokumanUzantilari.dummy: return new DummyDokumanFileTool();
                case DokumanUzantilari.docx: return new OpenXML.Word();
                case DokumanUzantilari.xlsx: return new OpenXML.Excel();
                default: return new DummyDokumanFileTool();
            }
        }

        public static string DokumanAdiOlustur(DataLayer.DomainClasses.DokumanDomainClasses.Dokuman dokuman)
        {
            return dokuman.Baslik;
            //string Ozellikler = "";
            //dokuman.Ozellikler.OrderBy(o => o.Ozellik).ToList().ForEach(o => Ozellikler += o.Ozellik.Ad + " ");
            //Ozellikler.Substring(Ozellikler.Length - 1, 1);

            //return String.Format("{0} {1} {2}",
            //    Ozellikler,
            //    (dokuman.DokumanTur != null) ? dokuman.DokumanTur.Ad ?? "" : "",
            //    dokuman.Baslik ?? "");
        }

        public static string DokumanDuzenleLinkiOlustur(string dokumanId, string editkey = null)
        {
            var controllerAndActionName = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetControllerAndActionName<Pekistirec.Controllers.DokumanController>(c => c.Duzenle("",""));
            if (String.IsNullOrEmpty(editkey))
            {
                return String.Format("/{0}/{1}/{2}", controllerAndActionName.ControllerName, controllerAndActionName.ActionName, dokumanId);
            }
            else
            {
                return String.Format("/{0}/{1}/{2}?editkey={3}", controllerAndActionName.ControllerName, controllerAndActionName.ActionName, dokumanId, editkey);
            }
        }

        public static string DokumanPaylasimLinkiOlustur(string siteRootURL, string dokumanId)
        {
            var controllerAndActionName = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetControllerAndActionName<Pekistirec.Controllers.DokumanController>(c => c.d(""));            

            return String.Format("{0}{1}/{2}/{3}",siteRootURL, controllerAndActionName.ControllerName, controllerAndActionName.ActionName, dokumanId);
        }
    }
}