using System.Windows;
using System.Windows.Controls;
using GasStation.ModalWindows.View;
using GasStation.xml.Script;

namespace SoursePrj
{
    /// <summary>
    /// Interaction logic for UserControl_Flap.xaml
    /// </summary>
    public partial class UserControl_FlapNoControl : UserControl
    {
        public UserControl_FlapNoControl()
        {
            InitializeComponent();         
        }

        private void MenuParams(object sender, RoutedEventArgs e)
        {
            var win = new WindowFlapParams();
            if (DataContext == null)
            {
                return;
            }

            var flap = (XmlClassFlap)DataContext;

            win.DataContext = flap;
            win.ShowDialog();
        }

    }
}
