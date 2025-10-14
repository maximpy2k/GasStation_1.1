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


namespace SoursePrj
{
    /// <summary>
    /// Interaction logic for UserControlChamber.xaml
    /// </summary>
    public partial class UserControlChamber : UserControl
    {
        public UserControlChamber()
        {
            InitializeComponent();
        }

        private void MenuItemParams(object sender, RoutedEventArgs e)
        {
            var win = new WindowThermoChamberParams();
            if (DataContext==null)
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

        //private void MenuItemParams1_Click(object sender, RoutedEventArgs e)
        //{
        //    var win = new WindowThermoSectionParams();
        //    if (DataContext == null)
        //        return;
        //    var cham = (XmlClassChamber)DataContext;

        //    win.DataContext = cham.ChamberSections[0];
        //    win.ShowDialog();
        //}

        //private void MenuItemParams2_Click(object sender, RoutedEventArgs e)
        //{
        //    var win = new WindowThermoSectionParams();
        //    if (DataContext == null)
        //        return;
        //    var cham = (XmlClassChamber)DataContext;

        //    win.DataContext = cham.ChamberSections[1];
        //    win.ShowDialog();
        //}

        //private void MenuItemParams3_Click(object sender, RoutedEventArgs e)
        //{
        //    var win = new WindowThermoSectionParams();
        //    if (DataContext == null)
        //        return;
        //    var cham = (XmlClassChamber)DataContext;

        //    win.DataContext = cham.ChamberSections[2];
        //    win.ShowDialog();
        //}  
    }
}
