using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst;
using System;
using System.Linq;
using System.Xml;

namespace GasStation.xml.Const.Elements
{
    public class XmlTdConst:XmlBaseConst
    {
        public XmlTdConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        public string TdType
        {
            get
            {
                var xmlConstFlaps = XmlNode.SelectSingleNode("dev/EditFild[@name='Type']");
                return Convert.ToString(xmlConstFlaps.Attributes["value"].Value);
            }
        }

        public double Aver
        {
            get
            {
                var xmlConstFlaps = XmlNode.SelectSingleNode("dev/EditFild[@name='Aver']");
                return Convert.ToDouble(xmlConstFlaps.Attributes["value"].Value);
            }
        }
        /// <summary>
        /// Ошибка температуры
        /// </summary>
        public double ErrorDT
        {
            get
            {
                var xmlConstFlaps = XmlNode.SelectSingleNode("dev/EditFild[@name='ErrorDT']");
                return Convert.ToDouble(xmlConstFlaps.Attributes["value"].Value);
            }
        }

        public int MaxErrorCnt
        {
            get
            {
                var xmlConstFlaps = XmlNode.SelectSingleNode("dev/EditFild[@name='MaxErrorCnt']");
                return Convert.ToInt32(xmlConstFlaps.Attributes["value"].Value);
            }
        }

        public string TablePath
        {
            get
            {
                var xmlConstFlaps = XmlNode.SelectSingleNode("dev/EditFild[@name='Path']");
                return Convert.ToString(xmlConstFlaps.Attributes["value"].Value);
            }
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


        public XmlTableConst CorrectionTable
        {
            get
            {
                var node = XmlNode.SelectSingleNode("table[@name='CorrectionTemp']");
                if (node == null)
                    return null;
                return new XmlTableConst(node);
            }
        }
    }
}
