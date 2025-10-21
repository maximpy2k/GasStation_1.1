using System;
using System.Xml;
using GasStation.xml.Const.Elements;
using System.ComponentModel;
using System.Linq;
using GasStation.ViewModels.Elements;
using System.Windows;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script
{
    /// <summary>
    /// Xml узел Барботера
    /// </summary>
    public class XmlClassBubbler : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы Барботера
        /// </summary>
        public XmlClassBubblerConst BubblerConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassBubblerView BubblerView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">номер барботера</param>
        /// <param name="bubblerConst">Константы барботера</param>
        /// <param name="bubblerView">Класс для отображения</param>
        public XmlClassBubbler(int num, XmlClassBubblerConst bubblerConst, ClassBubblerView bubblerView)
        {
            TypeElement = TypeElement.Bubbler;

            BubblerConst = bubblerConst;
            BubblerView = bubblerView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "bubbler";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrSetupVal = xmlDocument.CreateAttribute("setupValue");
            atrSetupVal.Value = (35).ToString();

            var atrUseBubbler = xmlDocument.CreateAttribute("UseBubbler");
            atrUseBubbler.Value = "False";

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";

            #endregion               
            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrUseBubbler);
            _xmlNode.Attributes.Append(atrSetupVal);
            _xmlNode.Attributes.Append(atrPriv);
            #endregion

            bubblerView.UsePriv = UsePriv;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlBublerNode">XML узел с заданием для барботера для шага скрипта</param>
        /// <param name="bubblerConst">Константы барботера</param>
        /// <param name="bubblerView">Класс для отображения</param>
        public XmlClassBubbler(XmlNode xmlBublerNode, XmlClassBubblerConst bubblerConst, ClassBubblerView bubblerView) :base(xmlBublerNode)
        {
            TypeElement = TypeElement.Bubbler;

            BubblerConst = bubblerConst;
            BubblerView = bubblerView;
        }
        

        ///// <summary>
        ///// Строковая подпись Барботера
        ///// </summary>
        public string BubblerNum
        {
            get
            {
                return $"{BubblerConst.RusName} №{BubblerConst.DevNum}";
            }
        }

        /// <summary>
        /// Состояние Flap (Клапана)
        /// </summary>
        //public bool UseBubbler
        //{
        //    get
        //    {
        //        bool val=false;
        //        bool.TryParse(XmlNode.Attributes["UseBubbler"].Value, out val);
        //        var result = (XmlNode.Attributes["UseBubbler"].Value);

        //        var flapState = bool.Parse(result);
        //        return flapState;
        //    }
        //    set
        //    {
        //        if (!EnabledChangedScript)
        //        {
        //            return;
        //        }
        //        XmlNode.Attributes["UseBubbler"].Value = value.ToString();
        //        PropertyIsChange("UseBubbler");
        //    }
        //}

        public bool UseBubbler
        {
            get
            {
                bool val = false;
                bool.TryParse(XmlNode.Attributes["UseBubbler"].Value, out val);
                var result = (XmlNode.Attributes["UseBubbler"].Value);

                var flapState = bool.Parse(result);
                return flapState;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                XmlNode.Attributes["UseBubbler"].Value = value.ToString();
                PropertyIsChange("UseBubbler");
            }
        }

        /// <summary>
        /// Установленное значение РРГ
        /// </summary>
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
                if (!BubblerConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
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
            get { return UsePriv || BubblerConst.EnabledChangedScript; }
        }
    }
}
