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
using System.Windows.Shapes;

namespace GasStation.ModalWindows.View
{
    /// <summary>
    /// Interaction logic for WindowChamberView.xaml
    /// </summary>
    public partial class WindowChamberView : Window
    {
        public WindowChamberView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chart0.DeleteEvents();
            chart1.DeleteEvents();
            chart2.DeleteEvents();
        }


    }
}
