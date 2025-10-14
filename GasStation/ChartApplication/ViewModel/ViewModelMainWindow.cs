using ChartApplication.Points;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace ChartApplication.ViewModel
{
    public class ViewModelMainWindow:INotifyPropertyChanged
    {
        public ViewModelMainWindow()
        {
            // Start();  
            
        }


        private EventHandler ViewPoints;

        


        public event PropertyChangedEventHandler PropertyChanged;

        private ObservableCollection<Point> colPoints0;
        public ObservableCollection<Point> ColPoints0
        {
            get { return colPoints0; }
            set
            {
                colPoints0 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColPoints0"));
            }
        }

        private ObservableCollection<Point> colPoints1;
        public ObservableCollection<Point> ColPoints1
        {
            get { return colPoints1; }
            set
            {
                colPoints1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColPoints1"));
            }
        }


        private ObservableCollection<Point> colPoints2;
        public ObservableCollection<Point> ColPoints2
        {
            get { return colPoints2; }
            set
            {
                colPoints2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColPoints2"));
            }
        }


        private ObservableCollection<Point> colPoints3;
        public ObservableCollection<Point> ColPoints3
        {
            get { return colPoints3; }
            set
            {
                colPoints3 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColPoints3"));
            }
        }


        private ObservableCollection<Point> colLine0;
        public ObservableCollection<Point> ColLine0
        {
            get { return colLine0; }
            set
            {
                colLine0 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColLine0"));
            }
        }

        private ObservableCollection<Point> colLine1;
        public ObservableCollection<Point> ColLine1
        {
            get { return colLine1; }
            set
            {
                colLine1 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColLine1"));
            }
        }

        private ObservableCollection<Point> colLine2;
        public ObservableCollection<Point> ColLine2
        {
            get { return colLine2; }
            set
            {
                colLine2 = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ColLine2"));
            }
        }

        public void Start()
        {
            ViewPoints += ShowPoints;
            ViewPoints.BeginInvoke(this, null, null, 0);
        }
        public void ShowPoints(object o, EventArgs e)
        {
            
            ColPoints0 = new ObservableCollection<Point>();
            ColPoints1 = new ObservableCollection<Point>();
            ColPoints2 = new ObservableCollection<Point>();

            ColLine0 = new ObservableCollection<Point>();
            ColLine1 = new ObservableCollection<Point>();
            ColLine2 = new ObservableCollection<Point>();

            double x = 0;
            for (;;)
            {
                ColPoints0.Add(new Point(x++, x * x));
                ColPoints1.Add(new Point(100 + x++, x * x));
                ColPoints2.Add(new Point(1000 + x++, x * x));

                ColLine0.Add(new Point(-(x++), x * x));
                ColLine1.Add(new Point(-(100 + x++), x * x));
                ColLine2.Add(new Point(-(1000 + x++), x * x));

                Thread.Sleep(100);
            }
        }

    }
}
