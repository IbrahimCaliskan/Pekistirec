using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Models.AccountModels
{
    public static class ACCOUNT_VIEWMODELS_CONSTS
    {
        public const string UserName_StringLength_ErrorMessage = "{0} en az {2} en fazla {1} karakter olmalıdır.";
        public const int UserName_StringLength_MinimumLength = Pekistirec.Engine.AspNetIdentity.IdentitConfig.UserNameMinLength;
        public const int UserName_StringLength_MaximumLength = Pekistirec.Engine.AspNetIdentity.IdentitConfig.UserNameMaxLength;
        public const string UserName_RegularExpression_Regex = Pekistirec.Engine.AspNetIdentity.IdentitConfig.UserNameRegex;
        public const string UserName_RegularExpression_ErrorMessage = "Kullanıcı adı boşluk ile başlayamaz veya bitemez.";
        public const string UserName_Display_Name = "Kullanıcı Adı";
    }
}