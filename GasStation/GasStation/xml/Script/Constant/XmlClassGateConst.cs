using GasStation.xml.Constant;
using GasStation.xml.Script.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GasStation.xml.Script.Constant
{
    public class XmlClassGateConst : XmlBaseConst
    {
        public XmlClassGateConst(XmlNode xmlNode) : base(xmlNode)
        {
        }
        public XmlClassSensorConst DioGateOpenConst
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
        public XmlClassSensorConst DioGateCloseConst
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


        /// <summary>
        /// Загрузка
        /// </summary>
        public XmlClassDioPortConst OpenGate
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='openDumper']");
                return node == null ? null : new XmlClassDioPortConst(node);
            }
        }
        /// <summary>
        /// Выгрузка
        /// </summary>
        public XmlClassDioPortConst CloseGate
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='closeDumper']");
                return node == null ? null : new XmlClassDioPortConst(node);
            }
        }
        public bool GateControl
        {
            get
            {
                if (OpenGate == null)
                    return false;
                else
                    return true;
            }
        }

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeGateScript);
    }
}
