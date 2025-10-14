using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst;
using System.Xml;


namespace GasStation.xml.Const.Elements
{
    public class XmlClassCapConst:XmlBaseConst
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode"></param>
        public XmlClassCapConst(XmlNode xmlNode):base(xmlNode)
        {
        }
        /// <summary>
        /// Номер контроллера
        /// </summary>
        public int ContrNum
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='contrNum']").Attributes["value"].Value, out val);
                return val;
            }
        }
        /// <summary>
        /// порт
        /// </summary>
        public int Port
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='port']").Attributes["value"].Value, out val);
                return val;
            }
        }

        /// <summary>
        /// Функция преобразования значения в код ЦАП
        /// </summary>
        /// <param name="val">Код ЦАП</param>
        /// <returns>Напряжение ЦАП</returns>
        public double GetValue(double val)
        {
            var u = Table.GetVal(val);
            return (u >= 0) ? u : 0;
        }

        /// <summary>
        /// Таблица преобразования значения в код ЦАП
        /// </summary>
        public XmlTableConst Table
        {
            get
            {
                var node = XmlNode.SelectSingleNode("table[@name='Tab']");
                if (node == null)
                    return null;
                return new XmlTableConst(node);
            }
        }
    }
}
