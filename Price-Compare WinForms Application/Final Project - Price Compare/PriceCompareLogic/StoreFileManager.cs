using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PriceCompareLogic
{
    public class StoreFileManager : IReportable, IDirectoryHendler, IXmlParse
    {
        public string FullReportOfStore(DatabaseOfItem databaseOfItem)
        {
            var report = new StringBuilder();
            double sum = 0;

            foreach (var item in databaseOfItem.ItemsAndPrices)
            {
                var amount = (item.Value * databaseOfItem.ItemsAndQuantities[item.Key])
                    != 0 ? (item.Value * databaseOfItem.ItemsAndQuantities[item.Key]).ToString()
                    : "Not Exists";
                report.AppendFormat(@"{0} = {1} x{2}{3}",
                    amount, item.Key,
                    databaseOfItem.ItemsAndQuantities[item.Key], 
                    Environment.NewLine);
                sum += item.Value * databaseOfItem.ItemsAndQuantities[item.Key];
            }

            report.Insert(0, $"Total = {sum}{Environment.NewLine}{Environment.NewLine}");
            return report.ToString();
        }

        public Dictionary<string, string> FullReportOfStores(List<FileIdentifiers> stores, DatabaseOfItem databaseOfShoppingCart)
        {
            var report = new Dictionary<string, string>();
            foreach (var store in stores)
            {
                var itemsAndPrices = GetItemsPrice(store, databaseOfShoppingCart.Items)
                .OrderBy(item => item.Value).ToDictionary(pair => pair.Key, pair => pair.Value);

                databaseOfShoppingCart.ItemsAndPrices = itemsAndPrices;
                report.Add($"{store.DirName}-{store.PartialFileName}", FullReportOfStore(databaseOfShoppingCart));
            }

            return report;
        }

        public FileInfo[] GetFileInfo(FileIdentifiers fileIdentifiers)
        {
            var directoryInWhichToSearch = new DirectoryInfo(fileIdentifiers.DirName);
            FileInfo[] filesInDir = directoryInWhichToSearch.GetFiles($"*{fileIdentifiers.PartialFileName}*.*", SearchOption.AllDirectories);

            return filesInDir;
        }

        public List<string> GetDirectories()
        {
            List<string> chainNames = new List<string>();
            var fileIdentifiers = new FileIdentifiers(){
                DirName = Directory.GetCurrentDirectory(),
                PartialFileName = "Stores"};
            FileInfo[] filesInDir = GetFileInfo(fileIdentifiers);
            foreach (FileInfo foundFile in filesInDir)
            {
                chainNames.Add(foundFile.Directory.Name);
            }

            return chainNames;
        }

        public List<string> GetStoresNames(FileIdentifiers fileIdentifiers, string optionalAreaName)
        {
            FileInfo[] filesInDir = GetFileInfo(fileIdentifiers);
            var xmlElementId = new XmlElementId()
            {
                DescendantFrom = "Store",
                ElementName = "StoreName",
                XmlFullPath = Path.Combine(fileIdentifiers.DirName, filesInDir[0].Name)
            };

            if (!string.IsNullOrEmpty(optionalAreaName))
            {
                xmlElementId.ElementCondition = "City";
                xmlElementId.ElementConditionExpected = optionalAreaName;
            }

            var storeNames = GetListOfElementsFromXml(xmlElementId);
            return storeNames;
        }

        public List<string> GetStoresNames(FileIdentifiers fileIdentifiers, XmlElementId xmlElementId)
        {
            FileInfo[] filesInDir = GetFileInfo(fileIdentifiers);
            xmlElementId.XmlFullPath = Path.Combine(fileIdentifiers.DirName, filesInDir[0].Name);
            var storeNames = GetListOfElementsFromXml(xmlElementId);
            return storeNames;
        }

        public List<string> GetListOfElementsFromXml(XmlElementId xmlElementId)
        {
            var listOfElements = new List<string>();

            try
            {
                var XElementDoc = XElement.Load(xmlElementId.XmlFullPath);
                listOfElements = (from element
                  in XElementDoc.Descendants()
                  .Where(el => string.Compare(el.Name.LocalName, xmlElementId.DescendantFrom,
                   StringComparison.OrdinalIgnoreCase) == 0)
                                  where (string.IsNullOrEmpty(xmlElementId.ElementCondition)
                                  || ((string)element.Element(xmlElementId.ElementCondition)).Trim() == xmlElementId.ElementConditionExpected)
                                  select (string)element
                                 .Element(xmlElementId.ElementName)).ToList();
            }
            catch (Exception)
            {
                File.WriteAllText(@"LogFiles\StoreItems.txt", $"{xmlElementId.ElementName} not found");
            }

            return listOfElements;
        }

        public async Task<List<string>> GetItemsOfStore(FileIdentifiers fileIdentifiers)
        {
            var items = new List<string>();
            var xmlElementId = new XmlElementId();
            xmlElementId.XmlFullPath = GetStoreFullPath(fileIdentifiers);
            xmlElementId.DescendantFrom = "Item";
            xmlElementId.ElementName = "ItemName";

            await Task.Run(() =>
            {
                var listOfItems = GetListOfElementsFromXml(xmlElementId);
                foreach (var item in listOfItems)
                {
                    if (!items.Contains(item))
                    {
                        items.Add(item);
                    }
                }
            });

            return items;
        }

        public string GetStoreFullPath(FileIdentifiers fileIdentifiers)
        {
            var storeFileIdentifier = new FileIdentifiers();
            storeFileIdentifier.DirName = fileIdentifiers.DirName;
            storeFileIdentifier.PartialFileName = "Stores";
            FileInfo[] fileInfo = GetFileInfo(storeFileIdentifier);
            var XElementDoc = XElement.Load(fileInfo[0].FullName);
            string fileFullPath = string.Empty;
            var storeId = (from element in (XElementDoc.Descendants()
                           .Where(el => string.Compare(el.Name.LocalName, "Store",
                           StringComparison.OrdinalIgnoreCase) == 0))
                           where (string)element.Element("StoreName") == fileIdentifiers.PartialFileName
                           select (string)element.Element("StoreId"))
                           .ToList().First();
            var storeIdInFormOf3Digits = string.Format("{0:000}", int.Parse(storeId));
            storeFileIdentifier.PartialFileName = $"Price*Full*-{storeIdInFormOf3Digits}-";
            FileInfo[] files = GetFileInfo(storeFileIdentifier);
            if (files.Any())
            {
                fileFullPath = files[0].FullName;
            }

            return fileFullPath;
        }

        public Dictionary<string, double> GetItemsPrice(FileIdentifiers fileIdentifiers, List<string> items)
        {
            var itemsNameAndPrice = new Dictionary<string, double>();
            double price = 0;
            var uri = GetStoreFullPath(fileIdentifiers);

            if (uri != string.Empty)
            {
                var XElementDoc = XElement.Load(uri);
                foreach (var itemName in items)
                {
                    price = (from element in XElementDoc.Descendants()
                                 .Where(el => string.Compare(el.Name.LocalName, "Item",
                                  StringComparison.OrdinalIgnoreCase) == 0)
                             where (string)element.Element("ItemName") == itemName
                             select (double)element.Element("ItemPrice")).FirstOrDefault();
                    itemsNameAndPrice.Add(itemName, price);
                }
            }

            return itemsNameAndPrice;
        }

        public void SaveFile(string filePath, DatabaseOfItem databaseOfItem)
        {
            using (Stream stream = File.Open(filePath, FileMode.OpenOrCreate))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                binaryFormatter.Serialize(stream, databaseOfItem);
            }
        }

        public DatabaseOfItem LoadFile(string filePath)
        {
            using (Stream stream = File.OpenRead(filePath))
            {
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                DatabaseOfItem databaseOfItem = null;
                try
                {
                    databaseOfItem = (DatabaseOfItem)binaryFormatter.Deserialize(stream);
                }
                catch (Exception) { }

                return databaseOfItem;
            }
        }
    }
}
