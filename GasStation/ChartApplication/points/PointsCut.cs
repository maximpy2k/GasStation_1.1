using ChartApplication.points;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

namespace ChartApplication.Points
{

     ///<summary>
    /// Класс получения точек для графика
    ///</summary>
 public  class PointsCut
 {
        public int plotGraphNum = 0;
        /// <summary>
        /// Название графика
        /// </summary>
        public string Title;
        /// <summary>
        /// Подпись оси X
        /// </summary>
        public string NameAxeX;
        /// <summary>
        /// Подпись оси Y
        /// </summary>
        public string NameAxeY;
        /// <summary>
        /// Подпись графика
        /// </summary>
        public string InfoSeries;
        
            
        public BlockingCollection<PointTime> ListPoints;

        public BlockingCollection<PointTime> PlotPoints = new BlockingCollection<PointTime>();

        ///<summary>
        /// Х минимум
        ///</summary>
        public Single XMin;

        ///<summary>
        /// Х максимум
        ///</summary>
        public Single XMax;

        /// <summary>
        /// Ширина графика в пикселях
        /// </summary>
        /// 
        public int Width = 1000;
        public void Clr(int cnt)
        {
            var lst = ListPoints.ToArray();
            ListPoints = new BlockingCollection<PointTime>();
            var crop = lst.Where((dat, idx) => idx >= lst.Length - cnt).ToArray();
            foreach (var d in crop)
            {
                ListPoints.Add(d);
            }
            RefrashIntervals(ListPoints);
            GenerateEventPlotPoints();
        }

        int intervals
        { get { return Width; } }

        int pointsInInterval=1;
        int begLastInterval = 0;
        int EndLastInterval = 1;

        private void RefrashIntervals( BlockingCollection<PointTime> lstPoints)
        {
            pointsInInterval = lstPoints.Count >= intervals ? lstPoints.Count / intervals : 1;
            begLastInterval = 0;
            EndLastInterval = begLastInterval + pointsInInterval;
            PlotPoints = new BlockingCollection<PointTime>();
            var arrayPoints = lstPoints.ToArray();

            for (;;)
            {
                if (arrayPoints.Length < EndLastInterval)
                    break;

                var min = arrayPoints[begLastInterval];
                var max = arrayPoints[begLastInterval];

                for (int i = begLastInterval; i < EndLastInterval; i++)
                {
                    var val = arrayPoints[i];
                    if (min.Y < val.Y)
                        min = val;
                    if (max.Y > val.Y)
                        max = val;
                }
                PointTime mn = new PointTime(min.X, min.Y);
                PointTime mx = new PointTime(max.X, max.Y);


                if (min.X == max.X)
                    PlotPoints.Add(mn);
                else
                {
                    if (min.X > max.X)
                    {
                        PlotPoints.Add(mx);
                        PlotPoints.Add(mn);
                    }
                    else
                    {
                        PlotPoints.Add(mn);
                        PlotPoints.Add(mx);
                    }
                }


                begLastInterval += pointsInInterval;
                EndLastInterval += pointsInInterval;
            }
        }
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="listX">Значения по оси X</param>
        /// <param name="listY">Значения по оси Y</param>
        /// <param name="width">Ширина графика</param>
        public PointsCut(BlockingCollection<PointTime> lstPoints, int width/*, EventHandler plotPoints*/)
        {
            Width = width;
            ListPoints = lstPoints;

            if (ListPoints.Count <= 0)
                return;
            RefrashIntervals(ListPoints);
            EventPlotPoints?.Invoke(this, new EventArgsUpdateSeries(0, PlotPoints));

        }
        public bool IsCorrection=false;
        
        public void Add(PointTime p)
        {
            ListPoints.Add(p);
            if (PlotPoints.Count > 8 * Width)
            {
                RefrashIntervals(ListPoints);
                GenerateEventPlotPoints();
                return;
            }
           
            for (;;)
            {

                if (ListPoints.Count < EndLastInterval)
                    break;

                var min = ListPoints.Where((dat, idx) => idx == begLastInterval).First();
                var max = ListPoints.Where((dat, idx) => idx == begLastInterval).First();

                for (int i = begLastInterval; i < EndLastInterval; i++)
                { 
                    var val = ListPoints.Where((dat, idx) => idx == i).First();
                    if (min.Y < val.Y)
                        min = val;
                    if (max.Y > val.Y)
                        max = val;
                }

                PointTime mn = new PointTime(min.X , min.Y);
                PointTime mx = new PointTime(max.X, max.Y);
                
                if(min.X==max.X)
                    PlotPoints.Add(mn);
                else
                {
                    if(min.X>max.X)
                    {
                        PlotPoints.Add(mx);
                        PlotPoints.Add(mn);
                    }
                    else
                    {
                        PlotPoints.Add(mn);
                        PlotPoints.Add(mx);
                    }
                }

                begLastInterval += pointsInInterval;
                EndLastInterval += pointsInInterval;

                GenerateEventPlotPoints();
            }

            
        }
                
        DateTime lastView;
        public EventHandler EventPlotPoints;
        /// <summary>
        /// Генерация события перерисовки графика
        /// </summary>
        public void GenerateEventPlotPoints()
        {
            DateTime currView = DateTime.Now;
            if ((currView - lastView).TotalSeconds < 1)
                return;

            lastView = currView;
            
            EventPlotPoints?.Invoke(this, new EventArgsUpdateSeries(plotGraphNum, PlotPoints));
        }
    }
}
