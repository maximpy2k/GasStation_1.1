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
using GasStation.xml.Script;

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_Barboter.xaml
    /// </summary>
    public partial class UserControl_Bubbler_FullControl : UserControl
    {
        public UserControl_Bubbler_FullControl()
        {
            InitializeComponent();
        }

        private void CheckBox_Heating_Checked(object sender, RoutedEventArgs e)
        {

        }
        private void MenuItemOpenView_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowBubblerView();
            if (DataContext == null)
            {
                return;
            }
            var bubbler = (XmlClassBubbler)DataContext;

            bubbler.BubblerView.BigName = $"{bubbler.BubblerConst.RusName} {bubbler.BubblerConst.DevNum}";
            win.DataContext = bubbler.BubblerView;
            win.ShowDialog();
        }

        private void MenuItemOpenParams_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowBubblerParams();
            if (DataContext == null)
            {
                return;
            }
            var bubbler = (XmlClassBubbler)DataContext;

            win.DataContext = bubbler;
            win.ShowDialog();
        }

        private void TextBox_SetTemp_Copy_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((Char.IsDigit(e.Text, 0) || ((e.Text == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString()) && (DS_Count(((TextBox)sender).Text) < 1))));
        }

        public int DS_Count(string s)
        {
            string substr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }
    }
}
