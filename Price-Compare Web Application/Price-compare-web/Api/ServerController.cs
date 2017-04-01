using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Price_compare_web.Api
{
    [RoutePrefix("api/serverPriceCompare")]
    public class ServerController : ApiController
    {
        private static StoreFileManager _storeFileManager = new StoreFileManager();

        public ServerController()
        {
            _storeFileManager.Path = System.Web.HttpContext.Current.Server.MapPath("~/bin");
        }

        [Route("getChainNames")]
        [HttpGet]
        public object[] GetChainNames()
        {
            return _storeFileManager.GetDirectories().ToArray<object>();
        }


        [Route("getStoreNames")]
        [HttpGet]
        public object[] GetStoreNames(string info)
        {
            string[] getData = info.Split(',');
            var fileIdentifiers = new FileIdentifiers()
            {
                DirName = getData[0],
                PartialFileName = getData[1]
            };
            string optionalAeraFilter = getData[2];

            return _storeFileManager.GetStoresNames(fileIdentifiers, optionalAeraFilter).ToArray<object>();
        }

        [Route("getProductItems")]
        [HttpGet]
        public async Task<object[]> GetProductItems(string info)
        {
            string[] getData = info.Split(',');
            var fileIdentifiers = new FileIdentifiers()
            {
                DirName = getData[0],
                PartialFileName = getData[1]
            };

            List<string> items = new List<string>();
            try
            {
                items = await _storeFileManager.GetItemsOfStore(fileIdentifiers);

            }
            catch (Exception ex) { }// log here}

            return items.ToArray<object>();
        }

        [Route("getReport")]
        [HttpPost]
        public object[] GetReport(Data data)
        {
            var databaseOfShoppingCart = new DatabaseOfItem();
            UpdtaeShopingCart(data.products, databaseOfShoppingCart);
            List<FileIdentifiers> stores = new List<FileIdentifiers>();
            foreach (string chainAndBranchName in data.storesToCompare)
            {
                string[] chainAndBranch = chainAndBranchName.Split(',');
                var store = new FileIdentifiers()
                {
                    DirName = chainAndBranch[0],
                    PartialFileName = chainAndBranch[1]
                };
                stores.Add(store);
            }

            var fullReportOfStores = _storeFileManager.FullReportOfStores(stores, databaseOfShoppingCart);
            List<Report> reporetOfStores = new List<Report>();

            foreach (var item in fullReportOfStores)
            {
                var report = new Report();
                report.StoreName = item.Key;
                report.StoreReport = item.Value;
                reporetOfStores.Add(report);
            }

            return reporetOfStores.ToArray<object>();
        }

        private void UpdtaeShopingCart(Product[] products, DatabaseOfItem databaseOfShoppingCart)
        {
            foreach (var item in products)
            {
                databaseOfShoppingCart.ItemsAndQuantities.Add(item.Name, item.Quantity);
                databaseOfShoppingCart.Items.Add(item.Name);
            }
        }
    }
}
