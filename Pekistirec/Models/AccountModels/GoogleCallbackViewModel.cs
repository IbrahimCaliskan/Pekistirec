using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using c = Pekistirec.Models.AccountModels.ACCOUNT_VIEWMODELS_CONSTS;

namespace Pekistirec.Models.AccountModels
{
    public class GoogleCallbackRequestViewModel
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
    }

    public class GoogleCallbackResponseViewModel
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

        public GoogleAPI.GoogleAPICarrier GoogleUser { get; set; }
        public string UserIp { get; set; }
    }
}