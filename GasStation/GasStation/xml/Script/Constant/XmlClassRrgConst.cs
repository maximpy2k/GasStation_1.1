using GasStation.xml.Constant;
using System;
using System.Linq;
using System.Xml;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassRrgConst:XmlBaseConst
    {
        public XmlClassRrgConst(XmlNode xmlNode):base(xmlNode)
        {
            //var p = Pid;
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

        /// <summary>
        /// Константы для насоса
        /// </summary>
        public XmlClassVacuumetrConst VacuumetrConst
        {
            get
            {
                var xmlNode = XmlNode.SelectSingleNode("dev[@name='vacuumetr']");
                if (xmlNode == null)
                    return null;
                return new XmlClassVacuumetrConst(xmlNode);
            }
        }

        /// <summary>
        /// Узел Xml
        /// </summary>
        public double RaiseSpeed
        {
            get
            {
                var xmlConstFlaps = XmlNode.SelectSingleNode("dev/EditFild[@name='raiseSpeed']");
                return Convert.ToDouble(xmlConstFlaps.Attributes["value"].Value);
            }
        }

        public double DownSpeed
        {
            get
            {
                var xmlConstFlaps = XmlNode.SelectSingleNode("dev/EditFild[@name='downSpeed']");
                return Convert.ToDouble(xmlConstFlaps.Attributes["value"].Value);
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

        public XmlClassFlapConst FlapConst
        {
            get
            {
                var flapNode = XmlNode.SelectSingleNode("dev[@name='flap']");
                if (flapNode == null)
                    return null;

                return new XmlClassFlapConst(flapNode);
            }
        }


        private double fluctRaise = 100;
        public double FluctRaise
        {
            get
            {
                var node = (XmlNode.SelectSingleNode("dev/EditFild[@name = 'fluctRaise']"));
                if (node == null)
                    return fluctRaise;
                fluctRaise = Convert.ToDouble(node.Attributes["value"].Value);
                return fluctRaise;
            }
            set { fluctRaise = value; }
        }

        /// <summary>
        /// Наличие клапана в РРГ
        /// </summary>
        public bool IsFlap
        {
            get
            {
                return FlapConst != null;
            }
        }


        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeRrgScript);
    }
}
