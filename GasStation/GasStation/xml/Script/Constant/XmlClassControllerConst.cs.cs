using System;
using System.Xml;

namespace GasStation.xml.Constant.XmlConst.Elements
{
    public class XmlClassControllerConst:XmlBaseConst
    {
        /// <summary>
        /// Узел Xml
        /// </summary>
        public XmlClassControllerConst(XmlNode xmlNode):base(xmlNode)
        {
        }

        public int Pa
        {
            get
            {
                var val = -1;
                int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='pa']").Attributes["value"].Value, out val);
                return val;
            }
        }

        public int DefaultState
        {
            get
            {
                if (XmlNode.SelectSingleNode("EditFild[@name='DefaultState']") == null)
                    return 0;
                if (XmlNode.SelectSingleNode("EditFild[@name='DefaultState']").Attributes["value"] == null)
                    return 0;
                var val =Convert.ToInt32(XmlNode.SelectSingleNode("EditFild[@name='DefaultState']").Attributes["value"].Value);

                return val;
            }
        }

        public String NameController
        {
            get
            {
                var val = XmlNode.SelectSingleNode("EditFild[@name='name']").Attributes["value"].Value;

                return val;
            }


        }

        public int CountReads
        {
            get
            {
               
                int val = -1;
                try
                {
                    if (XmlNode.SelectSingleNode("EditFild[@name='countReads']") != null)
                        int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='countReads']").Attributes["value"].Value,
                            out val);
                    else
                        val = 10;

                }
                catch
                {
                    return 10;
                }
                    
                return val;
            }
        }

        public String WDT
        {
            get
            {
                var val = -1.0;
                try
                {
                    if (XmlNode.SelectSingleNode("EditFild[@name='wdt']") == null)
                        return "50";
                    double.TryParse(XmlNode.SelectSingleNode("EditFild[@name='wdt']").Attributes["value"].Value, out val);
                }
                catch
                {
                    return "50";
                }

                
                if (val * 10 > 255)
                    return "FF";
                if (val * 10 < 1 )
                    return "01";

                return (Convert.ToString((int)(val * 10), 16)).ToUpper();
            }
        }

        public int DelayRead
        {
            get
            {
                var val = 30;

                if (XmlNode.SelectSingleNode("EditFild[@name='delayRead']") == null)
                    return val;
                try
                {
                    int.TryParse(XmlNode.SelectSingleNode("EditFild[@name='delayRead']").Attributes["value"].Value, out val);
                }
                catch
                {
                    return 30;
                }
                
                return val;
            }
        }

        public bool WriteLog
        {
            get
            {
                var val = true;

                if (XmlNode.SelectSingleNode("EditFild[@name='writeLog']") == null)
                    return val;
                try
                {
                    bool.TryParse(XmlNode.SelectSingleNode("EditFild[@name='writeLog']").Attributes["value"].Value, out val);
                }
                catch
                {
                    return true;
                }

                return val;
            }
        }
    }
}
