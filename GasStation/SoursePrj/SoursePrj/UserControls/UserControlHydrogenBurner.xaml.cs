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
using GasStation.ModalWindows.View;
using GasStation.ModalWindows.View.WindowsParams;
using GasStation.xml.Script.XmlScript;

namespace SoursePrj
{
    /// <summary>
    /// Interaction logic for UserControlHydrogenBurner.xaml
    /// </summary>
    public partial class UserControlHydrogenBurner : UserControl
    {
        public UserControlHydrogenBurner()
        {
            InitializeComponent();
        }

        public void OpenBurnerView(object properties, EventArgs e)
        {
            var win = new WindowHydrogenBurnerView();
            if (DataContext == null)
            {
                return;
            }
            var burner = (XmlClassHydrogenBurning)DataContext;
            win.DataContext = DataContext;
            //var burner = (XmlClassHydrogenBurning)DataContext;
            //win.DataContext = burner.HydrogenBurnerView;

            win.ShowDialog();
        }

        public void OpenBurnerParams(object properties, EventArgs e)
        {
            var win = new WindowHydrogenBurnerParams();
            if (DataContext == null)
            {
                return;
            }
            //var burner = (XmlClassHydrogenBurning)DataContext;
            win.DataContext = DataContext;//burner.HydrogenBurnerView;

            win.ShowDialog();
        }
    }
}
