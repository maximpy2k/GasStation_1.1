using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script.XmlScript
{
    public class XmlClassFreqGenerator : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы ШИМ генератора
        /// </summary>
        public XmlClassFreqGeneratorConst  ShimGeneratorConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassFreqGeneratorView ShimGeneratorView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">Номер водородной горелки</param>
        /// <param name="hydrogenBurner">Константы водородной горелки</param>
        /// <param name="hydrogenBurnerView">Класс для отображения</param>
        public XmlClassFreqGenerator(int num, XmlClassFreqGeneratorConst shimGeneratorConst, ClassFreqGeneratorView shimGeneratorView) :base()
        {
            TypeElement = TypeElement.HydrogenBurning;

            ShimGeneratorConst = shimGeneratorConst;
            ShimGeneratorView = shimGeneratorView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов

            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "freqGenerator";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrStartupSwitch = xmlDocument.CreateAttribute("startupSwitch");
            atrStartupSwitch.Value = false.ToString();

            var atrImpDuration1 = xmlDocument.CreateAttribute("impDuration1");
            atrImpDuration1.Value = (0.0).ToString();
            var atrImpDelay1 = xmlDocument.CreateAttribute("impDelay1");
            atrImpDelay1.Value = (0.0).ToString();

            var atrImpDuration2 = xmlDocument.CreateAttribute("impDuration2");
            atrImpDuration2.Value = (0.0).ToString();
            var atrImpDelay2 = xmlDocument.CreateAttribute("impDelay2");
            atrImpDelay2.Value = (0.0).ToString();

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";

            #endregion

            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);           
            _xmlNode.Attributes.Append(atrStartupSwitch);
            _xmlNode.Attributes.Append(atrPriv);

            _xmlNode.Attributes.Append(atrImpDuration1);
            _xmlNode.Attributes.Append(atrImpDelay1);
            _xmlNode.Attributes.Append(atrImpDuration2);
            _xmlNode.Attributes.Append(atrImpDelay2);
            #endregion

            //HydrogenBurnerView.UsePriv = UsePriv;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для водородной горелки для шага скрипта</param>
        /// <param name="hydrogenBurner">Константы водородной горелки</param>
        /// <param name="hydrogenBurnerView">Класс для отображения</param>
        public XmlClassFreqGenerator(XmlNode xmlNode, XmlClassFreqGeneratorConst freqGenerator, ClassFreqGeneratorView freqGeneratorsView) :base(xmlNode)
        {
            TypeElement = TypeElement.HydrogenBurning;

            ShimGeneratorConst = freqGenerator;
            ShimGeneratorView = freqGeneratorsView;
        }
  
        /// <summary>
        /// Включение питания ШИМ генератора
        /// </summary>
        public bool StartupSwitch
        {
            get
            {
                var sw = false;
                bool.TryParse(XmlNode.Attributes["startupSwitch"].Value, out sw);
                return sw;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                XmlNode.Attributes["startupSwitch"].Value = value.ToString();
                PropertyIsChange("StartupSwitch");
            }
        }

        /// <summary>
        /// Длительность первого импульса
        /// </summary>
        public double ImpulsDuration1
        {
            get
            {
                var duration = 0.0;
                double.TryParse(XmlNode.Attributes["impDuration1"].Value, out duration);
                return duration;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                XmlNode.Attributes["impDuration1"].Value = value.ToString();
                PropertyIsChange("ImpulsDuration1");

            }
        }

        /// <summary>
        /// Пауза между импульсами 1
        /// </summary>
        public double ImpulsDelay1
        {
            get
            {
                var duration = 0.0;
                double.TryParse(XmlNode.Attributes["impDelay1"].Value, out duration);
                return duration;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                XmlNode.Attributes["impDelay1"].Value = value.ToString();
                PropertyIsChange("ImpulsDelay1");
            }
        }

        /// <summary>
        /// Длительность второго импульса
        /// </summary>
        public double ImpulsDuration2
        {
            get
            {
                var duration = 0.0;
                double.TryParse(XmlNode.Attributes["impDuration2"].Value, out duration);
                return duration;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                XmlNode.Attributes["impDuration2"].Value = value.ToString();
                PropertyIsChange("ImpulsDuration2");
            }
        }

        /// <summary>
        /// Пауза между импульсами 2
        /// </summary>
        public double ImpulsDelay2
        {
            get
            {
                var duration = 0.0;
                double.TryParse(XmlNode.Attributes["impDelay2"].Value, out duration);
                return duration;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                XmlNode.Attributes["impDelay2"].Value = value.ToString();
                PropertyIsChange("ImpulsDelay2");
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
                if (!ShimGeneratorConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }
                XmlNode.Attributes["usePriv"].Value = value.ToString();
            }
        }

        public override bool EnabledChangedScript
        {
            get { return UsePriv || ShimGeneratorConst.EnabledChangedScript; }
        }

    }
}
