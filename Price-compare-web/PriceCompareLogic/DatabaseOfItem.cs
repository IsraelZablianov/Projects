using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_compare_web.Api
{
    [Serializable]
    public class DatabaseOfItem
    {
        public DatabaseOfItem()
        {
            ItemsAndPrices = new Dictionary<string, double>();
            ItemsAndQuantities = new Dictionary<string, double>();
            Items = new List<string>();
        }

        public Dictionary<string, double> ItemsAndPrices
        {
            get;
            set;
        }

        public Dictionary<string, double> ItemsAndQuantities
        {
            get;
            set;
        }

        public List<string> Items
        {
            get;
            set;
        }
    }
}
