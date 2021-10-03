using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using c = Pekistirec.Models.AccountModels.ACCOUNT_VIEWMODELS_CONSTS;

namespace Pekistirec.Models.AccountModels
{
    public class FbCallbackRequestViewModel
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

    public class FbCallbackResponseViewModel
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

        public Pekistirec.Engine.FacebookAPI.FacebookUser FbUser { get; set; }
        public string UserIp { get; set; }
    }     
}