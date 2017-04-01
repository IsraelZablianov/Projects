using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_compare_web.Api
{
    interface IXmlParse
    {
        List<string> GetListOfElementsFromXml(XmlElementId xmlElementId);
    }
}
