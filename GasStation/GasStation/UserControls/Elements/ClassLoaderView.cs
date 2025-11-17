using GasStation.xml.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.ViewModels.Elements
{
    public class ClassLoaderView : INotifyPropertyChanged
    {
        /// <summary>
        /// Константы загрузчика
        /// </summary>
        public XmlClassLoaderConst _const { get; set; }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="con">Константы загрузчика</param>
        public ClassLoaderView(XmlClassLoaderConst con)
        {
            _const = con;            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _isErrorLoad;
        /// <summary>
        /// Авария загрузки
        /// </summary>
        public bool IsErrorLoad
        {
            get
            {
                return _isErrorLoad;
            }
            set
            {
                if (_const.StatusConst.GateClosed != null)
                    _isErrorLoad = !_const.StatusConst.ErrorLoad.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsErrorLoad"));
            }
        }

        private bool _isErrorUpLoad;
        /// <summary>
        /// Авария выгрузки
        /// </summary>
        public bool IsErrorUpLoad
        {
            get
            {
                return _isErrorUpLoad;
            }
            set
            {
                if (_const.StatusConst.GateClosed != null)
                    _isErrorUpLoad = !_const.StatusConst.ErrorUnLoad.IsInverted ? value : !value; ;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsErrorUpLoad"));
            }
        }

        private bool _isGateOpen;
        /// <summary>
        /// Заслонка открыта
        /// </summary>
        public bool IsGateOpen
        {
            get
            {
                return _isGateOpen;
            }
            set
            {
                if (_const.StatusConst.GateClosed != null)
                    _isGateOpen = !_const.StatusConst.GateOpen.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsGateOpen"));
            }
        }

        private bool _isGateClose;
        /// <summary>
        /// Заслонка закрыта
        /// </summary>
        public bool IsGateClose
        {
            get
            {
                return _isGateClose;
            }
            set
            {
                if (_const.StatusConst.GateClosed != null)
                    _isGateClose = !_const.StatusConst.GateClosed.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsGateClose"));
            }
        }



        private bool _isLoadComplete;
        /// <summary>
        /// Статус Загружен
        /// </summary>
        public bool IsLoadComplete
        {
            get
            {
                return _isLoadComplete;
            }
            set
            {
                _isLoadComplete = !_const.StatusConst.LoadComplete.IsInverted ? value : !value; ;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsLoadComplete"));
            }
        }

        private bool _isUpLoadComplete;
        /// <summary>
        /// Статус Выгружен
        /// </summary>
        public bool IsUpLoadComplete
        {
            get
            {
                return _isUpLoadComplete;
            }
            set
            {
                _isUpLoadComplete = !_const.StatusConst.UnLoadComplete.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsUpLoadComplete"));
            }
        }

        private bool? _statusLoad=null;
        public bool? StatusLoad
        {
            get
            {
                return _statusLoad;
            }
            set
            {
                _statusLoad = !_const.StatusConst.LoadComplete.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusLoad"));
            }
        }


        private bool? _statusUnload = null;
        public bool? StatusUnload
        {
            get
            {
                return _statusUnload;
            }
            set
            {
                _statusUnload = !_const.StatusConst.UnLoadComplete.IsInverted ? value : !value; ;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("StatusUnload"));
            }
        }

        private bool _regWork = false;
        public bool RegWork
        {
            get
            {
                return _regWork;
            }
            set
            {
                if (_const.StatusConst.RegWork != null)
                    _regWork = !_const.StatusConst.RegWork.IsInverted ? value : !value;
                else
                    _regWork = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RegWork"));
            }
        }

        public bool UsePriv { get; set; }


    }
}
