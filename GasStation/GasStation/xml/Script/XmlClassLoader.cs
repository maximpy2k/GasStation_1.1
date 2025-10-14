using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using GasStation.xml.Script.Security;
using GasStation.UserControls.Commands;
using GasStation.xml.Script.EnumConst;

namespace GasStation.xml.Script
{
    public class XmlClassLoader : XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы загрузчика
        /// </summary>
        public XmlClassLoaderConst LoaderConst { get; set; }

        /// <summary>
        /// Класс для отображения
        /// </summary>       
        public ClassLoaderView LoaderView { get; set; }



        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">Номер шага скрипта</param>
        /// <param name="xmlClassLoaderConst">Константы загрузчика</param>
        /// <param name="loaderView">Класс для отображения</param>
        public XmlClassLoader(int num, XmlClassLoaderConst xmlClassLoaderConst, ClassLoaderView loaderView)
        {
            TypeElement = TypeElement.Loader;

            LoaderConst = xmlClassLoaderConst;
            LoaderView = loaderView;

            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "loader";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrSetupSpeed = xmlDocument.CreateAttribute("setupSpeed");
            atrNum.Value = num.ToString();

            var atrDestination = xmlDocument.CreateAttribute("destLoader");
            atrDestination.Value = null;

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";
            #endregion

            #region Добавление атрибутов

            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrSetupSpeed);
            _xmlNode.Attributes.Append(atrDestination);
            _xmlNode.Attributes.Append(atrPriv);

            #endregion

            LoaderView.UsePriv = UsePriv;
        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для загрузчика для шага скрипта</param>
        /// <param name="xmlClassLoaderConst">Константы загрузчика</param>
        /// <param name="loaderView">Класс для отображения</param>
        public XmlClassLoader(XmlNode xmlNode, XmlClassLoaderConst xmlClassLoaderConst, ClassLoaderView loaderView) : base(xmlNode)
        {
            TypeElement = TypeElement.Loader;

            LoaderConst = xmlClassLoaderConst;
            LoaderView = loaderView;
            CmdResetLoader = new CommandClass(ResetLoader);
        }

        /// <summary>
        /// Установленное значение скорости в процентах
        /// </summary>
        public int SetupSpeed
        {
            get
            {
                int setupSpeed = 0;
                int.TryParse(XmlNode.Attributes["setupSpeed"].Value, out setupSpeed);
                return setupSpeed;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;

                if (value>100)
                {
                    value = 100;
                }

                XmlNode.Attributes["setupSpeed"].Value = value.ToString();
                PropertyIsChange("SetupSpeed");
            }
        }

        public CommandClass CmdResetLoader { get; set; }

        public void ResetLoader(object sender)
        {
            Destination = null;
        }

        public bool? Destination
        {
            get
            {
                bool dest;

                if (bool.TryParse(XmlNode.Attributes["destLoader"].Value,out dest))
                    return dest;

                return null;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;
                XmlNode.Attributes["destLoader"].Value = value.ToString();
                PropertyIsChange("Destination");
                PropertyIsChange("DestUnload");
                PropertyIsChange("DestLoad");
            }
        }

        public bool? DestLoad
        {
            get
            {
                if (Destination == null)
                    return false;

                return Destination;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;
                Destination =value;
                
                PropertyIsChange("Destination");
                PropertyIsChange("DestUnload");
                PropertyIsChange("DestLoad");
            }
        }

        public bool? DestUnload
        {
            get
            {
                if (Destination == null)
                    return false;

                   return !Destination;
            }
            set
            {
                if (!EnabledChangedScript)
                    return;
                Destination = !value;

                PropertyIsChange("Destination");
                PropertyIsChange("DestUnload");
                PropertyIsChange("DestLoad");

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
                if (!LoaderConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }

                XmlNode.Attributes["usePriv"].Value = value.ToString();
                PropertyIsChange("UsePriv");
            }
        }

        public override bool EnabledChangedScript => UsePriv || LoaderConst.EnabledChangedScript;
    }


}
