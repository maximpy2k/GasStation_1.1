using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using GasStation.xml.Script.Constant;

namespace GasStation.ViewModels.Elements
{
    public class ClassSensorView : INotifyPropertyChanged
    {
        /// <summary>
        /// Константы
        /// </summary>
        public ClassSensorView(XmlClassSensorConst con)
        {
            _const = con;
        }
        /// <summary>
        /// Константы
        /// </summary>
        private XmlClassSensorConst _const;

        private bool _sensorValue=false;
        /// <summary>
        /// Показание датчика
        /// </summary>
        public bool SensorValue
        {
            get
            {
                return _sensorValue;
            }
            set
            {
                _sensorValue = !_const.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SensorValue"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;        
    }
}
