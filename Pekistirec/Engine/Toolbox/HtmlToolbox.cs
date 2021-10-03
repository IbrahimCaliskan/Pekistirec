using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pekistirec.Engine.Toolbox.HtmlToolbox
{
    public class HtmlTools
    {
        public static List<SelectListItem> HtmlDropDownItemList(Dictionary<string, string> List, string SelectedValue = null)
        {
            List<SelectListItem> collection = new List<SelectListItem>();
            foreach (var item in List)
            {
                SelectListItem i = new SelectListItem();
                i.Text = item.Key;
                i.Value = item.Value;
                if (SelectedValue == null)
                    i.Selected = false;
                else
                    i.Selected = (item.Value == SelectedValue) ? true : false;
                collection.Add(i);
            }
            return collection;
        }
    }
}