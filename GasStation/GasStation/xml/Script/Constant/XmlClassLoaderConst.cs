using System.Linq;
using System.Xml;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Constant
{
    public class XmlClassLoaderConst:XmlBaseConst
    {
        public XmlClassLoaderConst(XmlNode xmlNode) : base(xmlNode)
        {
        }

        /// <summary>
        /// Список настроек для АЦП
        /// </summary>
        public XmlClassAcpConst[] MasAcpConst
        {
            get
            {
                var nodes = XmlNode.SelectNodes("dev[@name='acp']");
                if (nodes.Count == 0)
                    return null;

                var masAcpConst = new XmlClassAcpConst[nodes.Count];

                for (var idx = 0; idx < nodes.Count; idx++)
                    masAcpConst[idx] = new XmlClassAcpConst(nodes[idx]);

                return masAcpConst;
            }
        }

        /// <summary>
        /// Список настроек для ЦАП
        /// </summary>
        public XmlClassCapConst[] MasCapConst
        {
            get
            {

                var nodes = XmlNode.SelectNodes("dev[@name='cap']");
                if (nodes.Count == 0)
                    return null;

                var masCapConst = new XmlClassCapConst[nodes.Count];

                for (var idx = 0; idx < nodes.Count; idx++)
                    masCapConst[idx] = new XmlClassCapConst(nodes[idx]);

                return masCapConst;
            }
        }

        //public XmlClassDioPortConst[] MasDioConst
        //{
        //    get
        //    {
        //        var nodes = XmlNode.SelectNodes("dev[@name='dio']");
        //        if (nodes.Count == 0)
        //            return null;

        //        var masDioConst = new XmlClassDioPortConst[nodes.Count];

        //        for (var idx = 0; idx < nodes.Count; idx++)
        //        {
        //            masDioConst[idx] = new XmlClassDioPortConst(nodes[idx]);
        //        }

        //        return masDioConst;
        //    }
        //}

        /// <summary>
        /// Загрузка
        /// </summary>
        public XmlClassDioPortConst Load
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='loadingPlatform']");
                return node == null ? null : new XmlClassDioPortConst(node);
            }
        }
        /// <summary>
        /// Выгрузка
        /// </summary>
        public XmlClassDioPortConst UnLoad
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='unloadingPlatform']");
                return node == null ? null : new XmlClassDioPortConst(node);
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

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeLoaderScript);
    }


}
