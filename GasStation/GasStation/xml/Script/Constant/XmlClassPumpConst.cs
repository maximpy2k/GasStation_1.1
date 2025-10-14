using System;
using System.Linq;
using System.Xml;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Script.Constant;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassPumpConst : XmlBaseConst
    {
        public XmlClassPumpConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        /// <summary>
        /// Настройди ДИО,  управляющего насосом
        /// </summary>
        public XmlClassDioPortConst PumpDioConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name = 'dio']");
                var pumpDioConst = new XmlClassDioPortConst(node);
                return pumpDioConst;
            }
        }

        public XmlClassStatusConst StatusConst
        {
            get
            {

                var statusNode = XmlNode.SelectSingleNode("dev[@name='status']");
                if (statusNode == null)
                    return null;
                return new XmlClassStatusConst(statusNode);
            }
        }


        /// <summary>
        /// Минимальное давление при котором возможно включение насоса
        /// </summary>
        public double MinPressure
        {
            get
            {
                double currPress= -1;
                if (XmlNode == null)
                    return currPress;

                var node = XmlNode.SelectSingleNode("dev[@name='properties']/EditFild[@name = 'minPressure']");
                if (node == null)
                     return currPress;

                var str = node.Attributes["value"].Value;

                double.TryParse(str, out currPress);
                
                return currPress;
            }
        }

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeFlapScript);
    }
}

