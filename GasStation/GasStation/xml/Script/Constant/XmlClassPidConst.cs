using System.Linq;
using GasStation.xml.Constant;
using System.Xml;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Const.Elements
{
    public class XmlClassPidConst:XmlBaseConst
    {
        public XmlClassPidConst(XmlNode xmlNode) : base(xmlNode)
        {
        }

        /// <summary>
        /// Коэффициент усиления пропорциональной составляющей ПИД
        /// </summary>
        public double Kp
        {
            get
            {
                double val = -1;
                double.TryParse(XmlNode.SelectSingleNode("EditFild[@name='Kp']").Attributes["value"].Value, out val);
                return val;
            }
            set
            {
                XmlNode.SelectSingleNode("EditFild[@name='Kp']").Attributes["value"].Value = value.ToString();
            }
        }

        /// <summary>
        /// Постоянная времени дифференцирования, с
        /// </summary>
        public double Td
        {
            get
            {
                double val = -1;
                double.TryParse(XmlNode.SelectSingleNode("EditFild[@name='Td']").Attributes["value"].Value, out val);
                return val;
            }
            set
            {

                XmlNode.SelectSingleNode("EditFild[@name='Td']").Attributes["value"].Value = value.ToString();
            }

        }

        /// <summary>
        /// Постоянная времени интегрирования, с
        /// </summary>
        public double Ti
        {
            get
            {
                double val = -1;
                double.TryParse(XmlNode.SelectSingleNode("EditFild[@name='Ti']").Attributes["value"].Value, out val);
                return val;
            }
            set
            {

                XmlNode.SelectSingleNode("EditFild[@name='Ti']").Attributes["value"].Value = value.ToString();
            }
        }

        /// <summary>
        /// Область допустимых значений ошибки управления
        /// </summary>
        public double Di
        {
            get
            {
                double val = -1;
                double.TryParse(XmlNode.SelectSingleNode("EditFild[@name='Di']").Attributes["value"].Value, out val);
                return val;
            }
            set
            {

                XmlNode.SelectSingleNode("EditFild[@name='Di']").Attributes["value"].Value = value.ToString();
            }
        }

        public double MaxImpact
        {
            get
            {
                double val = -1;
                double.TryParse(XmlNode.SelectSingleNode("EditFild[@name='MaxImpact']").Attributes["value"].Value, out val);
                return val;
            }
            set
            {

                XmlNode.SelectSingleNode("EditFild[@name='MaxImpact']").Attributes["value"].Value = value.ToString();
            }
        }

        public override bool EnabledChangedScript => SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangePid);
    }
}
