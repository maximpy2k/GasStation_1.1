using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using GasStation.xml.Script.EnumConst;
using System.ComponentModel;

namespace GasStation.xml.Script
{
    public class XmlBaseClassElementScript:INotifyPropertyChanged
    {
        #region Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Изменение значения property
        /// </summary>
        /// <param name="propName">Имя property</param>
        public void PropertyIsChange(string propName)
        {
            PropertyChanged?.Invoke(this, new  PropertyChangedEventArgs(propName));
        }
        #endregion

        public XmlBaseClassElementScript(XmlNode node)
        {
            _xmlNode = node;
        }

        public XmlBaseClassElementScript() { }

        protected XmlNode _xmlNode;
        /// <summary>
        /// Узел Xml
        /// </summary>
        public XmlNode XmlNode => _xmlNode;

        public TypeElement TypeElement;

        /// <summary>
        /// Номер узла
        /// </summary>
        public virtual int Num
        {
            get
            {
                var num = -1;
                if (XmlNode.Attributes["num"] == null)
                {
                    Console.WriteLine("Нет атрибута num");
                    return 1;
                }
                int.TryParse(XmlNode.Attributes["num"].Value, out num);
                return num;
            }
            set
            {
                XmlNode.Attributes["num"].Value=value.ToString();
            }
        }        

        /// <summary>
        /// Название узла
        /// </summary>
        public virtual string Name
        {
            get
            {
                var name = XmlNode.Attributes["name"].Value;

                return name;

            }
            set
            {
                XmlNode.Attributes["name"].Value=value.ToString();
            }
        }

         public virtual bool UsePriv
        {
            get
            {
                return false;
            }
            set
            {
                
            }
        }
        /// <summary>
        /// Разрешение изменять  поля скрипта 
        /// </summary>
        public virtual bool EnabledChangedScript { get; set; }
        /// <summary>
        /// Блокировка полей для ввода при недостатке прав
        /// </summary>
        public bool ReadOnly
        {
            get
            {
                return !EnabledChangedScript;
            }
        }
    }
}
