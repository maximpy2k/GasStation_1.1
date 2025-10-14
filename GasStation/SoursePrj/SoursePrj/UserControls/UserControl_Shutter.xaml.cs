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
using GasStation.ModalWindows;
using GasStation.xml.Script;
using System.Windows;
using GasStation.ModalWindows.View.WindowsParams;

namespace SoursePrj
{
    /// <summary>
    /// Interaction logic for UserControl_Shutter.xaml
    /// </summary>
    public partial class UserControl_Shutter : UserControl
    {
        public UserControl_Shutter()
        {
            InitializeComponent();
        }

        private void MenuItemOpenProp_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowShutterParams();
            if (DataContext == null)
            {
                return;
            }
            win.DataContext = DataContext;
            win.ShowDialog();
        }
    }


}
