using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceCompareLogic
{
    interface IDirectoryHendler
    {
        List<string> GetDirectories();
        FileInfo[] GetFileInfo(FileIdentifiers fileIdentifiers);
    }
}
