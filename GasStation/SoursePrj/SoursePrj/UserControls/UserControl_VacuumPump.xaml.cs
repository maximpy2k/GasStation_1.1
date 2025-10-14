using System.Windows;
using System.Windows.Controls;
using GasStation.ModalWindows.View;
using GasStation.xml.Script;

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_VacuumPump.xaml
    /// </summary>
    public partial class UserControl_VacuumPump : UserControl
    {
        public UserControl_VacuumPump()
        {
            InitializeComponent();
        }

        //private void checkBox2_Checked(object sender, RoutedEventArgs e)
        //{

        //}

        //private void MenuItemOpenView_Click(object sender, RoutedEventArgs e)
        //{
        //    var win = new WindowPumpView();
        //    if (DataContext == null)
        //    {
        //        return;
        //    }
        //    var bubbler = (XmlClassPumpSys)DataContext;

        //    bubbler.PumpSysView.BigName = $"{bubbler.PumpSysConst.RusName} {bubbler.PumpSysConst.DevNum}";
        //    win.DataContext = bubbler.PumpSysView;
        //    win.ShowDialog();
        //}

        private void MenuItemOpenParams_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowPumpParams();
            if (DataContext == null)
            {
                return;
            }
            var bubbler = (XmlClassPumpSys)DataContext;

            win.DataContext = bubbler;
            win.ShowDialog();
        }
    }
}
