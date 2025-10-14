using System;
using System.Xml;
using GasStation.xml.Const.Elements;
using System.ComponentModel;
using System.Linq;
using GasStation.ViewModels.Elements;
using System.Windows;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;
using GasStation.xml.Script.Constant;

namespace GasStation.xml.Script
{
    /// <summary>
    /// Xml узел датчик
    /// </summary>
    public class XmlClassSensor:XmlBaseClassElementScript
    {
        /// <summary>
        /// Константы датчика
        /// </summary>
        public XmlClassSensorConst SensorConst { get; set; }
        /// <summary>
        /// Класс для отображения
        /// </summary>
        public ClassSensorView SensorView { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="num">номер РРГ</param>
        /// <param name="rrgConst">Константы РРГ</param>
        /// <param name="rrgView">Класс для отображения</param>
        public XmlClassSensor(int num, XmlClassSensorConst sensorConst, ClassSensorView sensorView)
        {

            TypeElement = TypeElement.Rrg;
            SensorConst = sensorConst;
            SensorView = sensorView;
            var xmlDocument = new XmlDocument();
            _xmlNode = xmlDocument.CreateElement("dev");

            
            #region Создание атрибутов
            var atrName = xmlDocument.CreateAttribute("name");
            atrName.Value = sensorConst.DevName;

            var atrNum = xmlDocument.CreateAttribute("num");
            atrNum.Value = num.ToString();
            #endregion               
            #region Добавление атрибутов
            _xmlNode.Attributes.Append(atrName);
            _xmlNode.Attributes.Append(atrNum);
            #endregion
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlRrgNode">XML узел с заданием для РРГ для шага скрипта</param>
        /// <param name="rrgConst">Константы РРГ</param>
        /// <param name="rrgView">Класс для отображения</param>
        public XmlClassSensor(XmlNode xmlSensorNode, XmlClassSensorConst sensorConst, ClassSensorView sensorView) :base(xmlSensorNode)
        {
            TypeElement = TypeElement.Sensor;
            SensorConst = sensorConst;
            SensorView = sensorView;
        }     
      
        /// <summary>
        /// Строковая подпись датчика
        /// </summary>
        public string RrgNum => $"Датчик №{Num}";
        
    }
}
