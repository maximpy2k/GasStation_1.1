using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GasStation.xml.Constant.XmlConst.Elements
{
    public class XmlClassProgrammConst:XmlBaseConst
    {
        public XmlClassProgrammConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        public int Port
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='Port']").Attributes["value"].Value, out val);
                return val;
            }
            set
            {
                this.Port = value;
            }
        }

        public int Baudrate
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='Baudrate']").Attributes["value"].Value, out val);
                return val;
            }
        }

        public int Databits
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='Databits']").Attributes["value"].Value, out val);
                return val;
            }
        }

        public String Parity
        {
            get
            {
                var val = XmlNode.SelectSingleNode("EditFild[@name='Parity']").Attributes["value"].Value;
                return val;
            }
        }

        public String Stopbits
        {
            get
            {
                var val = XmlNode.SelectSingleNode("EditFild[@name='Stopbits']").Attributes["value"].Value;

                return val;
            }
        }

        public String Handshake
        {
            get
            {
                var val = XmlNode.SelectSingleNode("EditFild[@name='Handshake']").Attributes["value"].Value;

                return val;
            }
        }

        public String CR
        {
            get
            {

                var val = XmlNode.SelectSingleNode("EditFild[@name='CR']").Attributes["value"].Value;

                return val.Replace("//", "/");

            }
        }

        public String LF
        {
            get
            {
                var val = XmlNode.SelectSingleNode("EditFild[@name='LF']").Attributes["value"].Value;

                return val.Replace("//", "/"); ;
            }
        }
    }
}
