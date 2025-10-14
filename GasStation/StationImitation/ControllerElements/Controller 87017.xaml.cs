using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace StationImitation.ControllerElements
{
    /// <summary>
    /// Interaction logic for Controller_87017.xaml
    /// </summary>
    public partial class Controller_87017 : UserControl
    {
        public Controller_87017()
        {
            InitializeComponent();
        }

        public String Num
        {
            set { ContrNum.Text = value; }
        }

        #region Каналы

        #region Ch0

        public double Ch0
        {
            get { return (double)GetValue(Ch0Property); }
            set { SetValue(Ch0Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch0Property =
            DependencyProperty.Register("Ch0", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #region Ch1

        public double Ch1
        {
            get { return (double)GetValue(Ch1Property); }
            set { SetValue(Ch1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch1Property =
            DependencyProperty.Register("Ch1", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #region Ch2

        public double Ch2
        {
            get { return (double)GetValue(Ch2Property); }
            set { SetValue(Ch2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch2Property =
            DependencyProperty.Register("Ch2", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #region Ch3

        public double Ch3
        {
            get { return (double)GetValue(Ch3Property); }
            set { SetValue(Ch3Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch3Property =
            DependencyProperty.Register("Ch3", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #region Ch4

        public double Ch4
        {
            get { return (double)GetValue(Ch4Property); }
            set { SetValue(Ch4Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch4.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch4Property =
            DependencyProperty.Register("Ch4", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #region Ch5

        public double Ch5
        {
            get { return (double)GetValue(Ch5Property); }
            set { SetValue(Ch5Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch5.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch5Property =
            DependencyProperty.Register("Ch5", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #region Ch6

        public double Ch6
        {
            get { return (double)GetValue(Ch6Property); }
            set { SetValue(Ch6Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch6.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch6Property =
            DependencyProperty.Register("Ch6", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #region Ch7

        public double Ch7
        {
            get { return (double)GetValue(Ch7Property); }
            set { SetValue(Ch7Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch7.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch7Property =
            DependencyProperty.Register("Ch7", typeof(double), typeof(Controller_87017), new PropertyMetadata(0.0));

        #endregion

        #endregion
    }
}
