using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GasStation.xml.Constant.XmlConst;

namespace GasStation.xml.Constant
{
    public class XmlClassImpulsConst : XmlBaseConst
    {
        public XmlClassImpulsConst(XmlNode xmlNode):base(xmlNode)
        {

        }

        /// <summary>
        /// Длительность импульса
        /// </summary>
        public XmlClassDOConst ImpulsDuration
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='impDuration']");
                if (node == null)
                    return null;
                return new XmlClassDOConst(node);
            }
        }

        /// <summary>
        /// Задержка импульса
        /// </summary>
        public XmlClassDOConst ImpulsDelay
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='impDelay']");
                if (node == null)
                    return null;
                return new XmlClassDOConst(node);
            }
        }

    }
}
