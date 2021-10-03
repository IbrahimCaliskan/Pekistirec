using System.ComponentModel.DataAnnotations;

using c = Pekistirec.Models.AccountModels.ACCOUNT_VIEWMODELS_CONSTS;

namespace Pekistirec.Models.AccountModels
{
    public class RegisterRequestViewModel
    {
        [Required]
        [StringLength
            (c.UserName_StringLength_MaximumLength,
            ErrorMessage = c.UserName_StringLength_ErrorMessage,
            MinimumLength = c.UserName_StringLength_MinimumLength)]
        [RegularExpression
            (c.UserName_RegularExpression_Regex,
            ErrorMessage = c.UserName_RegularExpression_ErrorMessage)]
        [Display
            (Name = c.UserName_Display_Name)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter olmalıdır.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Onay")]
        [Compare("Password", ErrorMessage = "Şifre ve Şifre Onay eşleşmedi.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Sözleşmesi")]
        public bool SozlesmeKabul { get; set; }

        public string recaptcha_challenge_field { get; set; }

        public string recaptcha_response_field { get; set; }        

    }

    public class RegisterResponseViewModel
    {
        [Required]
        [StringLength
            (c.UserName_StringLength_MaximumLength,
            ErrorMessage = c.UserName_StringLength_ErrorMessage,
            MinimumLength = c.UserName_StringLength_MinimumLength)]
        [RegularExpression
            (c.UserName_RegularExpression_Regex,
            ErrorMessage = c.UserName_RegularExpression_ErrorMessage)]
        [Display
            (Name = c.UserName_Display_Name)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} en az {2} karakter olmalıdır.", MinimumLength = 1)]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre Onay")]
        [Compare("Password", ErrorMessage = "Şifre ve Şifre Onay eşleşmedi.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Kullanıcı Sözleşmesi")]
        public bool SozlesmeKabul { get; set; }

        public string recaptcha_challenge_field { get; set; }

        public string recaptcha_response_field { get; set; }

        public bool reCaptchaGerekliMi = true;
    }
}