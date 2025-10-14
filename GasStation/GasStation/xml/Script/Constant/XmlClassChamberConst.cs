using System;
using System.Linq;
using System.Xml;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script.Constant;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Constant
{
    public class XmlClassChamberConst:XmlBaseConst
    {
        public XmlClassChamberConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        public XmlClassThermoSectionConst[] ThermoSectionConst
        {
            get
            {
                var xmlConst = XmlNode.SelectNodes("dev[@name='section']");

                var masConst = new XmlClassThermoSectionConst[xmlConst.Count];
                for (var i = 0; i < xmlConst.Count; i++)
                {
                    masConst[i] = new XmlClassThermoSectionConst(xmlConst[i]);
                }
                return masConst;
            }
        }

        /// <summary>
        /// Датчик воды
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
        /// Датчик воды передний фланец
        /// </summary>
        public XmlClassSensorConst DioWaterForwardConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='status']/dev[@name = 'waterForward']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassSensorConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Датчик воды задний фланец
        /// </summary>
        public XmlClassSensorConst DioWaterBackwardConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='status']/dev[@name = 'waterBackward']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassSensorConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Датчик воды задний фланец
        /// </summary>
        public XmlClassSensorConst DioVoltage24Const
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='status']/dev[@name = 'IsVoltage24']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassSensorConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Датчик открытой заслонки
        /// </summary>
        /// 
        public XmlClassSensorConst DioDumperOpenConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='status']/dev[@name = 'dumperOpen']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassSensorConst(node);

                return dioConst;
            }
        }

        /// <summary>
        /// Датчик закрытой заслонки
        /// </summary>
        public XmlClassSensorConst DioDumperCloseConst
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='status']/dev[@name = 'dumperClosed']");
                if (node == null)
                    return null;

                var dioConst = new XmlClassSensorConst(node);
                return dioConst;
            }
        }

        public int CrashTemp
        {
            get
            {
                var crashTemp = Convert.ToInt32(XmlNode.SelectSingleNode("dev/EditFild[@name = 'crashTemp']").Attributes["value"].Value);
                return crashTemp;
            }
            set { }
        }

        private int[] timeToOff=new []{60,60,60};
        public int[] TimeToOff
        {
            get
            {
                var node = (XmlNode.SelectSingleNode("dev/EditFild[@name = 'timeToOff']"));
                if (node == null)
                    return new[] { 60, 60, 60 };

                var ToOff = node.Attributes["value"].Value;
                timeToOff= new []{Convert.ToInt32(ToOff), Convert.ToInt32(ToOff), Convert.ToInt32(ToOff) };
                return timeToOff;
            }
            set { timeToOff = value; }
        }




        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeChamberScript);
    }
}