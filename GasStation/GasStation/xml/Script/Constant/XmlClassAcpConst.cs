
using System.Linq;
using GasStation.xml.Constant;
using System.Security.Authentication.ExtendedProtection;
using System.Xml;
using GasStation.xml.Script.Security;
using GasStation.xml.Constant.XmlConst;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassAcpConst:XmlBaseConst
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode"></param>
        public XmlClassAcpConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        /// <summary>
        /// Подадрес контроллера
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
        /// Номер порта на контроллере
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
        /// Функция преобразования кода АЦП в значение
        /// </summary>
        /// <param name="code">Код АЦП</param>
        /// <returns></returns>
        public double GetValue(double code)
        {
            return Table.GetVal(code);
        }
        /// <summary>
        /// Таблица преобразования кода АЦП в значение
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
