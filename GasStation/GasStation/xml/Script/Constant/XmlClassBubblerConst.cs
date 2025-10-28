using GasStation.xml.Constant;
using System;
using System.Linq;
using System.Xml;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassBubblerConst : XmlBaseConst
    {
        public XmlClassBubblerConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        /// <summary>
        /// Константы термодатчика
        /// </summary>
        public XmlTdConst Td
        {
            get
            {
                var xmlNode = XmlNode.SelectSingleNode("dev[@name='td']");
                if (xmlNode == null)
                    return null;
                var td = new XmlTdConst(xmlNode);                                
                return td;
            }
        }
        /// <summary>
        /// Наличие термодатчика в константах
        /// </summary>
        public string EnableTd => Td == null ? "Collapsed" : "Visible";

        /// <summary>
        /// Наличие Порта включения в константах
        /// </summary>
        public string EnableSw => MasDioConst == null ? "Collapsed" : "Visible";

        /// <summary>
        /// Список настроек для ЦАП
        /// </summary>
        public XmlClassCapConst[] MasDioConst
        {
            get
            {

                var nodes = XmlNode.SelectNodes("dev[@name='dio']");
                if (nodes.Count == 0)
                    return null;

                var masCapConst = new XmlClassCapConst[nodes.Count];

                for (var idx = 0; idx < nodes.Count; idx++)
                    masCapConst[idx] = new XmlClassCapConst(nodes[idx]);

                return masCapConst;
            }
        }

        /// <summary>
        /// Пускатель
        /// </summary>
        public XmlClassDioPortConst DioRealyConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='dio']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassDioPortConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Константы для ПИД-регулятора
        /// </summary>
        public XmlClassPidConst Pid
        {
            get
            {
                var xmlNode = XmlNode.SelectSingleNode("dev[@name='pid']");
                if (xmlNode == null)
                    return null;
                var pid = new XmlClassPidConst(xmlNode);

                return pid;
            }
        }

        /// <summary>
        /// Наличие ПИД регулятора
        /// </summary>
        public string EnablePid => Pid == null ? "Collapsed" : "Visible";

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeBubblerScript);
    }
}
