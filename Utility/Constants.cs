using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TOC
{
    public class Constants
    {
        public const string CONNECTION_STRING = "Server=remotemysql.com;Database=xhKg6X3Og7;Uid=xhKg6X3Og7;Pwd=yz055y5WpE;Convert Zero Datetime=True";

        public const string MYFILES_FOLDER_PATH = "C:\\Myfiles\\";
        public const string SB_FILE_NAME = "StrategyBuilder.csv";
        public const string PT_FILE_NAME = "PositionsTracker.csv";
        public const string BO_FILE_NAME = "BasketOrder.csv";

        public const string ALL = "ALL";

        public static int BANKNIFTY_LOT_SIZE = 25;
        public static int NIFTY_LOT_SIZE = 75;

        public static int POS_MAX_ROWS = 50;
        public static int POS_ADD_ROW_COUNT = 1;

        public static int SB_MAX_ROWS = 50;
        public static int SB_ADD_ROW_COUNT = 1;

        public const int BO_MAX_ROWS = 10;
        public const int BO_ADD_ROW_COUNT = 1;

    }
}