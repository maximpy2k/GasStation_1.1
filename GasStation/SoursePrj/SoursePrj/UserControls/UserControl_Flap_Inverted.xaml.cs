using System.Windows;
using System.Windows.Controls;
using GasStation.ModalWindows.View;
using GasStation.xml.Script;

namespace SoursePrj
{
    /// <summary>
    /// Interaction logic for UserControl_Flap.xaml
    /// </summary>
    public partial class UserControl_Flap_Inverted : UserControl
    {
        public UserControl_Flap_Inverted()
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
