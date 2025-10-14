using System.Xml;
using GasStation.xml.Const.Elements;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Script.Security;
using GasStation.ModalWindows.Commands;

namespace GasStation.xml.Script
{
    /// <summary>
    /// Xml узел камеры
    /// </summary>
    public class XmlClassChamber : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы камеры
        /// </summary>
        public XmlClassChamberConst ChamberConst { get; set; }

        /// <summary>
        /// Класс для отображения
        /// </summary>       
        public ClassChamberView ChamberView { get; set; }

        public bool UsePid
        {
            get { return false; }
            set { var a = value; }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">Номер шага скрипта</param>
        /// <param name="xmlClassChamberConst">Константы камеры</param>
        /// <param name="chamberView">Класс для отображения</param>
        public XmlClassChamber(int num, XmlClassChamberConst xmlClassChamberConst, ClassChamberView chamberView)
        {

            ChamberConst = xmlClassChamberConst;
            ChamberView = chamberView;

            

            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            for (int i = 0; i < xmlClassChamberConst.ThermoSectionConst.Length; i++)
            {
                var fl = new XmlClassChamberSection(i + 1, xmlClassChamberConst.ThermoSectionConst[i], ChamberView.ThermoSectionView[i]);
                var importNode = xmlDocument.ImportNode(fl.XmlNode, true);
                _xmlNode.AppendChild(importNode);
            }

            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "cham";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrSetupTemp = xmlDocument.CreateAttribute("setupTemp");
            atrSetupTemp.Value = (25.0).ToString();

            var atrHeat = xmlDocument.CreateAttribute("heat");
            atrHeat.Value = false.ToString();

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";
            #endregion

            #region Добавление атрибутов

            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrSetupTemp);
            _xmlNode.Attributes.Append(atrHeat);
            _xmlNode.Attributes.Append(atrPriv);

            #endregion

            ChamberView.UsePriv = UsePriv;
        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для камеры для шага скрипта</param>
        /// <param name="xmlClassChamberConst">Константы камеры</param>
        /// <param name="chamberView">Класс для отображения</param>
        public XmlClassChamber(XmlNode xmlNode, XmlClassChamberConst xmlClassChamberConst, ClassChamberView chamberView): base(xmlNode)
        {
            ChamberConst = xmlClassChamberConst;
            ChamberView = chamberView;
        }

        private XmlClassChamberSection[] chamberSections;
        /// <summary>
        /// Параметры секций камеры для шага скрипта
        /// </summary>
        public XmlClassChamberSection[] ChamberSections
        {
            get
            {
                if (chamberSections != null)
                    return chamberSections;

                var nodes = XmlNode.SelectNodes("dev[@name='section']");
                chamberSections = new XmlClassChamberSection[nodes.Count];
                for (int i = 0; i < nodes.Count; i++)
                    chamberSections[i] = new XmlClassChamberSection(nodes[i], ChamberConst.ThermoSectionConst[i], ChamberView.ThermoSectionView[i]);

                return chamberSections;
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
                if (!ChamberConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
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
            get { return UsePriv || ChamberConst.EnabledChangedScript; }
        }

        //public bool Gogo { get; set; } = false;

        /// <summary>
        /// Установленное значение камеры в градусах с учетом таблицы корректировки
        /// </summary>
        public double SetupTemp
        {
            get
            {
                var setupTemp = double.NaN;
                double.TryParse(XmlNode.Attributes["setupTemp"].Value, out setupTemp);
                return setupTemp;
            }
            set
            {
                //Если установочное значение больше аварийного
                if (value > ChamberConst.CrashTemp)
                    return;

                if (!EnabledChangedScript)
                {
                    return;
                }

                XmlNode.Attributes["setupTemp"].Value = value.ToString();
                PropertyIsChange("SetupTemp");
                PropertyIsChange("ChamberSections");
            }
        }
        /// <summary>
        /// Включение реле нагрева
        /// </summary>
        public bool Heat
        {
            get
            {
                var heat = false;
                bool.TryParse(XmlNode.Attributes["heat"].Value, out heat);
                return heat;
            }
            set
            {
                XmlNode.Attributes["heat"].Value = value.ToString();
                PropertyIsChange("Heat");
            }

        }


    }
}
