using ChartApplication.points;
using GasStation.Annotations;
using GasStation.Elements.Data;
using GasStation.xml.Const.Elements;
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
                return _const.Td != null ? new[] { SeriesReadTemp, SeriesSetTemp } : null;                    
            }
        }
        /// <summary>
        /// Гпафик зависимости текущей барботера от времени
        /// </summary>
        private PointsData<Point> seriesReadTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelSeriesReadTemp);
        /// <summary>
        /// График зависимости считанного значения температуры от времени
        /// </summary>
        public PointsData<Point> SeriesReadTemp
        {
            get { return seriesReadTemp; }
            set
            {
                seriesReadTemp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesReadStream"));
            }
        }

        private PointsData<Point> seriesSetTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelSeriesSetTemp);
        public PointsData<Point> SeriesSetTemp
        {
            get { return seriesSetTemp; }
            set
            {
                seriesSetTemp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesReadStream"));
            }
        }

        private const string LabelX = @"Время";

        private const string LabelY = "Температура";

        private const string LabelChart = "Температура барботера";

        private const string LabelSeriesReadTemp = "Текущая температура";
        private const string LabelSeriesSetTemp = "Заданная температура";
        private const string LabelSeriesCalcTemp = "Рассчитанная температура";

        public void Clear()
        {
            SeriesReadTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesReadTemp} {_const.RusName} {_const.DevNum}");
            SeriesSetTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesSetTemp} {_const.RusName} {_const.DevNum}");
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

        private ClassDataBubler dataBubbler;
        /// <summary>
        /// Данные по Барботеру
        /// </summary>
        public ClassDataBubler DataBubbler
        {
            get { return dataBubbler; }
            set
            {
                dataBubbler = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataBubbler"));
            }
        }
        private bool usePid = false;
        public bool UsePid
        {
            get
            {
                return usePid;
            }
            set
            {
                usePid = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UsePid"));
            }

        }

        public event PropertyChangedEventHandler PropertyChanged;

    }
}
