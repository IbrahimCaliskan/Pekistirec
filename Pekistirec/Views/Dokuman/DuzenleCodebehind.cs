using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pekistirec.Views.Dokuman
{
    public class DuzenleCodebehind
    {
        public string FormSilActionName;
        public string FormSilControllerName;
        public string CheckboxSilName;
        public string DokumanName;
        public string DuzenleLink;
        public string DirekLink;
        public string PaylasimLinki;

        public DuzenleCodebehind(Pekistirec.Models.DokumanModels.DokumanDuzenleViewModel model)
        {
            var formSilNames = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetControllerAndActionName<Pekistirec.Controllers.DokumanController>(c => c.Duzenle("", ""));
            FormSilActionName = formSilNames.ActionName;
            FormSilControllerName = formSilNames.ControllerName;
            CheckboxSilName = Pekistirec.Engine.Toolbox.GenelToolbox.GenelTools.GetCorrectPropertyName<Pekistirec.Models.DokumanModels.DokumanDuzenleViewModel>(ddvm => ddvm.Sil);

            var dokuman = model.Dokuman;
            DokumanName = Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools.DokumanAdiOlustur(dokuman);

            PaylasimLinki = Pekistirec.Engine.Toolbox.DokumanToolbox.DokumanTools.DokumanPaylasimLinkiOlustur("", dokuman.Id.ToString());
        }
    }
}