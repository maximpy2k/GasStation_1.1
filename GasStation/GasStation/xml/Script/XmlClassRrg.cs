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
    /// Xml узел РРГ
    /// </summary>
    public class XmlClassRrg:XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы РРГ
        /// </summary>
        public XmlClassRrgConst RrgConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassRrgView RrgView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">номер РРГ</param>
        /// <param name="rrgConst">Константы РРГ</param>
        /// <param name="rrgView">Класс для отображения</param>
        public XmlClassRrg(int num, XmlClassRrgConst rrgConst, ClassRrgView rrgView)
        {

            TypeElement = TypeElement.Rrg;
            RrgConst = rrgConst;
            RrgView = rrgView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Добавление клапана
            var fl = new XmlClassFlap(1, RrgConst.FlapConst,rrgView.FlapView);
            var importNode = xmlDocument.ImportNode(fl.XmlNode, true);
            _xmlNode.AppendChild(importNode); 
            #endregion
            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "rrg";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrSetupVal = xmlDocument.CreateAttribute("setupValue");
            atrSetupVal.Value = (0).ToString();

            var atrUsePid = xmlDocument.CreateAttribute("usePid");
            atrUsePid.Value = "False";

            var atrTypeReg = xmlDocument.CreateAttribute("typeReg");
            atrTypeReg.Value = "MaximumSpeed";// typeReg.ToString();

            var atrTimeRaise = xmlDocument.CreateAttribute("timeRaise");
            atrTimeRaise.Value = (20).ToString();

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";

            #endregion               
            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrSetupVal);
            _xmlNode.Attributes.Append(atrUsePid);
            _xmlNode.Attributes.Append(atrTypeReg);
            _xmlNode.Attributes.Append(atrTimeRaise);
            _xmlNode.Attributes.Append(atrPriv);
            #endregion

            RrgView.UsePriv = UsePriv;
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlRrgNode">XML узел с заданием для РРГ для шага скрипта</param>
        /// <param name="rrgConst">Константы РРГ</param>
        /// <param name="rrgView">Класс для отображения</param>
        public XmlClassRrg(XmlNode xmlRrgNode, XmlClassRrgConst rrgConst, ClassRrgView rrgView) :base(xmlRrgNode)
        {
            TypeElement = TypeElement.Rrg;

            RrgConst = rrgConst;
            RrgView = rrgView;
        }

        /// <summary>
        /// Время выхода РРГ на заданный режим
        /// </summary>
        public double TimeRaise
        {
            get
            {
                var timeRaise = -1.0;
                double.TryParse(XmlNode.Attributes["timeRaise"].Value, out timeRaise);
                return timeRaise;
            }

            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                XmlNode.Attributes["timeRaise"].Value = value.ToString();
                PropertyIsChange("TimeRaise");
            }
        }
        
        /// <summary>
        /// Установленное значение РРГ
        /// </summary>
        public double SetupValue
        {
            get
            {
                var setupValue = double.NaN;
                if (XmlNode.Attributes != null) double.TryParse(XmlNode.Attributes["setupValue"].Value, out setupValue);
                return setupValue;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                if (value > RrgConst.MasCapConst[0].Table.MaxVal)
                {
                    XmlNode.Attributes["setupValue"].Value = RrgConst.MasCapConst[0].Table.MaxVal.ToString(); 
                    return;
                }

                if (value < RrgConst.MasCapConst[0].Table.MinVal)
                {
                    XmlNode.Attributes["setupValue"].Value = RrgConst.MasCapConst[0].Table.MinVal.ToString();
                    return;
                }
                XmlNode.Attributes["setupValue"].Value = value.ToString();
                PropertyIsChange("SetupValue");
            }
        }


        /// <summary>
        /// Режим работы РРГ
        /// </summary>
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
                if(XmlNode.Attributes["usePid"]==null)
                {
                    MessageBox.Show("Используется старая версия скрипта.\nНет атрибута 'usePid'");
                    return;
                }

                XmlNode.Attributes["usePid"].Value = value.ToString();
                PropertyIsChange("UsePid");
            }
        }

        private XmlClassFlap flap;
        /// <summary>
        /// Клапан РРГ
        /// </summary>
        public XmlClassFlap Flap
        {
            get
            {
                if (flap != null)
                    return flap;
                var node = _xmlNode.SelectSingleNode("dev[@name='flap']");
                flap = new XmlClassFlap(node, RrgConst.FlapConst,RrgView.FlapView);
                return flap;
            }
        }

        /// <summary>
        /// Строковая подпись РРГ
        /// </summary>
        public string RrgNum => $"РРГ №{Num}";

        /// <summary>
        /// Использование привелегий
        /// </summary>
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
                if (!RrgConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }

                XmlNode.Attributes["usePriv"].Value = value.ToString();

                var node = XmlNode.SelectSingleNode("dev[@name='flap']");

                node.Attributes["usePriv"].Value = value.ToString();

                PropertyIsChange("UsePriv");
            }
        }

        /// <summary>
        /// Возможность изменнеия скрипта
        /// </summary>
        public override bool EnabledChangedScript
        {
            get { return UsePriv || RrgConst.EnabledChangedScript; }
        }

        /// <summary>
        /// Отображение возможности изменения настроек для пользователя
        /// </summary>
        public string VisibleChanged => EnabledChangedScript ? "Visible" : "Collapsed";
    }
}
