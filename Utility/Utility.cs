using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TOC
{
    public class Utility
    {
        public static void SelectDataInCombo(System.Web.UI.WebControls.DropDownList control, string value)
        {
            System.Web.UI.WebControls.ListItem lItem;

            lItem = control.Items.FindByValue(value);
            if (lItem != null)
            {
                if (control.SelectedItem != null) control
                    .SelectedItem.Selected = false;
                lItem.Selected = true;
                control.SelectedItem.Selected = true;
            }
        }
    }
}