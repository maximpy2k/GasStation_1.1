using System;
using System.Linq;
using System.Xml;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassThermoSectionConst:XmlBaseConst 
    {
        public XmlClassThermoSectionConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        public int ContrNum
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev / EditFild[@name = 'contrNum']");
                return Convert.ToInt32(xmlConst.Attributes["value"].Value);
            }
        }

        /// <summary>
        /// Список настроек для управление мощьностью через ЦАП
        /// </summary>
        public XmlClassCapConst CapConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev/dev[@name='cap']");
                if (node==null)
                    return null;
                return new XmlClassCapConst(node);
            }
        }


        public int DataBits
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev / EditFild[@name = 'dataBits']");
                return Convert.ToInt32(xmlConst.Attributes["value"].Value);
            }
        }

        public int MaxValue
        {
            get { return Convert.ToInt32(Math.Pow(2, DataBits)-1); }
        }
        public double MinKey
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev / EditFild[@name = 'minKey']");
                return Convert.ToDouble(xmlConst.Attributes["value"].Value);
            }
        }
        public double MaxKey
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev / EditFild[@name = 'maxKey']");
                return Convert.ToDouble(xmlConst.Attributes["value"].Value);
            }
        }
        public double MaxSpeedUp
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev[@name='properties'] / EditFild[@name = 'maxUpSpeed']");
                return Convert.ToDouble(xmlConst.Attributes["value"].Value);
            }
        }
        public double MaxSpeedDown
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev[@name='properties'] / EditFild[@name = 'maxDownSpeed']");
                return Convert.ToDouble(xmlConst.Attributes["value"].Value);
            }
        }
       
        public XmlTableConst[] Tables
        {
            get
            {
                var nodes = XmlNode.SelectNodes("dev[@name='tables']/table[@name='CorrectionTemp']");
                if (nodes == null)
                    return null;             

                var tables = new XmlTableConst[nodes.Count];
                for (var idx = 0; idx < tables.Length; idx++)
                    tables[idx] = new XmlTableConst(nodes[idx]);

                return tables;
            }
        }

        public XmlTdConst[] Td
        {
            get
            {
                var xmlNode = XmlNode.SelectNodes("dev[@name='td']");

                var td = new XmlTdConst[xmlNode.Count];

                for (var index = 0; index < xmlNode.Count; index++)
                {
                    td[index] = new XmlTdConst(xmlNode[index]);
                }
                return td;
            }
        }

        public XmlClassPidConst[] Pid
        {
            get
            {
                var xmlNode = XmlNode.SelectNodes("dev[@name='pid']");

                var pids = new XmlClassPidConst[xmlNode.Count];

                for (var index = 0; index < xmlNode.Count; index++)
                {
                    pids[index] = new XmlClassPidConst(xmlNode[index]);
                }

                return pids;
            }
        }

        public bool UsePid
        {
            get
            {

                return false;
                if (Pid == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeChamberSectionScript);
    }   
}
