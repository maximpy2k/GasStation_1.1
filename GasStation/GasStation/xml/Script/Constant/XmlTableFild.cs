using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GasStation.xml.Constant
{
    public class XmlTableFild : INotifyPropertyChanged
    {
        public XmlTableFild(double x, double y, bool check)
        {
            X = x;
            Y = y;
            Check = check;
        }

        private double x;
        /// <summary>
        /// Значение по оси X
        /// </summary>
        public double X
        {
            get { return x; }
            set
            {
                x = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("X"));
            }
        }

        private double y;
        /// <summary>
        /// Значение по оси X
        /// </summary>
        public double Y
        {
            get { return y; }
            set
            {
                y = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Y"));
            }
        }

        private bool check;
        /// <summary>
        /// Check
        /// </summary>
        public bool Check
        {
            get { return check; }
            set
            {
                check = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Check"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Коэффициент пропорциональности заданию
        /// </summary>
        public XmlNode Node
        {
            get
            {
                var xmlDocument = new XmlDocument();
                var xmlNode = xmlDocument.CreateElement("field");

                #region Создание атрибутов
                var atrValueX = xmlDocument.CreateAttribute("valueX");
                atrValueX.Value = X.ToString();

                var atrValueY = xmlDocument.CreateAttribute("valueY");
                atrValueY.Value = Y.ToString();

                var atrCheck = xmlDocument.CreateAttribute("check");
                atrCheck.Value = Y.ToString();

                var atrRusNameX = xmlDocument.CreateAttribute("rusNameX");
                atrRusNameX.Value = "T уст";

                var atrRusNameY = xmlDocument.CreateAttribute("rusNameY");
                atrRusNameY.Value = "T наг";

                var atrDefX = xmlDocument.CreateAttribute("defX");
                atrDefX.Value = "Значение температуры начала интервала корректировочного графика внутри секции термокамеры";

                var atrDefY = xmlDocument.CreateAttribute("defY");
                atrDefY.Value = "Значение температуры начала интервала корректировочного графика между витками нагревательного элемента";
                #endregion

                #region Добавление атрибутов

                xmlNode.Attributes.Append(atrValueX);
                xmlNode.Attributes.Append(atrValueY);
                xmlNode.Attributes.Append(atrCheck);
                xmlNode.Attributes.Append(atrRusNameX);
                xmlNode.Attributes.Append(atrRusNameY);
                xmlNode.Attributes.Append(atrDefX);
                xmlNode.Attributes.Append(atrDefY);

                #endregion

                return xmlNode;
            }
        }
    }
}
