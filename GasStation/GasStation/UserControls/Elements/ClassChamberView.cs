using GasStation.xml.Const.Elements;
using System.ComponentModel;
using GasStation.xml.Constant;
using System.Windows;
using ChartApplication.points;
using System.Collections.Generic;

namespace GasStation.ViewModels.Elements
{
    public class ClassChamberView : INotifyPropertyChanged
    {
        XmlClassChamberConst _const;
        public ClassChamberView(XmlClassChamberConst con)
        {
            _const = con;
            ThermoSectionView = new ClassChamberSectionView[con.ThermoSectionConst.Length];

            for (int i = 0; i < con.ThermoSectionConst.Length; i++)
                ThermoSectionView[i] = new ClassChamberSectionView(con.ThermoSectionConst[i]);
        }


        private bool _isWater;
        /// <summary>
        /// Датчик воды
        /// </summary>
        public bool IsWater
        {
            get
            {
                return _isWater;
            }
            set
            {
                _isWater = !_const.DioWaterConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsWater"));
            }
        }

        private bool _isWaterForward;
        /// <summary>
        /// Датчик воды передний фланец
        /// </summary>
        public bool IsWaterForward
        {
            get
            {
                return _isWaterForward;
            }
            set
            {
                _isWaterForward = !_const.DioWaterForwardConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsWaterForward"));
            }
        }

        private bool _isWaterBackward;
        /// <summary>
        /// Датчик воды передний фланец
        /// </summary>
        public bool IsWaterBackward
        {
            get
            {
                return _isWaterBackward;
            }
            set
            {
                _isWaterBackward = !_const.DioWaterBackwardConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsWaterBackward"));
            }
        }

        bool openDumperStatus;
        /// <summary>
        /// Статус открытой заслонки
        /// </summary>
        public bool OpenDumperStatus
        {
            get { return openDumperStatus; }
            set
            {
                openDumperStatus = !_const.DioDumperOpenConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OpenDumperStatus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GateStatus"));
            }
        }

        bool closeDumperStatus;
        /// <summary>
        /// Статус закрытой заслонки
        /// </summary>
        public bool CloseDumperStatus
        {
            get { return closeDumperStatus; }
            set
            {
                closeDumperStatus = !_const.DioDumperCloseConst.IsInverted ? value : !value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CloseDumperStatus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GateStatus"));
            }
        }

        //public bool? OpenStatus
        //{
        //    get
        //    {
        //        if (OpenShutterStatus)
        //            return true;
        //        if (CloseShutterStatus)
        //            return false;
        //        return null;
        //    }
        //}

        /// <summary>
        /// Отображение заслонки
        /// </summary>
        public bool GateStatus
        {
            get
            {
                if (openDumperStatus)
                    return false;
                if (closeDumperStatus)
                    return true;

                return false;
            }
        }

        public bool enableButton = true;
        public bool EnableButton
        {
            get { return enableButton; }
            set
            {
                enableButton = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("EnableButton"));
            }

        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void Clear()
        {
            foreach (var item in ThermoSectionView)
            {
                item.Clear();
            }
        }

        /// <summary>
        /// Может ли менять настройки,используя привелегии
        /// </summary>
        public bool UsePriv { get; set; }

        public ClassChamberSectionView[] ThermoSectionView { get; set; }

        public PointsData<Point>[] MasGraph
        {
            get
            {
                if (ThermoSectionView.Length == 0)
                    return null;
                var lst = new List<PointsData<Point>>();
                for (int i = 0; i < ThermoSectionView.Length; i++)
                    lst.Add(ThermoSectionView[i].SeriesCurrSetTemp);
                for (int i = 0; i < ThermoSectionView.Length; i++)
                    lst.Add(ThermoSectionView[i].SeriesTdIn);
                for (int i = 0; i < ThermoSectionView.Length; i++)
                    lst.Add(ThermoSectionView[i].SeriesTdOut);
                return lst.ToArray();
            }
        }

        public bool isVoltage24 = true;
        public bool IsVoltage24
        {
            get { return isVoltage24; }
            set
            {
                isVoltage24 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsVoltage24"));
            }

        }
    }
}
