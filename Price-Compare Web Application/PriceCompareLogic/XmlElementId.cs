using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Price_compare_web.Api
{
    public class XmlElementId
    {
        public XmlElementId()
        {
            ElementCondition = ElementConditionExpected = string.Empty;
        }

        public string XmlFullPath
        {
            get;
            set;
        }

        public string DescendantFrom
        {
            get;
            set;
        }

        public string ElementName
        {
            get;
            set;
        }

        public string ElementCondition
        {
            get;
            set;
        }

        public string ElementConditionExpected
        {
            get;
            set;
        }
    }
}
