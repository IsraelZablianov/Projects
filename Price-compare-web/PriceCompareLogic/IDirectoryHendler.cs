using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_compare_web.Api
{
    interface IDirectoryHendler
    {
        List<string> GetDirectories();
        FileInfo[] GetFileInfo(FileIdentifiers fileIdentifiers);
    }
}
