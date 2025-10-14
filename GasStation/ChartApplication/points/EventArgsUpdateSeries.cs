using ChartApplication.points;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ChartApplication.Points
{
    public class EventArgsUpdateSeries:EventArgs
    {
        public int SeriesNum=0;
        public PointTime[] PlotPoints;
        public EventArgsUpdateSeries(int seriesNum, BlockingCollection<PointTime> plotPoints)
        {
            SeriesNum = seriesNum;
            PlotPoints = plotPoints.ToArray();
        }

        public DateTime[] MasX
        {
            get
            {
                DateTime[] lstX;
                //lock (PlotPoints)
                    lstX=PlotPoints.Select(dat => dat.X).ToArray();
                return lstX; 
            }
        }

        

        //public List<DateTime> MasTimeX
        //{
        //    get
        //    {
        //        var masX = MasX;

        //        var masTimes = new List<DateTime>();

        //        masTimes.Add(new DateTime(2000, 1, 1, 0, 0, 0));

        //        for (int index = 1; index < masX.Length; index++)
        //        {
        //            masTimes.Add(masTimes[index - 1].AddSeconds(Math.Abs(masX[index] - masX[index - 1])));
        //        }


        //        //List<DateTime> masTimes = new DateTime[MasX.Length];


        //        ////masTimes[0].Add((new DateTime(2000, 1, 2, 0, 0, 0)).TimeOfDay);

        //        //masTimes[0] = new DateTime(2000, 1, 1, 0, 0, 0);


        //        //for (int index = 1; index < MasX.Length; index++)
        //        //{
        //        //    masTimes[index] = masTimes[index - 1].AddSeconds(Math.Abs(MasX[index] - MasX[index - 1]));
        //        //}

        //        return masTimes;
        //    }
        //}

        //private DateTime TimeOut(DateTime fullTime)
        //{
        //    return new DateTime(1, 1, 1, fullTime.Hour, fullTime.Minute, fullTime.Second);
        //}
        public double[] MasY
        {
            get
            {
                double[] lstY;
                lock(PlotPoints)
                    lstY = PlotPoints.Select(dat => dat.Y).ToArray();
                return lstY;
            }
        }

    }
}
