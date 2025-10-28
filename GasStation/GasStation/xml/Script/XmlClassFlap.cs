using GasStation.ViewModels.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Constant;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script
{
    /// <summary>
    /// Xml узел Flap (Клапан)
    /// </summary>
    public class XmlClassFlap : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы клапана
        /// </summary>
        public XmlClassFlapConst FlapConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        private ClassFlapView _flapView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">Номер клапана</param>
        /// <param name="flapConst">Константы клапана</param>
        /// <param name="flapView">Класс для отображения</param>
        public XmlClassFlap(int num, XmlClassFlapConst flapConst, ClassFlapView flapView )
        {
            TypeElement  = TypeElement.Flap;

            FlapConst = flapConst;
            _flapView = flapView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов

            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "flap";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrFlapState = xmlDocument.CreateAttribute("flapState");
            atrFlapState.Value = false.ToString();

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";
            #endregion                           

            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrFlapState);
            _xmlNode.Attributes.Append(atrPriv);

            #endregion
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlFlapNode">XML узел с заданием для клапана для шага скрипта</param>
        /// <param name="flapConst">Константы клапана</param>
        /// <param name="flapView">Класс для отображения</param>
        public XmlClassFlap(XmlNode xmlFlapNode, XmlClassFlapConst flapConst, ClassFlapView flapView) : base(xmlFlapNode)
        {
            TypeElement = TypeElement.Flap;

            FlapConst = flapConst;
            _flapView = flapView;
        }

        /// <summary>
        /// Состояние Flap (Клапана)
        /// </summary>
        public bool FlapState
        {
            get
            {
                double val = double.NaN;
                double.TryParse(XmlNode.Attributes["flapState"].Value, out val);
                var result = (XmlNode.Attributes["flapState"].Value);

                var flapState = bool.Parse(result);
                return flapState;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;

                XmlNode.Attributes["flapState"].Value=value.ToString();
                PropertyIsChange("FlapState");
                PropertyIsChange("FlapView");
            }
        }

        public bool FlapView
        {
            get
            {
                if (FlapConst == null)
                    return true;
                if (FlapConst.FlapNormalState == false)
                    return FlapState;

                return !FlapState;
            }
            set
            {
                if (FlapConst == null)
                    return;
                if (FlapConst.FlapNormalState == false)
                {
                    FlapState = value;
                    return;
                }

                FlapState = !value;
                PropertyIsChange("FlapState");
                PropertyIsChange("FlapView");
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
                if (!FlapConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
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
            get
            {
                var flapEnabledChangedScript = FlapConst == null ? UsePriv : FlapConst.EnabledChangedScript;
                return UsePriv || flapEnabledChangedScript;
            }
        }

    }
}
