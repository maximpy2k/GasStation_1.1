using ChartApplication.points;
using System.ComponentModel;
using System.Windows;
using GasStation.xml.Constant.XmlConst.Elements;

namespace GasStation.ViewModels.Elements
{
    public class ClassHydrogenBurnerView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly XmlClassHydrogenBurnerConst  _const;

        public ClassHydrogenBurnerView(XmlClassHydrogenBurnerConst  con)
        {
            _const = con;

            if(_const.TdFire!=null)
                _seriesTdFire = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesTdFire} {_const.TdFire.RusName} {_const.TdFire.DevNum}");

            _seriesTdHeaterHydrogenBurner = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesTdHeaterHydrogenBurner} {_const.TdBurner.RusName} {_const.TdBurner.DevNum}");
        }

        private double _tdFire;
        /// <summary>
        /// Температура пламени горелки
        /// </summary>
        public double TdFire
        {
            get
            {
                return _tdFire;
            }

            set
            {
                _tdFire = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TdFire"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisibleTdFire"));
            }
        }

        private double _tdHeaterHydrogenBurner;
        /// <summary>
        /// Температура нагревателя горелки
        /// </summary>
        public double TdHeaterHydrogenBurner
        {
            get
            {
                return _tdHeaterHydrogenBurner;
            }

            set
            {
                _tdHeaterHydrogenBurner = value;

                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TdHeaterHydrogenBurner"));
            }
        }

        public Visibility visibleTdFire = Visibility.Visible;
        public Visibility VisibleTdFire
        {
            get
            {
                if (_const.TdFire != null)
                    visibleTdFire = Visibility.Visible;
                else
                    visibleTdFire = Visibility.Collapsed;

               return visibleTdFire;
            }
            set
            {
                visibleTdFire = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("VisibleTdFire"));
            }
        }

        //private bool _heating;
        ///// <summary>
        ///// Нагреватель горелки
        ///// </summary>
        //public bool Heating
        //{
        //    get
        //    {
        //        return _heating;
        //    }
        //    set
        //    {
        //        _heating = value;
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Heating"));
        //    }
        //}

        private bool _relay;

        /// <summary>
        /// Нагреватель горелки
        /// </summary>
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

        private bool _regimControl=true;

        /// <summary>
        /// Режим управления горелки
        /// </summary>
        public bool RegimControl
        {
            get
            {
                return _regimControl;
            }
            set
            {
                _regimControl = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RegimControl"));
            }
        }

        private bool _isFire;
        /// <summary>
        /// Температура пламени горелки
        /// </summary>
        public bool IsFire
        {
            get
            {
                return _isFire;
            }
            set
            {
                _isFire = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsFire"));
            }
        }

        private bool _isWater;
        /// <summary>
        /// Температура пламени горелки
        /// </summary>
        public bool IsWater
        {
            get
            {
                return _isWater;
            }
            set
            {
                _isWater = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("IsWater"));
            }
        }

        private PointsData<Point> _seriesTdFire;// = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelSeriesTdFire);
        /// <summary>
        /// График температурного датчика внутри камеры от времени
        /// </summary>
        public PointsData<Point> SeriesTdFire
        {
            get { return _seriesTdFire; }
            set
            {
                _seriesTdFire = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesTdIn"));
            }
        }

        private PointsData<Point> _seriesTdHeaterHydrogenBurner;// = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelSeriesTdHeaterHydrogenBurner);
        /// <summary>
        /// График температурного наружнего датчика  камеры от времени
        /// </summary>
        public PointsData<Point> SeriesTdHeaterHydrogenBurner
        {
            get { return _seriesTdHeaterHydrogenBurner; }
            set
            {
                _seriesTdHeaterHydrogenBurner = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesTdOut"));
            }
        }

        /// <summary>
        /// Использовать ли привилегии
        /// </summary>
        public bool UsePriv { get; set; }

        private const string LabelX = @"Время";

        private const string LabelY = "C˚";

        private const string LabelChart = "График температуры горелки";

        private const string LabelSeriesTdFire = "Т пламени";

        private const string LabelSeriesTdHeaterHydrogenBurner = "Т нагревателя";

        public PointsData<Point>[] MasGraph
        {
            get
            {
                if(SeriesTdFire!=null)
                    return new[] { SeriesTdHeaterHydrogenBurner, SeriesTdFire };
                return new[] { SeriesTdHeaterHydrogenBurner};
            }
        }

        public void Clear()
        {
            if(_const.TdFire!=null)
                SeriesTdFire = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{_const.TdFire.RusName} {_const.TdFire.DevNum}");
            SeriesTdHeaterHydrogenBurner = new PointsData<Point>(LabelX, LabelY, LabelChart,  $"{_const.TdBurner.RusName} {_const.TdBurner.DevNum}");
        }
    }
}
