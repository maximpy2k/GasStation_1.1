using GasStation.xml.Constant.XmlConst;
using System;
using System.Xml;

namespace GasStation.xml.Constant
{
    public class XmlClassDOConst:XmlBaseConst
    {
        public XmlClassDOConst(XmlNode xmlNode):base(xmlNode)
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
        /// Получение кода соответствующего значению
        /// </summary>
        /// <param name="arg">значение</param>
        /// <returns>код</returns>
        public int GetValue(double arg) => (int)Table.GetVal(arg);


        /// <summary>
        /// Таблица преобразования значения в код
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
