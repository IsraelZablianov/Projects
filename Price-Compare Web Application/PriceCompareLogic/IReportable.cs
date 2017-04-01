using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_compare_web.Api
{
    interface IReportable
    {
        string FullReportOfStore(DatabaseOfItem databaseOfItem);
        Dictionary<string, string> FullReportOfStores(List<FileIdentifiers> stores, DatabaseOfItem databaseOfShoppingCart);
    }
}
