using System.Linq;
using System.Xml;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Constant
{
    public class XmlClassPumpSysConst : XmlBaseConst
    {
        public XmlClassPumpSysConst(XmlNode xmlNode):base(xmlNode)
        {
        }
        
        /// <summary>
        /// Константы вакуумного насоса
        /// </summary>
        public XmlClassPumpConst VacuumPump
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='vacuumPump']");
                if (node != null)
                    return new XmlClassPumpConst(node);
                return null;
            }
        }

        /// <summary>
        /// Константы форвакуумного насоса
        /// </summary>
        public XmlClassPumpConst RoughingPump
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='roughingPump']");
                if (node != null)
                    return new XmlClassPumpConst(node);
                return null;
            }
        }

        /// <summary>
        /// Константы вакуметра
        /// </summary>
        public XmlClassVacuumetrConst VacuumetrConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='vacuumetr']");
                if (node != null)
                    return new XmlClassVacuumetrConst(node);
                return null;
            }
        }

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangePumpScript);
    }
}
