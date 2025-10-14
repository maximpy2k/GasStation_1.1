using System;
using System.Linq;
using System.Xml;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassFlapConst : XmlBaseConst
    {
        public XmlClassFlapConst(XmlNode xmlNode) : base(xmlNode)
        {
        }

        /// <summary>
        /// Список настроек для DIO
        /// </summary>
        public XmlClassDioPortConst[] MasDioConst
        {
            get
            {
                var nodes = XmlNode.SelectNodes("dev[@name = 'dio']");
                if (nodes.Count == 0)
                    return null;

                var masDioConst = new XmlClassDioPortConst[nodes.Count];

                for (var idx = 0; idx < nodes.Count; idx++)
                    masDioConst[idx] = new XmlClassDioPortConst(nodes[idx]);

                return masDioConst;
            }
        }
        public bool FlapNormalState
        {
            get
            {
                if (XmlNode == null)
                    return true;
                var node = XmlNode.SelectSingleNode("dev/EditFild[@name = 'normalState']");
                if (node == null)
                    return true;

                var str = node.Attributes["value"].Value;

                var flapState = false;

                if (str == "Close")
                {
                    flapState = false;
                }

                if (str == "Open")
                {
                    flapState = true;
                }

                return flapState;
            }
        }

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeFlapScript);
    }
}

