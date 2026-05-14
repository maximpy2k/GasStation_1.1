using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace GasStation.xml.Script.XmlScript
{
    public class XmlClassHydrogenBurning : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы водородной горелки
        /// </summary>
        public XmlClassHydrogenBurnerConst  HydrogenBurnerConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassHydrogenBurnerView HydrogenBurnerView { get; set; }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">Номер водородной горелки</param>
        /// <param name="hydrogenBurner">Константы водородной горелки</param>
        /// <param name="hydrogenBurnerView">Класс для отображения</param>
        public XmlClassHydrogenBurning(int num, XmlClassHydrogenBurnerConst  hydrogenBurner, ClassHydrogenBurnerView hydrogenBurnerView) :base()
        {
            TypeElement = TypeElement.HydrogenBurning;

            HydrogenBurnerConst = hydrogenBurner;
            HydrogenBurnerView = hydrogenBurnerView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов

            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "hydrogenBurning";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrState = xmlDocument.CreateAttribute("heat");
            atrState.Value = false.ToString();

            var atrUsePid = xmlDocument.CreateAttribute("usePid");
            atrUsePid.Value = "False";

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";

            var atrTypeReg = xmlDocument.CreateAttribute("typeReg");
            atrTypeReg.Value = "DefaultSpeed";// typeReg.ToString();

            var atrSetupVal = xmlDocument.CreateAttribute("setupValue");
            atrSetupVal.Value = (35).ToString();

            #endregion

            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);           
            _xmlNode.Attributes.Append(atrState);
            _xmlNode.Attributes.Append(atrPriv);
            _xmlNode.Attributes.Append(atrUsePid);
            _xmlNode.Attributes.Append(atrTypeReg);
            _xmlNode.Attributes.Append(atrSetupVal);
            #endregion

            HydrogenBurnerView.UsePriv = UsePriv;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для водородной горелки для шага скрипта</param>
        /// <param name="hydrogenBurner">Константы водородной горелки</param>
        /// <param name="hydrogenBurnerView">Класс для отображения</param>
        public XmlClassHydrogenBurning(XmlNode xmlNode, XmlClassHydrogenBurnerConst  hydrogenBurner, ClassHydrogenBurnerView hydrogenBurnerView) :base(xmlNode)
        {
            TypeElement = TypeElement.HydrogenBurning;

            HydrogenBurnerConst = hydrogenBurner;
            HydrogenBurnerView = hydrogenBurnerView;
        }
        /// <summary>
        /// Включение нагрева
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

        public Regims TypeReg
        {
            get
            {
                var currType = (Regims)Enum.Parse(typeof(Regims), XmlNode.Attributes["typeReg"].Value);
                return currType;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;

                XmlNode.Attributes["typeReg"].Value = value.ToString();
                PropertyIsChange("TypeReg");
            }
        }

        /// <summary>
        /// Использование ПИД регулятора
        /// </summary>
        public bool UsePid
        {
            get
            {
                if (XmlNode.Attributes["usePid"] == null)
                    return false;
                bool usePid;

                bool.TryParse(XmlNode.Attributes["usePid"].Value, out usePid);
                return usePid;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;
                if (XmlNode.Attributes["usePid"] == null)
                {
                    MessageBox.Show("Используется старая версия скрипта.\nНет атрибута 'usePid'");
                    return;
                }

                XmlNode.Attributes["usePid"].Value = value.ToString();
                PropertyIsChange("UsePid");
            }
        }

        public double SetupValue
        {
            get
            {
                var setupValue = 35.0;
                if (XmlNode.Attributes != null) double.TryParse(XmlNode.Attributes["setupValue"].Value, out setupValue);
                return setupValue;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }


                XmlNode.Attributes["setupValue"].Value = value.ToString();
                PropertyIsChange("SetupValue");
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
                if (!HydrogenBurnerConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
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
            get { return UsePriv || HydrogenBurnerConst.EnabledChangedScript; }
        }

    }
}
