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
    /// Interaction logic for Controller_Shim.xaml
    /// </summary>
    public partial class Controller_Shim : UserControl
    {
        public Controller_Shim()
        {
            InitializeComponent();
        }
        public String Num
        {
            set { ContrNum.Text = value; }
        }


    }
}
