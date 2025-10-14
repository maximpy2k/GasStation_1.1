using System.Xml;
using GasStation.xml.Script.Security;
using System.Collections.Generic;
using System;

namespace GasStation.xml.Const.Elements
{
    /// <summary>
    /// Xml узел камеры
    /// </summary>
    public class XmlClassUserConst
    {
        /// <summary>
        /// Пользовательские привилегии
        /// </summary>
        public EnumPriv[] Privs
        {
            get
            {
                var lstPrivs = new List<EnumPriv>() { EnumPriv.None };
                
                var nodes=XmlNode.SelectNodes("priv");
                if (nodes.Count == 0)
                    return new[] { EnumPriv.None };
                for (int i = 0; i < nodes.Count; i++)
                {
                    var priv = EnumPriv.None;
                    if (Enum.TryParse(nodes[i].Attributes["privName"].Value, false, out priv))
                        lstPrivs.Add(priv);
                }
                return lstPrivs.ToArray();
            }
        }

        private XmlNode _xmlNode;
        public XmlNode XmlNode => _xmlNode;
     

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для камеры для шага скрипта</param>        
        public XmlClassUserConst(XmlNode xmlNode)
        {
            _xmlNode = xmlNode;
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get
            {
                return XmlNode.Attributes["pass"].Value;
            }
            set
            {
                XmlNode.Attributes["pass"].Value = value;
            }
        }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string UserName
        {
            get
            {
                return XmlNode.Attributes["userName"].Value;
            }
            set
            {
                XmlNode.Attributes["userName"].Value = value;
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
    }
}
