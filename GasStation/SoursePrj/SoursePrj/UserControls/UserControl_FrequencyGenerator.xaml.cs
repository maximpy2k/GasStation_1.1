using System.Windows;
using System.Windows.Controls;
using GasStation.ModalWindows.View.WindowsParams;

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_FrequencyGenerator.xaml
    /// </summary>
    public partial class UserControl_FrequencyGenerator : UserControl
    {
        public UserControl_FrequencyGenerator()
        {
            InitializeComponent();
        }

        private void MenuItemOpenProp_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowFreqGeneratorParams();
            if (DataContext == null)
            {
                return;
            }
            win.DataContext = DataContext;
            win.ShowDialog();
        }
    }

}
