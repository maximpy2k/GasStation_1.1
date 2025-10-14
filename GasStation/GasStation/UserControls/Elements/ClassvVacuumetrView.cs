using ChartApplication.points;
using GasStation.xml.Const.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GasStation.ViewModels.Elements
{
    public class ClassVacuumetrView : INotifyPropertyChanged
    {
        XmlClassVacuumetrConst _const;
        String _devName = "";
        public ClassVacuumetrView(XmlClassVacuumetrConst con, string devName)
        {
            _const = con;
            _devName = devName;
        }

        private double _currentPress = 0.0;
        /// <summary>
        /// Текущее значение температры
        /// </summary>
        public double CurrentPress
        {
            get
            {
                return _currentPress;
            }
            set
            {
                _currentPress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentPress"));
            }
        }

        //public string EnableOut
        //{
        //    get
        //    {
        //        if_const
        //        return _const.EnableOut;
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        private bool _pressDio = false;
        /// <summary>
        /// Отображение состояния выхода вакуметра
        /// </summary>
        public bool PressDio
        {
            get
            {
                return _pressDio;
            }
            set
            {
                _pressDio = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("PressDio"));
            }
        }

        private const string LabelX = @"Время";

        private const string LabelY = "Давление";

        private const string LabelChart = "График давления";

        public const string LabelSeriesReadPress = "Вакууметр";
        public string Label
        {
            get { return LabelSeriesReadPress; }
        }
        /// <summary>
        /// Гпафик зависимости текущей барботера от времени
        /// </summary>
        private PointsData<Point> seriesReadPress = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelChart);
        public PointsData<Point> SeriesReadPress
        {
            get { return seriesReadPress; }
            set
            {
                seriesReadPress = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesReadPress"));
            }
        }
        public void Clear()
        {
            SeriesReadPress = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{Label} {_devName}");
        }
    }
}
