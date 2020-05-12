using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TOC
{
    public partial class DynamicMenu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DataTable dt = this.BindMenuData(0);
                DynamicMenuControlPopulation(dt, 0, null);
            }
        }

        /// <summary>  
        /// This function retur a datatable according to parent menu id passed by user  
        /// </summary>  
        /// <param name="parentmenuId">parent menu ID</param>  
        /// <returns>data table</returns>  
        protected DataTable BindMenuData(int parentmenuId)
        {
            //declaration of variable used  
            DataSet ds = new DataSet();
            DataTable dt;
            DataRow dr;
            DataColumn menu;
            DataColumn pMenu;
            DataColumn title;
            DataColumn description;
            DataColumn URL;

            //create an object of datatable  
            dt = new DataTable();

            //creating column of datatable with datatype  
            menu = new DataColumn("MenuId", Type.GetType("System.Int32"));
            pMenu = new DataColumn("ParentId", Type.GetType("System.Int32"));
            title = new DataColumn("Title", Type.GetType("System.String"));
            description = new DataColumn("Description", Type.GetType("System.String"));
            URL = new DataColumn("URL", Type.GetType("System.String"));

            //bind data table columns in datatable  
            dt.Columns.Add(menu);//1st column  
            dt.Columns.Add(pMenu);//2nd column  
            dt.Columns.Add(title);//3rd column  
            dt.Columns.Add(description);//4th column  
            dt.Columns.Add(URL);//5th column  

            //creating data row and assiging the value to columns of datatable  
            //1st row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 1;
            dr["ParentId"] = 0;
            dr["Title"] = "Home";
            dr["Description"] = "";
            dr["URL"] = "~/Home.aspx";
            dt.Rows.Add(dr);

            //2nd row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 2;
            dr["ParentId"] = 0;
            dr["Title"] = "Option Chain";
            dr["Description"] = "Option Chain";
            dr["URL"] = "~/OptionChain.aspx";
            dt.Rows.Add(dr);

            //3rd row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 3;
            dr["ParentId"] = 0;
            dr["Title"] = "Trading Calls";
            dr["Description"] = "Trading Calls";
            dr["URL"] = "~/ReadFiles.aspx";
            dt.Rows.Add(dr);

            //4th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 4;
            dr["ParentId"] = 0;
            dr["Title"] = "Contact Us";
            dr["Description"] = "Contact Us page";
            dr["URL"] = "~/Contact.aspx";
            dt.Rows.Add(dr);

            //5th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 5;
            dr["ParentId"] = 0;
            dr["Title"] = "Testmonial";
            dr["Description"] = "Testimonial page";
            dr["URL"] = "~/Testimonial.aspx";
            dt.Rows.Add(dr);

            //6th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 6;
            dr["ParentId"] = 2;
            dr["Title"] = "Consulting";
            dr["Description"] = "Consulting page";
            dr["URL"] = "~/Consult.aspx";
            dt.Rows.Add(dr);

            //7th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 7;
            dr["ParentId"] = 2;
            dr["Title"] = "Outsourcing";
            dr["Description"] = "Outsourcing page";
            dr["URL"] = "~/Outsource.aspx";
            dt.Rows.Add(dr);

            //8th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 8;
            dr["ParentId"] = 7;
            dr["Title"] = "Domestic";
            dr["Description"] = "Domestic outsourcing page";
            dr["URL"] = "~/Domestic.aspx";
            dt.Rows.Add(dr);

            //9th row of data table  
            dr = dt.NewRow();
            dr["MenuId"] = 9;
            dr["ParentId"] = 7;
            dr["Title"] = "International";
            dr["Description"] = "International outsourcing page";
            dr["URL"] = "~/International.aspx";
            dt.Rows.Add(dr);

            ds.Tables.Add(dt);
            var dv = ds.Tables[0].DefaultView;
            dv.RowFilter = "ParentId='" + parentmenuId + "'";
            DataSet ds1 = new DataSet();
            var newdt = dv.ToTable();
            return newdt;
        }

        /// <summary>  
        /// This is a recursive function to fetchout the data to create a menu from data table  
        /// </summary>  
        /// <param name="dt">datatable</param>  
        /// <param name="parentMenuId">parent menu Id of integer type</param>  
        /// <param name="parentMenuItem"> Menu Item control</param>  
        protected void DynamicMenuControlPopulation(DataTable dt, int parentMenuId, MenuItem parentMenuItem)
        {
            string currentPage = Path.GetFileName(Request.Url.AbsolutePath);
            foreach (DataRow row in dt.Rows)
            {
                MenuItem menuItem = new MenuItem
                {
                    Value = row["MenuId"].ToString(),
                    Text = row["Title"].ToString(),
                    NavigateUrl = row["URL"].ToString(),
                    Selected = row["URL"].ToString().EndsWith(currentPage, StringComparison.CurrentCultureIgnoreCase)
                };
                if (parentMenuId == 0)
                {
                    Menu1.Items.Add(menuItem);
                    DataTable dtChild = this.BindMenuData(int.Parse(menuItem.Value));
                    DynamicMenuControlPopulation(dtChild, int.Parse(menuItem.Value), menuItem);
                }
                else
                {

                    parentMenuItem.ChildItems.Add(menuItem);
                    DataTable dtChild = this.BindMenuData(int.Parse(menuItem.Value));
                    DynamicMenuControlPopulation(dtChild, int.Parse(menuItem.Value), menuItem);

                }
            }
        }
    }
}