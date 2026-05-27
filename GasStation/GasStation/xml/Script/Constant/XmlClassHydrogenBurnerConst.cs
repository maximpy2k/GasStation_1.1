using System;
using System.Linq;
using System.Xml;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script.Constant;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Constant.XmlConst.Elements
{
    public class XmlClassHydrogenBurnerConst :XmlBaseConst
    {
        public XmlClassHydrogenBurnerConst (XmlNode xmlNode):base(xmlNode)
        {            
        }      

        public int TdBurnerNum
        {
            get
            {
                return TdBurner.DevNum;
            }
        }

        public int TdFireNum
        {
            get
            {
                return TdFire.DevNum;
            }
        }

        /// <summary>
        /// Нагреватель
        /// </summary>
        public XmlClassDioPortConst DioHeaterConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='heater']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassDioPortConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Узел Xml
        /// </summary>
        public double UpSpeed
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev/EditFild[@name='maxUpSpeed']");
                if (xmlConst == null)
                    return 100;
                return Convert.ToDouble(xmlConst.Attributes["value"].Value);
            }
        }

        public double DownSpeed
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("dev/EditFild[@name='maxDownSpeed']");
                if (xmlConst == null)
                    return 100;
                return Convert.ToDouble(xmlConst.Attributes["value"].Value);
            }
        }

        /// <summary>
        /// Пускатель
        /// </summary>
        public XmlClassDioPortConst DioRealyConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='relay']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassDioPortConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Датчик воды
        /// </summary>
        public XmlClassSensorConst DioWaterConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='status']/dev[@name = 'water']");
                if (node == null)
                    return null;
                
                var dioConst = new XmlClassSensorConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Датчик пламени
        /// </summary>
        public XmlClassSensorConst DioFireConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='status']/dev[@name = 'fire']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassSensorConst(node);

                return dioConst;
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

        public XmlTdConst TdFire 
        {
            get
            {
                if (Td.Length <= 1 ) 
                    return null;

                return (XmlTdConst)(Td[1]);
            }
        }

        public XmlTdConst TdBurner => (XmlTdConst)(Td[0]);

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

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeBurningScript);

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

    }
}
