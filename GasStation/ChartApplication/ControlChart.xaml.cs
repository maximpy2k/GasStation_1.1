using ChartApplication.points;
using ChartApplication.Points;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms.DataVisualization.Charting;

namespace ChartApplication
{
    /// <summary>
    /// Interaction logic for ControlChart.xaml
    /// </summary>
    public partial class ControlChart : System.Windows.Controls.UserControl
    {
        public ControlChart()
        {
            InitializeComponent(); 
            chart.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart.ChartAreas[0].CursorY.IsUserEnabled = true;
            chart.ChartAreas[0].CursorY.IsUserSelectionEnabled = true;

            chart.ChartAreas[0].CursorX.Interval = 1;
            chart.ChartAreas[0].CursorX.IntervalType = DateTimeIntervalType.Seconds;
            chart.ChartAreas[0].CursorY.Interval = 5;
            chart.ChartAreas[0].AxisY.IsStartedFromZero = false;

            chart.ChartAreas[0].AxisX.LabelStyle.Format = "g";
            //chart.ChartAreas[0].AxisX.LabelStyle.Angle=-90;
            chart.ChartAreas[0].AxisY.LabelStyle.Format = "{0.0}";
            chart.ChartAreas[0].AxisY.LabelStyle.Angle = -90;

            chart.Legends.Add(new Legend());
            chart.Legends[0].Docking = Docking.Bottom;
            chart.Titles.Add(new Title());

            for (int idx = 0; idx < chart.Series.Count; idx++)
            {
                chart.Series[idx].IsVisibleInLegend = false;
                LegendItem legendItem = new LegendItem();
                legendItem.ImageStyle = LegendImageStyle.Line;
                legendItem.BorderWidth = 3;
                legendItem.Color = chart.Series[idx].Color;
                legendItem.Cells.Add(LegendCellType.SeriesSymbol, "", System.Drawing.ContentAlignment.MiddleCenter);
                legendItem.Cells.Add(LegendCellType.Text, chart.Series[idx].Name, System.Drawing.ContentAlignment.MiddleCenter);
                legendItem.Enabled = false;
                chart.Legends[0].CustomItems.Add(legendItem);
                chart.Legends[0].CustomItems[idx].Tag = chart.Series[idx];
            }
            chart.MouseDown += chart1_MouseDown1;
        }

        private void chart1_MouseDown1(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            System.Windows.Forms.DataVisualization.Charting.HitTestResult result = chart.HitTest(e.X, e.Y);
            if (result != null && result.Object != null)
            {
                // When user hits the LegendItem
                if (result.Object is LegendItem)
                {
                    // Legend item result
                    LegendItem legendItem = (LegendItem)result.Object;

                    // series item selected
                    Series selectedSeries = (Series)legendItem.Tag;

                    if (selectedSeries != null)
                    {
                        if (selectedSeries.Enabled)
                        {
                            selectedSeries.Enabled = false;
                            legendItem.Cells[1].ForeColor = System.Drawing.Color.Gray;
                        }
                        else
                        {
                            selectedSeries.Enabled = true;
                            legendItem.Cells[1].ForeColor = System.Drawing.Color.Black;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Подготовка точек для вывода на графике
        /// </summary>
        private PointsData<Point> PointsCutPoints0;
        private PointsData<Point> PointsCutPoints1;
        private PointsData<Point> PointsCutPoints2;
                          
        private PointsData<Point> PointsCutLine0;
        private PointsData<Point> PointsCutLine1;
        private PointsData<Point> PointsCutLine2;

        public PointsData<Point>[] MasGraph;




        //private string ToStrTime(double sec)
        //{
        //    var secInt = Convert.ToInt32(sec);

        //    var hour = secInt / 3600;

        //    var minute = (secInt - hour * 3600) / 60;

        //    var secc = (secInt - hour * 3600 - minute * 60);

        //    return $"{hour}:{minute}:{secc}";
        //}

        public void UpdateLegends(PointsData<Point> points, int axe)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new Action<PointsData<Point>, int>(UpdateLegends), points, axe);
                return;
            }

            chart.ChartAreas[0].AxisX.Title = points.LabelAxeX;
            chart.ChartAreas[0].AxisY.Title = points.LabelAxeY;

            chart.Titles[0].Text = points.LabelChart;
            //chart.Titles[0].Font = new Font(new timeNe);
            chart.Series[axe].Name = points.LabelSeries;
            chart.Series[axe].Enabled = true;

            chart.Legends[0].CustomItems[axe].Cells[1].Text = chart.Series[axe].Name;
            chart.Legends[0].CustomItems[axe].Enabled = true;
        }

        

        private void PlotPoints0(object sender, EventArgs e)
        {
            if(Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new EventHandler(PlotPoints0), sender, e);
                return;
            }
            var args = (EventArgsUpdateSeries)(e);

            //var times =args.MasX.Select(ToStrTime).ToList();

            //'' повернуть значения подписи оси X  на 90 гр
            //Chart3.ChartAreas.Item(0).AxisX.LabelStyle.Angle = -90;

            //chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;

            //points0.Points.DataBindXY(times, args.MasY);
            points0.Points.DataBindXY(args.MasX, args.MasY);
        }
        private void PlotPoints1(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new EventHandler(PlotPoints1), sender, e);
                return;
            }
            var args = (EventArgsUpdateSeries)(e);

            //var times = args.MasX.Select(ToStrTime).ToList();

            //points1.Points.DataBindXY(args.MasX, args.MasY);
            points1.Points.DataBindXY(args.MasX, args.MasY);
        }
        private void PlotPoints2(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new EventHandler(PlotPoints2), sender, e);
                return;
            }
            var args = (EventArgsUpdateSeries)(e);

            //var times = args.MasX.Select(ToStrTime).ToList();

            //points2.Points.DataBindXY(args.MasX, args.MasY);
            points2.Points.DataBindXY(args.MasX, args.MasY);
        }


        private void PlotLine0(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new EventHandler(PlotLine0), sender, e);
                return;
            }
            var args = (EventArgsUpdateSeries)(e);

            line0.Points.DataBindXY(args.MasX, args.MasY);
            //line0.Points.DataBindXY(args.MasTimeX, args.MasY);
            
        }
        private void PlotLine1(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new EventHandler(PlotLine1), sender, e);
                return;
            }
            var args = (EventArgsUpdateSeries)(e);
            line1.Points.DataBindXY(args.MasX, args.MasY);
            //line1.Points.DataBindXY(args.MasTimeX, args.MasY);
        }
        private void PlotLine2(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new EventHandler(PlotLine2), sender, e);
                return;
            }
            var args = (EventArgsUpdateSeries)(e);
            line2.Points.DataBindXY(args.MasX, args.MasY);
            //line2.Points.DataBindXY(args.MasTimeX, args.MasY);
        }


        #region DependencyProperty Points0
        public PointsData<Point> Points0
        {
            get { return (PointsData<Point>)GetValue(Points0Property); }
            set { SetValue(Points0Property, value); }
        }

        public static readonly DependencyProperty Points0Property =
            DependencyProperty.Register("Points0", typeof(PointsData<Point>), typeof(ControlChart), new FrameworkPropertyMetadata(new PointsData<Point>(), ChangePoints0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void ChangePoints0(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var control = (ControlChart)d;
            var points = (PointsData<Point>)e.NewValue;
            control.PointsCutPoints0 = points;

        }
        #endregion

        #region DependencyProperty Points1
        public PointsData<Point> Points1
        {
            get { return (PointsData<Point>)GetValue(Points1Property); }
            set { SetValue(Points1Property, value); }
        }

        public static readonly DependencyProperty Points1Property =
            DependencyProperty.Register("Points1", typeof(PointsData<Point>), typeof(ControlChart), new FrameworkPropertyMetadata(new PointsData<Point>(), ChangePoints1) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void ChangePoints1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var control = (ControlChart)d;
            var points = (PointsData<Point>)e.NewValue;
            control.PointsCutPoints1 = points;
            points.PointsPrepare.EventPlotPoints += control.PlotPoints0;

        }
        #endregion

        #region DependencyProperty Points2
        public PointsData<Point> Points2
        {
            get { return (PointsData<Point>)GetValue(Points2Property); }
            set { SetValue(Points2Property, value); }
        }

        public static readonly DependencyProperty Points2Property =
            DependencyProperty.Register("Points2", typeof(PointsData<Point>), typeof(ControlChart), new FrameworkPropertyMetadata(new PointsData<Point>(), ChangePoints2) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void ChangePoints2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var control = (ControlChart)d;
            var points = (PointsData<Point>)e.NewValue;
            control.PointsCutPoints2 = points;

        }
        #endregion

        #region DependencyProperty line0
        public PointsData<Point> Line0
        {
            get { return (PointsData<Point>)GetValue(Line0Property); }
            set { SetValue(Line0Property, value); }
        }

        public static readonly DependencyProperty Line0Property =
            DependencyProperty.Register("Line0", typeof(PointsData<Point>), typeof(ControlChart), new FrameworkPropertyMetadata(new PointsData<Point>(), ChangeLine0) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void ChangeLine0(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var control = (ControlChart)d;
            var points = (PointsData<Point>)e.NewValue;

            control.PointsCutLine0 = points;
            points.PointsPrepare.EventPlotPoints += control.PlotLine0;
            control.UpdateLegends(points, 3);
        }
        #endregion

        #region DependencyProperty line1
        public PointsData<Point> Line1
        {
            get { return (PointsData<Point>)GetValue(Line1Property); }
            set { SetValue(Line1Property, value); }
        }

        public static readonly DependencyProperty Line1Property =
            DependencyProperty.Register("Line1", typeof(PointsData<Point>), typeof(ControlChart), new FrameworkPropertyMetadata(new PointsData<Point>(), ChangeLine1) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void ChangeLine1(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var control = (ControlChart)d;
            var points = (PointsData<Point>)e.NewValue;
            control.PointsCutLine1 = points;
            points.PointsPrepare.EventPlotPoints += control.PlotLine1;
            control.UpdateLegends(points, 4);
        }
        #endregion

        #region DependencyProperty line2
        public PointsData<Point> Line2
        {
            get { return (PointsData<Point>)GetValue(Line2Property); }
            set { SetValue(Line2Property, value); }
        }

        public static readonly DependencyProperty Line2Property =
            DependencyProperty.Register("Line2", typeof(PointsData<Point>), typeof(ControlChart), new FrameworkPropertyMetadata(new PointsData<Point>(), ChangeLine2) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void ChangeLine2(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var control = (ControlChart)d;
            var points = (PointsData<Point>)e.NewValue;
            control.PointsCutLine2 = points;
            points.PointsPrepare.EventPlotPoints += control.PlotLine2;
            control.UpdateLegends(points, 5);
        }
        #endregion



        private void PlotLine(object sender, EventArgs e)
        {
            if (Dispatcher.Thread != Thread.CurrentThread)
            {
                Dispatcher.Invoke(new EventHandler(PlotLine), sender, e);
                return;
            }
            var args = (EventArgsUpdateSeries)(e);
            int num = args.SeriesNum;
            
            chart.Series[num].Points.DataBindXY(args.MasX, args.MasY);
            //line2.Points.DataBindXY(args.MasTimeX, args.MasY);
        }

        public System.Drawing.Color GetColor(int i)
        {
            switch (i)
            {
                case 0:
                    var color = System.Drawing.Color.FromArgb(255, 107, 51);
                    return color;
                    
                case 1:
                    return System.Drawing.Color.LightGreen;
                case 2:
                    var color2 = System.Drawing.Color.FromArgb(255, 105, 105);
                    return color2;
                case 3:
                    var color1 = System.Drawing.Color.FromArgb(255, 70, 0);
                    return color1;
                case 4:
                    return System.Drawing.Color.Green;
                case 5:
                    return System.Drawing.Color.Red;
                case 6:
                    return System.Drawing.Color.Blue;
                case 7:
                    return System.Drawing.Color.LightBlue;
                case 8:
                    return System.Drawing.Color.Cyan;
                case 9:
                    return System.Drawing.Color.Black;
                case 10:
                    return System.Drawing.Color.IndianRed;
                case 11:
                    return System.Drawing.Color.DarkRed;
                case 12:
                    return System.Drawing.Color.Cyan;
                case 13:
                    return System.Drawing.Color.DarkBlue;
                case 14:
                    return System.Drawing.Color.DarkGoldenrod;
                case 15:
                    return System.Drawing.Color.DarkKhaki;
                default:
                    return System.Drawing.Color.Black;

            }
        }

        public void PrepareSeries(PointsData<Point>[] masGraph)
        {
            
            chart.ChartAreas[0].AxisX.Title = "Время";
            chart.ChartAreas[0].AxisY.Title = "Значение";
 
            chart.Series.Clear();
            chart.Legends.Clear();
            for (int i = 0; MasGraph != null &&  i < MasGraph.Length; i++)
            {
                
                if (MasGraph[i].PointsPrepare == null)
                    continue;
                MasGraph[i].PointsPrepare.EventPlotPoints -= PlotLine;
            }

            MasGraph = masGraph;
            chart.Legends.Add(new Legend());
            chart.Legends[0].Docking = Docking.Right;

            for (int i = 0; i < masGraph.Length; i++)
            {
                MasGraph[i].PointsPrepare.plotGraphNum = i;
                MasGraph[i].PointsPrepare.EventPlotPoints += PlotLine;
                var ser = new Series();
                //ser.Name = masGraph[i].LabelSeries;
                ser.XValueType = ChartValueType.DateTime;
                ser.BorderWidth = 3;
                ser.IsVisibleInLegend = false;
                ser.Color = GetColor(i);//System.Drawing.Color.Black;
                ser.ChartType = SeriesChartType.FastLine;
                chart.Series.Add(ser);
                
                #region подпись


                LegendItem legendItem = new LegendItem();
                legendItem.ImageStyle = LegendImageStyle.Line;                
                legendItem.BorderWidth = 3;
                legendItem.Color = chart.Series[i].Color;
                legendItem.Cells.Add(LegendCellType.SeriesSymbol, "", System.Drawing.ContentAlignment.MiddleCenter);
                legendItem.Cells.Add(LegendCellType.Text, chart.Series[i].Name, System.Drawing.ContentAlignment.MiddleCenter);
                legendItem.Enabled = false;
                chart.Legends[0].CustomItems.Add(legendItem);
                chart.Legends[0].CustomItems[i].Tag = chart.Series[i];
                chart.Legends[0].CustomItems[i].Cells[1].Text = masGraph[i].LabelSeries;
                chart.Legends[0].CustomItems[i].Enabled = true;
                #endregion
            }
            //ChartGraphics
        }
        #region DependencyProperty PointsMas
        public PointsData<Point>[] PointsMas
        {
            get { return (PointsData<Point>[])GetValue(PointsMasProperty); }
            set { SetValue(PointsMasProperty, value); }
        }

        public static readonly DependencyProperty PointsMasProperty =
            DependencyProperty.Register("PointsMas", typeof(PointsData<Point>[]), typeof(ControlChart), new FrameworkPropertyMetadata(new PointsData<Point>[0], ChangePointsMas) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

        private static void ChangePointsMas(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue == null)
                return;

            var control = (ControlChart)d;
            var masGraph = (PointsData<Point>[])e.NewValue;

            control.PrepareSeries(masGraph);
        }
        #endregion



        public void DeleteEvents()
        {

            if (PointsCutLine0 != null)
                PointsCutLine0.PointsPrepare.EventPlotPoints -= PlotLine0;
            if (PointsCutLine1 != null)
                PointsCutLine1.PointsPrepare.EventPlotPoints -= PlotLine1;
            if (PointsCutLine2 != null)
                PointsCutLine2.PointsPrepare.EventPlotPoints -= PlotLine2;


            if (PointsCutPoints0 != null)
                PointsCutPoints0.PointsPrepare.EventPlotPoints -= PlotPoints0;
            if (PointsCutPoints1 != null)
                PointsCutPoints1.PointsPrepare.EventPlotPoints -= PlotPoints1;
            if (PointsCutPoints2 != null)
                PointsCutPoints2.PointsPrepare.EventPlotPoints -= PlotPoints2;

        }
    }
}
