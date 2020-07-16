using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace TOC
{
    public partial class Settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GlobalFilterConditions globalFilter = new GlobalFilterConditions();
            globalFilter.Hide50MulSP = true;
            globalFilter.Hide1SDSP = true;
            globalFilter.Hide2SDSP = true;

            MySession.Current.GlobalFilterConditions = globalFilter;
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {

        }
    }
}