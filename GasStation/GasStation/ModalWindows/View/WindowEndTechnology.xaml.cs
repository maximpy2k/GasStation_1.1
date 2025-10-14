using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GasStation.ModalWindows.View
{
    /// <summary>
    /// Interaction logic for WindowEndTechnology.xaml
    /// </summary>
    public partial class WindowEndTechnology:Window
    {
        public WindowEndTechnology()
        {
            InitializeComponent();
        }

        public DialogResult dialogResult;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            dialogResult=System.Windows.Forms.DialogResult.OK;
            Close();
        }
    }
}
