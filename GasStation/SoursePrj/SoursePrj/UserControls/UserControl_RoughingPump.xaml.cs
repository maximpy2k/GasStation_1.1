using System.Windows;
using System.Windows.Controls;
using GasStation.ModalWindows.View;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_ForVacuumPump.xaml
    /// </summary>
    public partial class UserControl_RoughingPump : UserControl
    {
        public UserControl_RoughingPump()
        {
            InitializeComponent();
        }

        private void MenuItemOpenView_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowPumpView();
            if (DataContext == null)
            {
                return;
            }
            var bubbler = (XmlClassPumpConst)DataContext;

            //bubbler.PumpSysView.BigName = $"{bubbler.RusName} {bubbler.DevNum}";
            //win.DataContext = bubbler.PumpSysView;
            win.ShowDialog();
        }

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
