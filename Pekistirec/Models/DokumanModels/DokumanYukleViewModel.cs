using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.DokumanModels
{
    public class DokumanYukleViewModel
    {
        public HttpPostedFileWrapper Dosya { get; set; }

        [DisplayName("Başlık")]
        [Required]
        public string Baslik { get; set; }

        [DisplayName("Etiketler")]
        public string Etiketler { get; set; }
        
        [Required]
        public bool Ozel { get; set; }

        public string recaptcha_challenge_field { get; set; }

        public string recaptcha_response_field { get; set; }

        public bool reCaptchaGerekliMi = true;
    }
}