using GasStation.ModalWindows.View;
using GasStation.xml.Script;
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

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControl_SmallChamber.xaml
    /// </summary>
    public partial class UserControl_SmallChamber : UserControl
    {
        public UserControl_SmallChamber()
        {
            InitializeComponent();
        }
        private void MenuItemParams(object sender, RoutedEventArgs e)
        {
            var win = new WindowThermoChamberParams();
            if (DataContext == null)
            {
                return;
            }

            var cham = (XmlClassChamber)DataContext;

            win.DataContext = cham;
            win.ShowDialog();
        }

        private void MenuItemViews(object sender, RoutedEventArgs e)
        {
            var win = new WindowChamberView();
            if (DataContext == null)
            {
                return;
            }

            var cham = (XmlClassChamber)DataContext;

            win.DataContext = cham;
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
