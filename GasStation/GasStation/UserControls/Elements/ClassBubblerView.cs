using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GasStation.Annotations;
using ChartApplication.points;
using GasStation.xml.Const.Elements;

namespace GasStation.ViewModels.Elements
{
    public class ClassBubblerView : INotifyPropertyChanged
    {
       

        public ClassBubblerView(XmlClassBubblerConst con)
        {
            _const = con;
        }



        public PointsData<Point>[] MasGraph
        {
            get
            {
                return _const.Td != null ? new[] { SeriesReadTd } : null;                    
            }
        }
        /// <summary>
        /// Гпафик зависимости текущей барботера от времени
        /// </summary>
        private PointsData<Point> seriesReadTd = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelChart);
        /// <summary>
        /// График зависимости считанного значения температуры от времени
        /// </summary>
        public PointsData<Point> SeriesReadTd
        {
            get { return seriesReadTd; }
            set
            {
                seriesReadTd = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesReadStream"));
            }
        }

        private const string LabelX = @"Время";

        private const string LabelY = "Температура";

        private const string LabelChart = "Температура барботера";

        private const string LabelSeriesReadTd = "Текущая температура";

        public void Clear()
        {
            SeriesReadTd = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesReadTd} {_const.RusName} {_const.DevNum}");
        }

        private bool _relay;
        public bool Relay
        {
            get
            {
                return _relay;
            }
            set
            {
                _relay = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Relay"));
            }
        }


        private XmlClassBubblerConst _const;

        public string BigName { get; set; }


        private double _currentTd=0.0;
        /// <summary>
        /// Текущее значение температры
        /// </summary>
        public double CurrentTd
        {
            get
            {
                return _currentTd;
            }
            set
            {
                _currentTd = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentTd"));
            }
        }

        /// <summary>
        /// Возможность проверки привелегий
        /// </summary>
        public bool UsePriv { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
