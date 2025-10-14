using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Constant
{
    public class XmlClassShutterConst : XmlBaseConst
    {
        public XmlClassShutterConst(XmlNode xmlNode) : base(xmlNode)
        {
        }

        public XmlClassFlapConst[] Flaps
        {
            get
            {
                var xmlConst = XmlNode.SelectNodes("dev[@name='flap']");

                var masConst = new XmlClassFlapConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassFlapConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        public XmlClassStatusConst StatusConst
        {
            get
            {
                var statusNode = XmlNode.SelectSingleNode("dev[@name='status']");
                return new XmlClassStatusConst(statusNode);
            }
        }

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeShutterScript);
    }
}
