using GasStation.xml.Const.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GasStation.xml.Constant
{
    public class XmlBaseConst
    {
        public XmlBaseConst(XmlNode xmlNode)
        {
            XmlNode = xmlNode;
        }

        /// <summary>
        /// Узел Xml
        /// </summary>
        public XmlNode XmlNode { get; private set; }
        /// <summary>
        /// Номер РРГ в xml файле 1,2,3...
        /// </summary>
        public int DevNum
        {
            get
            {
                var devNum = Convert.ToInt32(XmlNode.Attributes["num"].Value);
                return devNum;
            }
        }

        /// <summary>
        /// Номер РРГ в массиве 0,1,2,3...
        /// </summary>
        public int DevNumMas
        {
            get
            {              
                return DevNum-1;
            }
        }
        public string RusName
        {
            get
            {
                var rusName = (XmlNode.Attributes["rusName"].Value);
                return rusName;
            }
        }

        public string DevName
        {
            get
            {
                var rusName = (XmlNode.Attributes["name"].Value);
                return rusName;
            }
        }

        public XmlClassSecuretyConst SecuretyConst
        {
            get
            {
                var xmlConst = XmlNode.SelectSingleNode("/settings/securety");
                var securety = new XmlClassSecuretyConst(xmlConst);
                return securety;
            }
        }

        public virtual bool EnabledChangedScript
        {
            get { return false; }
        }
    }
}
