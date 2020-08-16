//https://stackoverflow.com/questions/621549/how-to-access-session-variables-from-any-class-in-asp-net

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace TOC
{
    public class MySession
    {   
        // private constructor
        private MySession()
        {
            RecordsObject = null;
            StrategyBuilderDt = null;
            RecordsNifty = null;
            RecordsBankNifty = null;
            PositionsTrackerDt = null;
            GlobalFilterConditions = null;
            BasketOrderDt = null;
        }

        // Gets the current session.
        public static MySession Current
        {
            get
            {
                MySession session =
                  (MySession)HttpContext.Current.Session["__MySession__"];
                if (session == null)
                {
                    session = new MySession();
                    HttpContext.Current.Session["__MySession__"] = session;
                }
                return session;
            }
        }

        // **** add your session properties here, e.g like this:
        public Records RecordsObject { get; set; }
        public Records RecordsNifty { get; set; }
        public Records RecordsBankNifty { get; set; }
        public DataTable StrategyBuilderDt { get; set; }
        public DataTable PositionsTrackerDt { get; set; }
        public GlobalFilterConditions GlobalFilterConditions { get; set; }
        public DataTable BasketOrderDt { get; set; }
    }
}