using ChartApplication.Points;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ChartApplication.points
{
    public class PointsData<data>
    {
        public string LabelAxeX = "Время";
        public string LabelAxeY = "Ось Y";

        public string LabelChart = "Подпись графика";
        public string LabelSeries = "Подпись графика";

        public PointsData()
        {

            PointsPrepare = new PointsCut(new BlockingCollection<PointTime>(), 900);
        }


        public PointsData(string labelAxeX,string labelAxeY,string labelChart,string labelSeries)
        {
            PointsPrepare = new PointsCut(new BlockingCollection<PointTime>(), 900);

            LabelAxeX = labelAxeX;

            LabelAxeY = labelAxeY;

            LabelChart = labelChart;

            LabelSeries = labelSeries;
        }

        //public BlockingCollection<Point> LstPoints;

        public PointsCut PointsPrepare { get; set; }
        //public int idx = 0;
        public void Add(PointTime d)
        {

            PointsPrepare.Add(d);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            if (CollectionChanged != null)
            {
                int a = 10;
            }

        }

        public Action<object, NotifyCollectionChangedEventArgs> CollectionChanged;
    }
}
