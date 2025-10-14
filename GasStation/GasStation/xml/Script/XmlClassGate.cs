using GasStation.UserControls.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.Constant;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace GasStation.xml
{
    public class XmlClassGate : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы РРГ
        /// </summary>
        public XmlClassGateConst GateConst { get; set; }

        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassGateView GateView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">номер системы создания вакуума</param>
        /// <param name="pumpSysConst">Константы системы создания вакуума</param>
        /// <param name="pumpSysView">Класс для отображения</param>
        public XmlClassGate(int num, XmlClassGateConst gateConst, ClassGateView gateView)
        {
            TypeElement = TypeElement.Gate;

            GateConst = gateConst;
            GateView = gateView;
            TypeElement = TypeElement.Loader;


            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "gate";
            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrOpenGate = xmlDocument.CreateAttribute("openGate");
            atrOpenGate.Value = "False";

            var atrCloseGate = xmlDocument.CreateAttribute("closeGate");
            atrOpenGate.Value = "False";

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";
            #endregion

            #region Добавление атрибутов

            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrOpenGate);
            _xmlNode.Attributes.Append(atrCloseGate);
            _xmlNode.Attributes.Append(atrPriv);

            #endregion

            GateView.UsePriv = UsePriv;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlPumpSysNode">XML узел с заданием для системы создания вакуума для шага скрипта</param>
        /// <param name="pumpSysConst">Константы системы создания вакуума</param>
        /// <param name="pumpSysView">Класс для отображения</param>
        public XmlClassGate(XmlNode xmlGateNode, XmlClassGateConst gateConst, ClassGateView gateView) : base(xmlGateNode)
        {
            TypeElement = TypeElement.Gate;

            GateConst = gateConst;
            GateView = gateView;
        }


        public bool gateOpen=false;
        public bool GateOpen
        {
            get
            {
                return gateOpen;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;

                if (value)
                    GateClose = false;
                gateOpen = value;
                PropertyIsChange("GateOpen");
                PropertyIsChange("GateClose");
            }
        }

        public bool gateClose = false;
        public bool GateClose
        {
            get
            {
                return gateClose;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;

                if (value)
                    GateOpen = false;

                gateClose = value;
                PropertyIsChange("GateOpen");
                PropertyIsChange("GateClose");
            }
        }
        public string GateNum => $"Датчик №{Num}";
        public override bool UsePriv
        {
            get
            {
                var setupTemp = false;
                bool.TryParse(XmlNode.Attributes["usePriv"].Value, out setupTemp);
                return setupTemp;
            }
            set
            {
                if (!GateConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }

                XmlNode.Attributes["usePriv"].Value = value.ToString();
                PropertyIsChange("UsePriv");
            }
        }

        public override bool EnabledChangedScript => UsePriv || GateConst.EnabledChangedScript;
    }
}
