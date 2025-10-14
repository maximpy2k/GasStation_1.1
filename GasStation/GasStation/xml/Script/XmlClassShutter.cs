using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script
{
    public class XmlClassShutter : XmlBaseClassElementScript
    {

        #region Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// Константы камеры
        /// </summary>
        public XmlClassShutterConst ShutterConst { get; set; }

        /// <summary>
        /// Класс для отображения
        /// </summary>       
        public ClassShutterView ShutterView { get; set; }


        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">Номер шага скрипта</param>
        /// <param name="xmlClassChamberConst">Константы камеры</param>
        /// <param name="chamberView">Класс для отображения</param>
        public XmlClassShutter(int num, XmlClassShutterConst xmlClassShutterConst, ClassShutterView shutterView)
        {
            TypeElement=TypeElement.Shutter;

            ShutterConst = xmlClassShutterConst;
            ShutterView = shutterView;

            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            for (int i = 0; i < xmlClassShutterConst.Flaps.Length; i++)
            {
                var fl = new XmlClassFlap(i + 1, xmlClassShutterConst.Flaps[i], ShutterView.FlapView[i]);
                var importNode = xmlDocument.ImportNode(fl.XmlNode, true);
                _xmlNode.AppendChild(importNode);
            }

            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = "shutter";

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();

            var atrPriv = xmlDocument.CreateAttribute("usePriv");
            atrPriv.Value = "False";
            #endregion

            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            _xmlNode.Attributes.Append(atrPriv);
            #endregion

        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для камеры для шага скрипта</param>
        /// <param name="xmlClassChamberConst">Константы камеры</param>
        /// <param name="chamberView">Класс для отображения</param>
        public XmlClassShutter(XmlNode xmlNode, XmlClassShutterConst xmlClassShutterConst, ClassShutterView shutterView) : base(xmlNode)
        {
            TypeElement = TypeElement.Shutter;

            ShutterConst = xmlClassShutterConst;
            ShutterView = shutterView;
        }

        private XmlClassFlap[] flaps;
        /// <summary>
        /// Клапаны
        /// </summary>
        public XmlClassFlap[] Flaps
        {
            get
            {
                if (flaps != null)
                    return flaps;

                var nodes = XmlNode.SelectNodes("dev[@name='flap']");
                flaps = new XmlClassFlap[nodes.Count];
                for (int i = 0; i < nodes.Count; i++)
                    flaps[i] = new XmlClassFlap(nodes[i], ShutterConst.Flaps[i], ShutterView.FlapView[i]);

                return flaps;
            }
        }

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
                if (!ShutterConst.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.MegaBoss))
                {
                    MessageBox.Show(@"У вас нет привилегии MegaBoss");
                    return;
                }

                XmlNode.Attributes["usePriv"].Value = value.ToString();

                var node = XmlNode.SelectNodes("dev[@name='flap']");

                for(int index = 0;index < node.Count;index++)
                {
                    node[index].Attributes["usePriv"].Value = value.ToString();
                }

                

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UsePriv"));
            }
        }

        /// <summary>
        /// Возможность изменнеия скрипта
        /// </summary>
        public override bool EnabledChangedScript
        {
            get { return UsePriv || ShutterConst.EnabledChangedScript; }
        }
    }
}
