using GasStation.xml.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GasStation.xml.Script.Constant
{
    public class XmlClassSensorConst: XmlClassDioPortConst
    {
        public XmlClassSensorConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        /// <summary>
        /// Инвертирование показаний
        /// </summary>
        public bool IsInverted
        {
            get
            {
                var val =false;
                if(XmlNode.SelectSingleNode("EditFild[@name='isInverted']")!=null)
                    bool.TryParse(XmlNode.SelectSingleNode("EditFild[@name='isInverted']").Attributes["value"].Value, out val);
                return val;
            }
        }
    }
}
