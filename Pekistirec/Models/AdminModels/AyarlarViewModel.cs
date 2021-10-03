using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.AdminModels
{
    public class AyarlarViewModel
    {
        public bool EVENTLOG_ENABLED { get; set; }
        
        public int EVENTLOG_BUFFER_SIZE { get; set; }
        public bool RECAPTCHA_ENABLED { get; set; }

        public int RECAPTCHA_KULLANICI_KAYIT_ESNASINDA_GORUNMESI_ICIN_MAX_KULLANICI_KAYIT_SAYISI { get; set; }

        public int RECAPTCHA_KULLANICI_GIRIS_ESNASINDA_GORUNMESI_ICIN_MAX_HATALI_GIRIS_DENEMESI { get; set; }

        public int RECAPTCHA_DOKUMAN_YUKLE_GORUNMESI_ICIN_MAX_YUKLEME_SAYISI { get; set; }

        public int RECAPTCHA_DOKUMAN_GOSTER_GORUNMESI_ICIN_MAX_GOSTERME_SAYISI { get; set; }
    }
}