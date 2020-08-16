using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TOC
{
    public class GlobalFilterConditions
    {
        public bool Hide50MulSP { get; set; } = true;
        public bool Hide1SDSP { get; set; } = true;
        public bool Hide2SDSP { get; set; } = true;

        public GlobalFilterConditions()
        {
            Hide50MulSP = true;
            Hide1SDSP = false;
            Hide2SDSP = false;
        }
    }
}