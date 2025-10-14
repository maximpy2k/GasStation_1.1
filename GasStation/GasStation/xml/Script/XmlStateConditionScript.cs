using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Xml;
using GasStation.xml.Constant;
using GasStation.xml.Script.Security;

namespace GasStation.xml.Script
{
    public class XmlStateConditionScript
    {
        private readonly XmlNode xmlNode;
        private XmlClassConst _consts;

        /// <summary>
        /// Узел Xml
        /// </summary>
        public XmlNode XmlNode => xmlNode;

        public XmlStateConditionScript(XmlNode xmlNode, XmlClassConst consts)
        {
            this.xmlNode = xmlNode;

            _consts = consts;
        }

        public XmlStateConditionScript(XmlClassConst consts)
        {
            _consts = consts;


            var xmlDocument = new XmlDocument();
            xmlNode = xmlDocument.CreateElement("state");


            #region Создание атрибутов

            //var atrName = xmlDocument.CreateAttribute("devName");
            //atrName.Value = devName;

            //var atrNum = xmlDocument.CreateAttribute("devNum");
            //atrNum.Value = devNum.ToString();

            //var atrTypeDevice = xmlDocument.CreateAttribute("typeDevice");
            //atrTypeDevice.Value = typeDevice.ToString();

            var atrValue = xmlDocument.CreateAttribute("value");
            atrValue.Value = 0.ToString();

            var atrDevice = xmlDocument.CreateAttribute("device");
            atrDevice.Value = 0.ToString();

            var atrTimer = xmlDocument.CreateAttribute("timer");
            atrTimer.Value = 0.ToString();

            //var atrNumStep = xmlDocument.CreateAttribute("numStep");
            //atrNumStep.Value = 0.ToString();

            var atrNameStep = xmlDocument.CreateAttribute("nameStep");
            atrNameStep.Value = 0.ToString();

            var atrCurState = xmlDocument.CreateAttribute("curState");
            atrCurState.Value = 0.ToString();



            #endregion               
            #region Добавление атрибутов
            xmlNode.Attributes.Append(atrDevice);
            xmlNode.Attributes.Append(atrCurState);
            xmlNode.Attributes.Append(atrValue);
            xmlNode.Attributes.Append(atrTimer);
            xmlNode.Attributes.Append(atrNameStep);

            #endregion
        }

        
        /// <summary>
        /// Конецчное значение когда должен быть переход
        /// </summary>
        public double ValueEnd => CurState + ValueAbs;

        /// <summary>
        /// Начальное значение, когда олжен быть переход
        /// </summary>
        public double ValueBegin => CurState - ValueAbs;


        /// <summary>
        /// Граница значений для условного перехода
        /// </summary>
        public double ValueAbs
        {
            get
            {
                double val = 0;

                double.TryParse(XmlNode.Attributes["value"].Value, out val);

                var result = (xmlNode.Attributes["value"].Value);

                var a = double.Parse(result);
                return a;

            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                Setting("value", value.ToString());
            }
        }


        /// <summary>
        /// Устройство
        /// </summary>
        public string Device
        {
            get { return xmlNode.Attributes["device"].Value; }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                xmlNode.Attributes["device"].Value = value;
            }
        }

        public int NumState { get; set; }

        /// <summary>
        /// Значение состояния
        /// </summary>
        public double CurState
        {
            get
            {
                double val = 0;

                double.TryParse(XmlNode.Attributes["curState"].Value, out val);

                var result = (xmlNode.Attributes["curState"].Value);

                var a = double.Parse(result);
                return a;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                xmlNode.Attributes["curState"].Value = value.ToString();
            }
        }

        /// <summary>
        /// Номер шага для которого используется условный переход
        /// </summary>
        public int NumStep
        {
            get
            {
                if (NameStep == "0")
                {
                    MessageBox.Show("Вы выбрали шаг ожидание для условного перехода");
                    return 0;
                }

                var indexZ = NameStep.LastIndexOf(" ");

                var numStepStr = NameStep.Remove(indexZ, NameStep.Length -indexZ);

                var numStep = -1;

                try
                {

                    numStep = Convert.ToInt32(numStepStr);

                }
                catch (FormatException excp)
                {
                    indexZ = NameStep.IndexOf(" ");
                    numStepStr = NameStep.Remove(indexZ, NameStep.Length - indexZ);

                    numStep = Convert.ToInt32(numStepStr);
                }

                return numStep;
            }
            //set { Setting("numStep",value.ToString()); }
        }

        public string NameStep
        {
            get
            {
                //string val = 0;

                //int.TryParse(XmlNode.Attributes["nameStep"].Value, out val);

                var result = (XmlNode.Attributes["nameStep"].Value);

                //var a = int.Parse(result);
                return result;
            }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                xmlNode.Attributes["nameStep"].Value = value;
            }
        }

        /// <summary>
        /// Время
        /// </summary>
        public int Timer
        {
            get { return Getting("timer"); }
            set
            {
                if (!EnabledChangedScript)
                {
                    return;
                }
                Setting("timer",value.ToString());
            }
        }

        private int Getting(string name)
        {
            int val = 0;

            int.TryParse(XmlNode.Attributes[name].Value, out val);

            var result = (xmlNode.Attributes[name].Value);

            var a = int.Parse(result);
            return a;
        }

        /// <summary>
        /// Имя устройства
        /// </summary>
        public string DevName
        {
            get
            {
                var device = Device;

                var indexZ = device.LastIndexOf(" ");

                var devName = device.Remove(indexZ, device.Length - indexZ);

                var res = "";

                for(int index = 0; index < indexZ; index++)
                {
                    res += devName[index];
                }


                return res;
            }
        }

        /// <summary>
        /// Номер устройства
        /// </summary>
        public int DevNum
        {
            get
            {
                var device = Device;

                var indexZ = device.LastIndexOf(" ");

                var devNumStr = device.Remove(0, indexZ);

                var devNum = Convert.ToInt32(devNumStr);

                return devNum;
            }
        }

        private void Setting(string name,string value)
        {
            xmlNode.Attributes[name].Value = value;
        }

        /// <summary>
        /// Возможность изменнеия скрипта
        /// </summary>
        public bool EnabledChangedScript => UsePriv || _consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeConditionalScript);

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
