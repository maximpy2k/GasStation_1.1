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

namespace GasStation.Elements.Controllers
{
    /// <summary>
    /// Interaction logic for Controller_Tehmash_power.xaml
    /// </summary>
    public partial class Controller_Tehmash_power : UserControl
    {
        public Controller_Tehmash_power()
        {
            InitializeComponent();
        }
        public String Num
        {
            set { ContrNum.Text = value; }
        }

        public int SetPower
        {
            get { return (int)GetValue(SetPowerValueProperty); }
            set { SetValue(SetPowerValueProperty, value); }
        }

        public static readonly DependencyProperty SetPowerValueProperty =
            DependencyProperty.Register("SetPower", typeof(bool), typeof(Controller_Tehmash_power), new FrameworkPropertyMetadata(false) { BindsTwoWayByDefault = true, DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

    }
}
