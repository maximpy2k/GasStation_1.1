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
    /// Interaction logic for Controller_Tehmash.xaml
    /// </summary>
    public partial class Controller_Tehmash_ports : UserControl
    {
        public Controller_Tehmash_ports()
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
            DependencyProperty.Register("Ch0", typeof(bool), typeof(Controller_Tehmash_ports), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion

        #region Ch1
        public bool Ch1
        {
            get { return (bool)GetValue(Ch1ValueProperty); }
            set { SetValue(Ch1ValueProperty, value); }
        }

        public static readonly DependencyProperty Ch1ValueProperty =
            DependencyProperty.Register("Ch1", typeof(bool), typeof(Controller_Tehmash_ports), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
        #endregion
    }
}
