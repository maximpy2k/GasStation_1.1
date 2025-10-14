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
using GasStation.Elements.Data;

namespace GasStation.ViewModels.Elements
{
    public class ClassRrgView : INotifyPropertyChanged
    {
        private PointsData<Point> seriesSetStream = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelSeriesSetStream);

        public ClassRrgView(XmlClassRrgConst con)
        {
            _const = con;
            if (_const.VacuumetrConst != null)
                VacuumetrView = new ClassVacuumetrView(con.VacuumetrConst, $" { _const.RusName } { _const.DevNum}");
        }
        /// <summary>
        /// График зависимости установленного значения потока газа от времени
        /// </summary>
        public PointsData<Point> SeriesSetStream
        {
            get { return seriesSetStream; }
            set
            {
                seriesSetStream = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesSetStream"));
            }
        }

        private PointsData<Point> _seriesCalcStream = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelSeriesCalcStream);
        /// <summary>
        /// График зависимости рассчитанного(устанавливаемое) значения потока газа от времени
        /// </summary>
        public PointsData<Point> SeriesCalcStream
        {
            get { return _seriesCalcStream; }
            set
            {
                _seriesCalcStream = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesCalcStream"));
            }
        }       

        public PointsData<Point>[] MasGraph
        {
            get
            {
                if(_const.VacuumetrConst!=null)
                    return new[] { SeriesReadStream, SeriesSetStream, VacuumetrView.SeriesReadPress };
                else
                    return new[] { SeriesReadStream, SeriesSetStream };
            }
        }

        private PointsData<Point> seriesReadStream = new PointsData<Point>(LabelX, LabelY, LabelChart, LabelSeriesReadStream);
        /// <summary>
        /// График зависимости считанного значения потока газа от времени
        /// </summary>
        public PointsData<Point> SeriesReadStream
        {
            get { return seriesReadStream; }
            set
            {
                seriesReadStream = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SeriesReadStream"));
            }
        }

        private const string LabelX = @"Время";

        private const string LabelY = "л/ч";

        private const string LabelChart = "График расхода газа";

        private const string LabelSeriesReadStream = "Текущий расход";

        private const string LabelSeriesCalcStream = "Расчетный расход";

        private const string LabelSeriesSetStream = "Заданный расход";

        public void Clear()
        {
            SeriesReadStream = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesReadStream} {_const.RusName} {_const.DevNum}");
            SeriesSetStream = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesSetStream} {_const.RusName} {_const.DevNum}");
            SeriesCalcStream = new PointsData<Point>(LabelX, LabelY, LabelChart, $"{LabelSeriesCalcStream} {_const.RusName} {_const.DevNum}");
            if(_const.VacuumetrConst!=null)
                VacuumetrView.Clear();
        }

        private double _currentValue = 4;
        private XmlClassRrgConst _const;

        public string BigName { get; set; }



        /// <summary>
        /// Текущее значение на РРГ
        /// </summary>
        public double CurrentValue
        {
            get
            {
                return _currentValue;
            }
            set
            {
                _currentValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentValue"));
            }
        }

        public ClassFlapView FlapView { get; set; }

        /// <summary>
        /// Возможность проверки привелегий
        /// </summary>
        public bool UsePriv { get; set; }


        /// <summary>
        /// Класс отображения данных вакуметра
        /// </summary>
        public ClassVacuumetrView VacuumetrView { get; set; } 

        public event PropertyChangedEventHandler PropertyChanged;

        private ClassDataRRG dataRrg;
        /// <summary>
        /// Данные по РРГ
        /// </summary>
        public ClassDataRRG DataRrg
        {
            get { return dataRrg; }
            set
            {
                dataRrg = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DataRrg"));
            }
        }

        private bool avalible = true;
        public bool Avalible
        {
            get { return avalible; }
            set
            {
                avalible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Avalible"));
            }
        }

    }
}
