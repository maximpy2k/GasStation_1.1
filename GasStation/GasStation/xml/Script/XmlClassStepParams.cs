using System;
using System.Xml;
using GasStation.xml.Script.EnumConst;
using System.ComponentModel;
using System.Linq;
using GasStation.xml.Constant;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script.XmlScript.Elements
{
    /// <summary>
    /// Xml узел шаг скрипта
    /// </summary>
    public class XmlClassStepParams:INotifyPropertyChanged
    {
        #region Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="numStep">Номер шага</param>
        /// <param name="consts">Константы</param>
        public XmlClassStepParams(int numStep, XmlClassConst consts)
        {
            var xmlDocument = new XmlDocument();
            xmlNode = xmlDocument.CreateElement("params");

            _consts = consts;

            _numStep = numStep;

            #region Создание атрибутов

            var atrTime = xmlDocument.CreateAttribute("time");
            atrTime.Value = "0";

            var atrNumStep = xmlDocument.CreateAttribute("numStep");
            atrNumStep.Value = numStep.ToString();

            var atrNameStep = xmlDocument.CreateAttribute("nameStep");
            atrNameStep.Value = (atrNumStep.Value == "1")?"Ожидание":"";
            

            var atrTypeStep = xmlDocument.CreateAttribute("typeStep");
            atrTypeStep.Value = ("Normal");

            #endregion                           

            #region Добавление атрибутов
            xmlNode.Attributes.Append(atrTime);
            xmlNode.Attributes.Append(atrNumStep);
            xmlNode.Attributes.Append(atrNameStep);
            xmlNode.Attributes.Append(atrTypeStep);
            #endregion
        }
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="xmlNode">Нода</param>
        /// <param name="consts">Константы</param>
        public XmlClassStepParams(XmlNode xmlNode, XmlClassConst consts)
        {
            _consts = consts;

            this.xmlNode = xmlNode;
        }

        /// <summary>
        /// Нода для разбора
        /// </summary>
        private readonly XmlNode xmlNode;
        /// <summary>
        /// Узел Xml
        /// </summary>
        public XmlNode XmlNode => xmlNode;

        /// <summary>
        /// Константы
        /// </summary>
        private readonly XmlClassConst _consts;
        /// <summary>
        /// Путь к логу шага
        /// </summary>
        public string PathToLog => _consts.ChannelConsts.LogPathFull;

        private int _numStep;

        /// <summary>
        /// Время шага
        /// </summary>
        public TimeSpan TimeStep
        {
            get
            {
                int val = 0;

                var res = new TimeSpan();

                if (int.TryParse(XmlNode.Attributes["time"].Value, out val))
                {
                    var result = (xmlNode.Attributes["time"].Value);

                    val = int.Parse(result);

                    res = new TimeSpan(0, 0, val);
                }
                else
                {
                    TimeSpan.TryParse(XmlNode.Attributes["time"].Value, out res);
                }

                return res;
            }
            set
            {
                var time = value.TotalHours > 23.99999 ? (new TimeSpan(23,59,59)):value;

                xmlNode.Attributes["time"].Value = time.ToString();

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeStepH"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeStepM"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimeStepS"));
            }
        }

        /// <summary>
        /// Время шага в часах
        /// </summary>
        public int TimeStepH
        {
            get
            {
                return TimeStep.Hours;// TimeStep /3600;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                if (NumStep == 1)
                {
                    return;
                }

                TimeStep = new TimeSpan(value,TimeStep.Minutes,TimeStep.Seconds);
            }
        }

        /// <summary>
        /// Время шага в минутах
        /// </summary>
        public int TimeStepM
        {
            get
            {
                //var time = TimeStep;
                return TimeStep.Minutes;//(time - TimeStepH * 3600)/60;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                if (NumStep == 1)
                {
                    return;
                }

                TimeStep = new TimeSpan(TimeStep.Hours, value, TimeStep.Seconds);
            }
        }

        /// <summary>
        /// Время шага в секундах
        /// </summary>
        public int TimeStepS
        {
            get
            {

                return TimeStep.Seconds;// TimeStep - TimeStepH*3600 - TimeStepM*60;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                if (NumStep == 1)
                {
                    return;
                }

                TimeStep = new TimeSpan(TimeStep.Hours, TimeStep.Minutes, value);
            }
        } 

        /// <summary>
        /// Описание шага стрипта
        /// </summary>
        public string NameStep
        {
            get
            {
                return (xmlNode.Attributes["nameStep"].Value);
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                if (NumStep == 1)
                    return;
                xmlNode.Attributes["nameStep"].Value=value.ToString();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NameStep"));
            }
        }

        /// <summary>
        /// Номер комманды скрипта
        /// </summary>
        public int NumStep
        {
            get
            {
                int val = -1;
                int.TryParse(XmlNode.Attributes["numStep"].Value, out val);
                return val;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }

                XmlNode.Attributes["numStep"].Value=value.ToString();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("NumStep"));
            }
        }

        /// <summary>
        /// Тип шага
        /// </summary>
        public RegimsStep TypeStep
        {
            get
            {
                var val = XmlNode.Attributes["typeStep"].Value;
                var val1=(RegimsStep)(Enum.Parse(typeof(RegimsStep),val));

                return val1;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypeStep"));
                    return;
                }

                if (NumStep == 1)
                {
                    XmlNode.Attributes["typeStep"].Value = XmlNode.Attributes["typeStep"].Value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypeStep"));
                    return;
                }

                XmlNode.Attributes["typeStep"].Value = value.ToString();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TypeStep"));
            }
        }

        /// <summary>
        /// Возможность изменнеия скрипта
        /// </summary>
        public bool EnabledChangedScript => 
            UsePriv || _consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeScript);

        /// <summary>
        /// Пользовательские привелегии
        /// </summary>
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
    }
}
