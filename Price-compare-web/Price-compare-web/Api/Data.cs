using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Price_compare_web.Api
{
    public class Data
    {
        public object[] storesToCompare { get; set; }

        public Product[] products { get; set; }
    }
}