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
    /// Interaction logic for WindowRRGView.xaml
    /// </summary>
    public partial class WindowRrgView : Window
    {
        public WindowRrgView()
        {
            InitializeComponent();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            chart1.DeleteEvents();
        }
    }
}
