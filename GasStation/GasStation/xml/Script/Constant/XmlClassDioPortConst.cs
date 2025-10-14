using System;
using System.Xml;

namespace GasStation.xml.Constant
{
    public class XmlClassDioPortConst:XmlBaseConst
    {
        public XmlClassDioPortConst(XmlNode xmlNode):base(xmlNode)
        {            
        }
        public int ContrNum
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='contrNum']").Attributes["value"].Value, out val);
                return val;
            }
        }


        /// <summary>
        /// порт
        /// </summary>
        public int Port
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='port']").Attributes["value"].Value, out val);
                return val;
            }
        }
    }
}
