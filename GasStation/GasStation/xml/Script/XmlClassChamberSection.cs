using System;
using System.Xml;
using GasStation.xml.Const.Elements;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using GasStation.ViewModels.Elements;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;
using GasStation.ModalWindows.Commands;

namespace GasStation.xml.Script
{
    /// <summary>
    /// Xml узел камеры
    /// </summary>
    public class XmlClassChamberSection: XmlBaseClassElementScript
    { 
        /// <summary>
        /// Константы термосекции
        /// </summary>
        public XmlClassThermoSectionConst ThermoSectionConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassChamberSectionView ThermoSectionView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">номер термосекции</param>
        /// <param name="thermoSectionConst">Константы термосекции</param>
        /// /// <param name="ThermoSectionView">Класс для отображения</param>
        public XmlClassChamberSection(int num, XmlClassThermoSectionConst thermoSectionConst, ClassChamberSectionView ThermoSectionView)
        {
            TypeElement = TypeElement.ChamberSection;

            ThermoSectionConst = thermoSectionConst;
            this.ThermoSectionView = ThermoSectionView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "section";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrUseSetThermo = xmlDocument.CreateAttribute("useSetThermo");
            atrUseSetThermo.Value = true.ToString();

            var atrUsePid = xmlDocument.CreateAttribute("usePid");
            atrUsePid.Value = false.ToString();

            var atrTypeReg = xmlDocument.CreateAttribute("typeReg");
            atrTypeReg.Value = "MaximumSpeed";

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";
            #endregion

            #region Добавление атрибутов

            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);            
            _xmlNode.Attributes.Append(atrUseSetThermo);
            _xmlNode.Attributes.Append(atrUsePid);
             _xmlNode.Attributes.Append(atrTypeReg);
            _xmlNode.Attributes.Append(atrPriv);
            #endregion
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для термосекции для шага скрипта </param>
        /// <param name="thermoSectionConst">Константы термоскции</param>
        /// /// <param name="ThermoSectionView">Класс для отображения</param>
        public XmlClassChamberSection(XmlNode xmlNode, XmlClassThermoSectionConst thermoSectionConst, ClassChamberSectionView ThermoSectionView) :base(xmlNode)
        {
            TypeElement = TypeElement.ChamberSection;

            ThermoSectionConst = thermoSectionConst;
            this.ThermoSectionView = ThermoSectionView;
        }            
        /// <summary>
        /// Номер таблицы корректировки
        /// </summary>
        public int TabNum => UsePid ? 1 : 0;

        /// <summary>
        /// Использование ПИД регулятора
        /// </summary>
        public bool 
UsePid
        {
            get
            {
                bool usePid = false;
                bool.TryParse(XmlNode.Attributes["usePid"].Value, out usePid);
                return usePid;
            }
            set
            {
                //if (!EnabledChangedScript)
                //{
                //    return;
                //}

                XmlNode.Attributes["usePid"].Value = value.ToString();
                PropertyIsChange("UsePid");
                PropertyIsChange("SetupTemp");
            }
        }
        /// <summary>
        /// Использовать заданную температуру
        /// </summary>
        public bool UseSetThermo
        {
            get
            {
                bool useSetThermo = false;
                bool.TryParse(XmlNode.Attributes["useSetThermo"].Value, out useSetThermo);
                return useSetThermo;
            }
            set
            {
                XmlNode.Attributes["useSetThermo"].Value = value.ToString();
                PropertyIsChange("UseSetThermo");
                PropertyIsChange("SetupTemp");
            }
        }
       
        /// <summary>
        /// Режим работы Термосекции
        /// </summary>
        public Regims TypeReg
        {
            get
            {
                return (Regims) Enum.Parse(typeof (Regims), XmlNode.Attributes["typeReg"].Value);
            }
            set
            {
                XmlNode.Attributes["typeReg"].Value=value.ToString();
                PropertyIsChange("TypeReg");
            }
        }

        /// <summary>
        /// Установленное значение камеры в градусах из общего задания камеры
        /// </summary>
        public double SetupTemp
        {
            get
            {
                var setupTemp = double.NaN;
                double.TryParse(XmlNode.ParentNode.Attributes["setupTemp"].Value, out setupTemp);


                //XmlTableFild beg = new XmlTableFild(0.0, 0.0, true);
                //XmlTableFild end = new XmlTableFild(1500, 1500, true);

                //var tabNum = 0;
                //if (UseSetThermo)
                //    tabNum = 0;

                //if (UsePid)
                //    tabNum = 1;

                var tab = ThermoSectionConst.Tables[TabNum];


                var points = tab.UsedPoints.OrderBy(dat=>dat.X).Distinct().ToArray();

                if (points.Length == 0)
                    return setupTemp;

                if (setupTemp < points[0].X)
                    return setupTemp;
                if (setupTemp > points.Last().X)
                    return setupTemp;

                var minMas = points.Where(dat => dat.X <= setupTemp).ToArray();
                var maxMas = points.Where(dat => dat.X > setupTemp).ToArray();

                if (minMas.Length != 0 && maxMas.Length == 0)
                    return minMas.Last().Y;
                if (minMas.Length == 0 && maxMas.Length != 0)
                    return maxMas[0].Y;


                var beg = minMas.Last();
                var end = maxMas.First();

                var k = (beg.Y - end.Y) / (beg.X - end.X);
                var b = beg.Y - k * beg.X;

                return k * setupTemp + b;                
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
                if (!ThermoSectionConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
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
            get { return UsePriv || ThermoSectionConst.EnabledChangedScript; }
        }

        public void RefrashTemp()
        {
            PropertyIsChange("SetupTemp");
        }

        private CommandOpenTable cmdOpenTable = new CommandOpenTable();

        public CommandOpenTable CmdOpenTable
        {
            get
            {
                cmdOpenTable.IsEnabled = ThermoSectionConst.EnabledChangedScript;
                return cmdOpenTable;
            }
            set { cmdOpenTable = value; }
        }
    }
}
