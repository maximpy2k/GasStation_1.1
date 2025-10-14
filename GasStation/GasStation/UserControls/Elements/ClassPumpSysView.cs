using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using GasStation.Annotations;
using ChartApplication.points;
using GasStation.xml.Const.Elements;
using GasStation.xml.Constant;

namespace GasStation.ViewModels.Elements
{
    public class ClassPumpSysView : INotifyPropertyChanged
    {
        public ClassPumpSysView(XmlClassPumpSysConst con)
        {
            _const = con;
            WorkUseRoughingPump = false;
            VacuumetrView = new ClassVacuumetrView(con.VacuumetrConst, $" { _const.RusName.ToLower() } { _const.DevNum}");
        }

        public PointsData<Point>[] MasGraph
        {
            get
            {
                return new[] { VacuumetrView.SeriesReadPress };
            }
        }

        /// <summary>
        /// График зависимости считанного значения температуры от времени
        /// </summary>


        private const string LabelX = @"Время";

        private const string LabelY = "Давление";

        private const string LabelChart = "график давления";


        public string VacuumPumpLampVisiblity => _const.VacuumPump.StatusConst == null ? "Collapsed" : "Visiblity";
        public string RoughingPumpLampVisiblity => _const.RoughingPump.StatusConst == null ? "Collapsed" : "Visiblity";


        bool vacuumPumpLamp;
        public bool VacuumPumpLamp
        {
            get { return vacuumPumpLamp; }
            set
            {
                vacuumPumpLamp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VacuumPumpLamp"));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VacuumPumpLamp"));
            }
        }


        bool roughingPumpLamp;
        public bool RoughingPumpLamp
        {
            get { return roughingPumpLamp; }
            set
            {
                roughingPumpLamp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoughingPumpLamp"));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RoughingPumpLamp"));
            }
        }


        private XmlClassPumpSysConst _const;


        private bool workUseRoughingPump;
        /// <summary>
        /// Реальная работа форвакуумного насоса
        /// </summary>
        public bool WorkUseRoughingPump
        {
            get { return workUseRoughingPump; }
            set { workUseRoughingPump = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WorkUseRoughingPump")); }
        }



        public string BigName { get; set; }
        /// <summary>
        /// Класс отображения данных вакуметра
        /// </summary>
        public ClassVacuumetrView VacuumetrView { get; set; }

        public void Clear()
        {
            VacuumetrView.Clear();
        }

        /// <summary>
        /// Возможность проверки привелегий
        /// </summary>
        public bool UsePriv { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}