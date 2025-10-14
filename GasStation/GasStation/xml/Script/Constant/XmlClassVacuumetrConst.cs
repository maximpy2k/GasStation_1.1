using System;
using System.Linq;
using System.Xml;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Script.Security;
using GasStation.Mathem;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassVacuumetrConst:XmlBaseConst
    {
        public XmlClassVacuumetrConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        /// <summary>
        /// Наличие ДИО в константах
        /// </summary>
        public string EnableOut => DioConst == null ? "Collapsed" : "Visible";

        /// <summary>
        /// Список настроек для АЦП
        /// </summary>
        public XmlClassAcpConst AcpConst
        {
            get
            {
                var nodes = XmlNode.SelectSingleNode("dev[@name='acp']");
                if (nodes == null)
                    return null;

                return  new XmlClassAcpConst(nodes);
            }
        }

        /// <summary>
        /// Список настроек для ЦП
        /// </summary>
        public XmlClassDioPortConst DioConst
        {
            get
            {
                var nodes = XmlNode.SelectSingleNode("dev[@name='dio']");
                if (nodes == null)
                    return null;

                return new XmlClassDioPortConst(nodes);
            }
        }
        /// <summary>
        /// Условие
        /// </summary>
        public Conditions Condition
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='properties']/EditFild[@name='condition']");
                if (node == null)
                    return Conditions.equally;
                var s = node.Attributes["value"].Value;

                var cond = Conditions.equally;

                Enum.TryParse(s, out cond);
                return cond;
            }
        }
        /// <summary>
        /// Пороговое значение
        /// </summary>
        public double ThresholdPress
        {
            get
            {
                var node = XmlNode.SelectSingleNode("dev[@name='properties']/EditFild[@name='thresholdPress']");
                if (node == null)
                    return 0.0;

                var s = node.Attributes["value"].Value;
                var thresholdPress = 0.0;
                double.TryParse(s, out thresholdPress);
                return thresholdPress;
            }
        }
        /// <summary>
        /// Проверка условия
        /// </summary>
        /// <param name="val">Значение</param>
        /// <returns>Результат</returns>
        public bool CheckCondition(double val)
        {
            switch(Condition)
            {
                case Conditions.more:
                    return val > ThresholdPress;
                case Conditions.less:
                    return val < ThresholdPress;
                case Conditions.equally:
                    return val == ThresholdPress;

                case Conditions.moreOrEqually:
                    return (val > ThresholdPress || val == ThresholdPress);
                case Conditions.lessOrEqually:
                    return (val < ThresholdPress || val == ThresholdPress);
                default:
                    return false;
            }
            
        }
    }
}

