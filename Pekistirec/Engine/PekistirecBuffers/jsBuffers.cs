using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Engine.PekistirecBuffers
{
    public enum jsBufferTip
    {
        DokumanYukleBaslikArrayBuffer,
        DokumanUzanti
    }

    public static class jsBuffers
    {
        public static void Yenile(params jsBufferTip[] tipler)
        {
            foreach (var tip in tipler)
            {
                switch (tip)
                {
                    case jsBufferTip.DokumanYukleBaslikArrayBuffer: _OzelliklerArrayBuffer = null;
                        break;
                    case jsBufferTip.DokumanUzanti: { _DokumanUzantiArrayBuffer = null; _DokumanUzantiVerticalBarSperatorBuffer = null; }
                        break;
                }
            }           
        }        

        public static void DokumanYukleBaslikArrayBufferYenile()
        {
            Yenile(jsBufferTip.DokumanYukleBaslikArrayBuffer);
        }

        public static void DokumanUzantiBufferYenile()
        {
            Yenile(jsBufferTip.DokumanUzanti);
        }

        private static string _DokumanUzantiArrayBuffer = null;
        public static string DokumanUzantiArrayBuffer
        {
            get
            {
                if (_DokumanUzantiArrayBuffer == null)
                {
                    string temp = "[";
                    temp += string.Join(",", Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools.DokumanUzantiList.Select(k => "'" + k.Key + "'").ToList());
                    temp += "]";
                    _DokumanUzantiArrayBuffer = temp;
                }
                return _DokumanUzantiArrayBuffer;
            }
        }

        private static string _DokumanUzantiVerticalBarSperatorBuffer = null;
        public static string DokumanUzantiVerticalBarSperatorBuffer
        {
            get
            {
                if (_DokumanUzantiVerticalBarSperatorBuffer == null)
                {
                    string temp = "'";
                    temp += string.Join("|", Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools.DokumanUzantiList.Select(k => k.Key).ToList());
                    temp += "'";
                    _DokumanUzantiVerticalBarSperatorBuffer = temp;
                }
                return _DokumanUzantiVerticalBarSperatorBuffer;
            }
        }


        private static string _OzelliklerArrayBuffer = null;
        public static string OzelliklerArrayBuffer
        {
            get
            {
                if (_OzelliklerArrayBuffer == null)
                {
                    List<string> baslikList = new List<string>();
                    string jsBaslik = "[";
                    DataLayer.Buffers.Branslar.Select(b => "'" + b.Ad + "'").ToList().ForEach(b => baslikList.Add(b));
                    DataLayer.Buffers.EgitimYillari.Select(b => "'" + b.Ad + "'").ToList().ForEach(b => baslikList.Add(b));
                    DataLayer.Buffers.Kulupler.Select(b => "'" + b.Ad + "'").ToList().ForEach(b => baslikList.Add(b));
                    DataLayer.Buffers.DokumanTurleri.Select(b => "'" + b.Ad + "'").ToList().ForEach(b => baslikList.Add(b));
                    jsBaslik += string.Join(",",baslikList) + "]";

                    _OzelliklerArrayBuffer = jsBaslik;
                }

                return _OzelliklerArrayBuffer;
            }
        }
    }
}