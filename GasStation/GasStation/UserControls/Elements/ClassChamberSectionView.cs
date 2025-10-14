using ChartApplication.points;
using GasStation.Elements.Data;
using GasStation.xml.Const.Elements;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace GasStation.ViewModels.Elements
{
    public class ClassChamberSectionView:INotifyPropertyChanged
    {
        public ClassChamberSectionView(XmlClassThermoSectionConst con)
        {
            _const = con;
          // DataChamber = new ClassDataChamber(dataTime, con);

            _seriesTdIn = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesTdIn} {_const.RusName.Replace("Термосекция", "")}");

            _seriesTdOut = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesTdOut} {_const.RusName.Replace("Термосекция", "")}");

            _seriesSetTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesSetTemp} {_const.RusName.Replace("Термосекция", "")}");

            _seriesCurrSetTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesCurrSetTemp} {_const.RusName.Replace("Термосекция", "")}");
        }

        private readonly XmlClassThermoSectionConst _const;

        private ClassDataChamber dataChamber;
        /// <summary>
        /// Данные по термосекции
        /// </summary>
        public ClassDataChamber DataChamber
        {
            get { return dataChamber; }
            set
            {
                if (dataChamber == null)               
                    dataChamber = value; //new ClassDataChamber(0, _const);

                //dataChamber.DataTime = value.DataTime.TimeStep;
                dataChamber.SetupTemp = value.SetupTemp;
                dataChamber.SetPower = value.SetPower;
                dataChamber.ClassPidOut = value.ClassPidOut;
                dataChamber.ClassPidIn = value.ClassPidIn;              
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataChamber"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentSetup"));
            }
        }

        private PointsData<Point> _seriesSetTemp;
        /// <summary>
        /// График установленной температуры от времени
        /// </summary>
        public PointsData<Point> SeriesSetTemp
        {
            get { return _seriesSetTemp; }
            set
            {
                _seriesSetTemp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesSetTemp"));
            }
        }

        private PointsData<Point> _seriesTdIn;
        /// <summary>
        /// График температурного датчика внутри камеры от времени
        /// </summary>
        public PointsData<Point> SeriesTdIn
        {
            get { return _seriesTdIn; }
            set
            {
                _seriesTdIn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesTdIn"));
            }
        }

        private PointsData<Point> _seriesTdOut;
        /// <summary>
        /// График температурного наружнего датчика  камеры от времени
        /// </summary>
        public PointsData<Point> SeriesTdOut
        {
            get { return _seriesTdOut; }
            set
            {
                _seriesTdOut = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesTdOut"));
            }
        }

        private PointsData<Point> _seriesCurrSetTemp;

        /// <summary>
        /// Текущее задание
        /// </summary>
        public double CurrentSetup
        {
            get
            {
                if (SeriesCurrSetTemp.PointsPrepare.ListPoints.Count == 0)
                    return 0;

                return SeriesCurrSetTemp.PointsPrepare.ListPoints.Last().Y;
            }
        }

        /// <summary>
        /// График считанной температуры от времени
        /// </summary>
        public PointsData<Point> SeriesCurrSetTemp
        {
            get { return _seriesCurrSetTemp; }
            set
            {
                _seriesCurrSetTemp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesCurrSetTemp"));
            }
        }

        private const string LabelX = @"Время";

        private const string LabelY = "˚C";

        private const string LabelChart = "График температуры";

        private const string LabelSeriesCurrSetTemp = "Т зад.";

        private const string LabelSeriesTdOut = "Рабочая Тп";

        private const string LabelSeriesTdIn = "Контрольная Tп";

        private const string LabelSeriesSetTemp = "Т уст.";

        public void Clear()
        {
            _seriesTdIn = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesTdIn} {_const.RusName.Replace("Термосекция", "")}");

            _seriesTdOut = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesTdOut} {_const.RusName.Replace("Термосекция", "")}");

            _seriesSetTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesSetTemp} {_const.RusName.Replace("Термосекция", "")}");

            _seriesCurrSetTemp = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesCurrSetTemp} {_const.RusName.Replace("Термосекция", "")}");
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public PointsData<Point>[] MasGraph
        {
            get
            {
                return new[] { SeriesTdIn, SeriesTdOut, SeriesCurrSetTemp };
            }
        }
    }
}
