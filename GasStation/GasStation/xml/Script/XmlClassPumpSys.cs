using System;
using System.Xml;
using GasStation.xml.Const.Elements;
using System.ComponentModel;
using System.Linq;
using GasStation.ViewModels.Elements;
using System.Windows;
using GasStation.xml.Constant;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script
{
    /// <summary>
    /// Xml узел РРГ
    /// </summary>
    public class XmlClassPumpSys : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы РРГ
        /// </summary>
        public XmlClassPumpSysConst PumpSysConst { get; set; }

        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassPumpSysView PumpSysView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">номер системы создания вакуума</param>
        /// <param name="pumpSysConst">Константы системы создания вакуума</param>
        /// <param name="pumpSysView">Класс для отображения</param>
        public XmlClassPumpSys(int num, XmlClassPumpSysConst pumpSysConst, ClassPumpSysView pumpSysView)
        {
            TypeElement = TypeElement.PumpSys;

            PumpSysConst = pumpSysConst;
            PumpSysView = pumpSysView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "pumpSys";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrUseVacuumPump = xmlDocument.CreateAttribute("UseVacuumPump");
            atrUseVacuumPump.Value = "False";

            var atrUseRoughingPump = xmlDocument.CreateAttribute("UseRoughingPump");
            atrUseRoughingPump.Value = "False";

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";

            #endregion               
            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrUseVacuumPump);
            _xmlNode.Attributes.Append(atrUseRoughingPump);
            _xmlNode.Attributes.Append(atrPriv);
            #endregion

            PumpSysView.UsePriv = UsePriv;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlPumpSysNode">XML узел с заданием для системы создания вакуума для шага скрипта</param>
        /// <param name="pumpSysConst">Константы системы создания вакуума</param>
        /// <param name="pumpSysView">Класс для отображения</param>
        public XmlClassPumpSys(XmlNode xmlPumpSysNode, XmlClassPumpSysConst pumpSysConst, ClassPumpSysView pumpSysView) :base(xmlPumpSysNode)
        {
            TypeElement = TypeElement.PumpSys;

            PumpSysConst = pumpSysConst;
            PumpSysView = pumpSysView;
        }

        /// <summary>
        /// Строковая подпись вакуумной системы
        /// </summary>
        public string PumpSysNum
        {
            get
            {
                return $"Вак. система №{Num}";
            }
        }

        public string WorkVacuumPump
        {
            get
            {
                return $"Включение {PumpSysConst.VacuumPump.RusName}";
            }
        } 

        public string WorkRoughingPump => $"Включение {PumpSysConst.RoughingPump.RusName}";



        /// <summary>
        /// Включение вакуумного насоса
        /// </summary>
        public bool UseVacuumPump
        {
            get
            {
                bool val=false;
                bool.TryParse(XmlNode.Attributes["UseVacuumPump"].Value, out val);

                return val;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                XmlNode.Attributes["UseVacuumPump"].Value = value.ToString();
                PropertyIsChange("UseVacuumPump");
            }
        }

        /// <summary>
        /// Включение вакуумного насоса
        /// </summary>
        public bool UseRoughingPump
        {
            get
            {
                bool val = false;
                bool.TryParse(XmlNode.Attributes["UseRoughingPump"].Value, out val);

                return val;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                XmlNode.Attributes["UseRoughingPump"].Value = value.ToString();
                PropertyIsChange("UseRoughingPump");
            }
        }

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
                if (!PumpSysConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }

                XmlNode.Attributes["usePriv"].Value = value.ToString();
                PropertyIsChange("UsePriv");
            }
        }

        public override bool EnabledChangedScript
        {
            get { return UsePriv || PumpSysConst.EnabledChangedScript; }
        }
    }
}
