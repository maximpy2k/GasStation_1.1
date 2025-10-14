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
    /// Interaction logic for Controller_87024.xaml
    /// </summary>
    public partial class Controller_87024 : UserControl
    {
        public Controller_87024()
        {
            InitializeComponent();
        }

        #region Ch0

        public double Ch0
        {
            get { return (double)GetValue(Ch0Property); }
            set { SetValue(Ch0Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch0.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch0Property =
            DependencyProperty.Register("Ch0", typeof(double), typeof(Controller_87024), new PropertyMetadata(0.0));

        #endregion

        #region Ch1

        public double Ch1
        {
            get { return (double)GetValue(Ch1Property); }
            set { SetValue(Ch1Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch1.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch1Property =
            DependencyProperty.Register("Ch1", typeof(double), typeof(Controller_87024), new PropertyMetadata(0.0));

        #endregion

        #region Ch2

        public double Ch2
        {
            get { return (double)GetValue(Ch2Property); }
            set { SetValue(Ch2Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch2.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch2Property =
            DependencyProperty.Register("Ch2", typeof(double), typeof(Controller_87024), new PropertyMetadata(0.0));

        #endregion

        #region Ch3

        public double Ch3
        {
            get { return (double)GetValue(Ch3Property); }
            set { SetValue(Ch3Property, value); }
        }

        // Using a DependencyProperty as the backing store for Ch3.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty Ch3Property =
            DependencyProperty.Register("Ch3", typeof(double), typeof(Controller_87024), new PropertyMetadata(0.0));

        #endregion

    }
}
