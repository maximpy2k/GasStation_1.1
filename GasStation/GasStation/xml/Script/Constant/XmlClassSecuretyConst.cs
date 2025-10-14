using System.Xml;
using GasStation.xml.Const.Elements;
using System.ComponentModel;
using System.Linq;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;

namespace GasStation.xml.Const.Elements
{
    /// <summary>
    /// Xml узел камеры
    /// </summary>
    public class XmlClassSecuretyConst:INotifyPropertyChanged
    {
        public XmlNode _xmlNode;
        public XmlNode XmlNode => _xmlNode;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="xmlNode">XML узел с заданием для камеры для шага скрипта</param>        
        public XmlClassSecuretyConst(XmlNode xmlNode)
        {
            _xmlNode = xmlNode;
        }
        string name = "User";

        /// <summary>
        /// Имя выбранного пользователя
        /// </summary>
        public string CurrUserName
        {
            get
            {
                //return name;
                return XmlNode.Attributes["user"].Value;
            }
            set
            {
                name = value;
                XmlNode.Attributes["user"].Value = value;
                XmlNode.Attributes["rusName"].Value = CurrUser.RusName;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrUserName"));
            }
        }
        public XmlClassUserConst CurrUser
        {
            get
            {
                return Users.First(user => user.UserName == CurrUserName);
                //return XmlNode.Attributes["user"].Value;
            }
        }


        public XmlClassUserConst[] _users;

        public event PropertyChangedEventHandler PropertyChanged;


        public XmlClassUserConst[] Users
        {
            get
            {
                if (_users != null)
                    return _users;

                var nodes = XmlNode.SelectNodes("user");
                _users = new XmlClassUserConst[nodes.Count];
                for (int i = 0; i < nodes.Count; i++)
                    _users[i] = new XmlClassUserConst(nodes[i]);

                return _users;
            }
        }        
    }
}
