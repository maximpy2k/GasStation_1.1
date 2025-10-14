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
    /// Interaction logic for Controller_87053.xaml
    /// </summary>
    public partial class Controller_87053 : UserControl
    {
        public Controller_87053()
        {
            InitializeComponent();
        }

        public String Num
        {
            set { ContrNum.Text = value; }
        }

        #region Ch0
        public bool Ch0
        {
            get { return (bool)GetValue(Ch0ValueProperty); }
            set { SetValue(Ch0ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch0ValueProperty =
            DependencyProperty.Register("Ch0", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch1
        public bool Ch1
        {
            get { return (bool)GetValue(Ch1ValueProperty); }
            set { SetValue(Ch1ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch1ValueProperty =
            DependencyProperty.Register("Ch1", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch2
        public bool Ch2
        {
            get { return (bool)GetValue(Ch2ValueProperty); }
            set { SetValue(Ch2ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch2ValueProperty =
            DependencyProperty.Register("Ch2", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch3
        public bool Ch3
        {
            get { return (bool)GetValue(Ch3ValueProperty); }
            set { SetValue(Ch3ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch3ValueProperty =
            DependencyProperty.Register("Ch3", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch4
        public bool Ch4
        {
            get { return (bool)GetValue(Ch4ValueProperty); }
            set { SetValue(Ch4ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch4ValueProperty =
            DependencyProperty.Register("Ch4", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch5
        public bool Ch5
        {
            get { return (bool)GetValue(Ch5ValueProperty); }
            set { SetValue(Ch5ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch5ValueProperty =
            DependencyProperty.Register("Ch5", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch6
        public bool Ch6
        {
            get { return (bool)GetValue(Ch6ValueProperty); }
            set { SetValue(Ch6ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch6ValueProperty =
            DependencyProperty.Register("Ch6", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch7
        public bool Ch7
        {
            get { return (bool)GetValue(Ch7ValueProperty); }
            set { SetValue(Ch7ValueProperty, value); }
        }   

        public static readonly DependencyProperty Ch7ValueProperty =
            DependencyProperty.Register("Ch7", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch8
        public bool Ch8
        {
            get { return (bool)GetValue(Ch8ValueProperty); }
            set { SetValue(Ch8ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch8ValueProperty =
            DependencyProperty.Register("Ch8", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch9
        public bool Ch9
        {
            get { return (bool)GetValue(Ch9ValueProperty); }
            set { SetValue(Ch9ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch9ValueProperty =
            DependencyProperty.Register("Ch9", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch10
        public bool Ch10
        {
            get { return (bool)GetValue(Ch10ValueProperty); }
            set { SetValue(Ch10ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch10ValueProperty =
            DependencyProperty.Register("Ch10", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch11
        public bool Ch11
        {
            get { return (bool)GetValue(Ch11ValueProperty); }
            set { SetValue(Ch11ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch11ValueProperty =
            DependencyProperty.Register("Ch11", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch12
        public bool Ch12
        {
            get { return (bool)GetValue(Ch12ValueProperty); }
            set { SetValue(Ch12ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch12ValueProperty =
            DependencyProperty.Register("Ch12", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch13
        public bool Ch13
        {
            get { return (bool)GetValue(Ch13ValueProperty); }
            set { SetValue(Ch13ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch13ValueProperty =
            DependencyProperty.Register("Ch13", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch14
        public bool Ch14
        {
            get { return (bool)GetValue(Ch14ValueProperty); }
            set { SetValue(Ch14ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch14ValueProperty =
            DependencyProperty.Register("Ch14", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch15
        public bool Ch15
        {
            get { return (bool)GetValue(Ch15ValueProperty); }
            set { SetValue(Ch15ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch15ValueProperty =
            DependencyProperty.Register("Ch15", typeof(bool), typeof(Controller_87053), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion
    }
}
