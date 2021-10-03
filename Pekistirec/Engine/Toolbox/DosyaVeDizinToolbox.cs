using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.Toolbox.DosyaVeDizinToolbox
{
    public class ContentDizin
    {
        public string GoreceliPath { get; private set; }                
        public string GercekPath { get; private set; }        

        public ContentDizin(string goreceliPath)
        {
            this.GercekPath = HttpContext.Current.Server.MapPath(goreceliPath);

            if (!Directory.Exists(this.GercekPath))
            {
                Directory.CreateDirectory(this.GercekPath);
            }
        }
    }

    public class DosyaVeDizinTools
    {
        public static ContentDizin TempFolder = new ContentDizin("~\\Content\\Temp");
        public static ContentDizin DokumanFolder = new ContentDizin("~\\Content\\Dokuman");
        public static ContentDizin UserAvatarFolder = new ContentDizin("~\\Content\\UserAvatar");

        public static string TempUpload(HttpPostedFileBase PostedFile)
        {
            try
            {
                if (PostedFile.ContentLength > 0)
                {
                    FileInfo fi = new FileInfo(PostedFile.FileName);
                    DateTime s = DateTime.Now;

                    string FullPath = DosyaAdiTureterekKaydetmeYoluGetir(TempFolder) + fi.Extension;
                    if (!File.Exists(FullPath))
                    {
                        PostedFile.SaveAs(FullPath);
                        return FullPath;
                    }
                }

                return "";

            }
            catch (Exception)
            {
                return "";
            }
        }

        
        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private static Object GeciciDosyaKaydetmeYoluGetirLock = new Object();
        private static DateTime SonDosyaYoluTuretilirkenKullanilanZaman { get; set; }
        public static string DosyaAdiTureterekKaydetmeYoluGetir(ContentDizin DosyaninKaydedilecegiDizin, string DosyaAdiBaslangicText = "")
        {
            string DosyaninKaydedilecegiGercekKlasorYolu = DosyaninKaydedilecegiDizin.GercekPath;
            string dosyaYolu = "";
            lock(GeciciDosyaKaydetmeYoluGetirLock)
            {                
                do
                {
                    var n = DateTime.Now;
                    do
                    {
                        if (n == SonDosyaYoluTuretilirkenKullanilanZaman)
                        {
                            n = DateTime.Now;
                        }
                        else
                        {
                            SonDosyaYoluTuretilirkenKullanilanZaman = n;
                            break;
                        }
                    } while (true);

                    dosyaYolu = String.Format("{0}\\{1}{2}{3}{4}{5}{6}{7}{8}", DosyaninKaydedilecegiGercekKlasorYolu, DosyaAdiBaslangicText, n.Year, n.Month, n.Day, n.Hour, n.Minute, n.Second, n.Millisecond);                    
                    if (!File.Exists(dosyaYolu))
                        break;
                } while (true);
            }
            return dosyaYolu;
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}