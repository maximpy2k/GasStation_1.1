using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GasStation.xml.Constant.XmlConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Constant
{
    public class XmlClassFreqGeneratorConst : XmlBaseConst
    {
        public XmlClassFreqGeneratorConst(XmlNode xmlNode) : base(xmlNode)
        {
        }

        /// <summary>
        /// Порт включения ШИМ генератора
        /// </summary>
        public XmlClassDioPortConst StartupSwitch
        {
            get
            {

                var node = XmlNode.SelectSingleNode("dev[@name='startupSwitch']");
                if (node == null)
                    return null;
                return new XmlClassDioPortConst(node);
            }
        }
        
        public XmlClassImpulsConst[] ImpulsConsts
        {
            get
            {
                var nodes = XmlNode.SelectNodes("dev[@name='impConst']");
                if (nodes == null)
                    return null;

                var shimImpulsConst = new XmlClassImpulsConst[nodes.Count];
                for (var idx = 0; idx < shimImpulsConst.Length; idx++)
                    shimImpulsConst[idx] = new XmlClassImpulsConst(nodes[idx]);

                return shimImpulsConst;
            }
        }


        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeFreqGenScript);
    }
}
