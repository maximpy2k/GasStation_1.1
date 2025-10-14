using GasStation.xml.Script.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.UserControls.Elements
{
    public class ClassGateView : INotifyPropertyChanged
    {
        /// <summary>
        /// Константы загрузчика
        /// </summary>
        public XmlClassGateConst _const { get; set; }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="con">Константы загрузчика</param>
        public ClassGateView(XmlClassGateConst con)
        {
            _const = con;
        }
        public bool UsePriv { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        bool openGateStatus;
        /// <summary>
        /// Статус открытой заслонки
        /// </summary>
        public bool OpenGateStatus
        {
            get { return openGateStatus; }
            set
            {
                openGateStatus = !_const.DioGateOpenConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OpenGateStatus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GateStatus"));
            }
        }

        bool closeGateStatus;
        /// <summary>
        /// Статус закрытой заслонки
        /// </summary>
        public bool CloseGateStatus
        {
            get { return closeGateStatus; }
            set
            {
                closeGateStatus = !_const.DioGateCloseConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CloseGateStatus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GateStatus"));
            }
        }

        bool gateStatus;
        /// <summary>
        /// Статус закрытой заслонки
        /// </summary>
        public bool GateStatus
        {
            get { return closeGateStatus; }
            set
            {
                closeGateStatus = !_const.DioGateCloseConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GateStatus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OpenGateStatus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CloseGateStatus"));
            }
        }
    }
}
