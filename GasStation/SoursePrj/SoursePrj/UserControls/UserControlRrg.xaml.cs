using System;
using GasStation.ModalWindows.View;
using System.Windows;
using System.Windows.Controls;
using GasStation.xml.Script;

namespace SoursePrj
{
    /// <summary>
    /// Interaction logic for UserControlRrg.xaml
    /// </summary>
    public partial class UserControlRrg : UserControl
    {
        public UserControlRrg()
        {
            InitializeComponent();
        }


        private void MenuItemOpenProp_Click(object sender, RoutedEventArgs e)
        {
            var win = new WindowRrgParams();
            if (DataContext == null)
            {
                return;
            }
            win.DataContext = DataContext;
            win.ShowDialog();
        }

        private void MenuItemOpenView_Click(object sender, RoutedEventArgs e)
        {
            
            if (DataContext==null)
            {
                return;
            }
            var rrg = (XmlClassRrg) DataContext;


            if (rrg.RrgConst.Pid !=null)
            {
                var win = new WindowRrgPidView();

                rrg.RrgView.BigName = $"{rrg.RrgConst.RusName} {rrg.RrgConst.DevNum}";
                win.DataContext = rrg;
                win.ShowDialog();
            }
            else
            {
                var win = new WindowRrgView();

                rrg.RrgView.BigName = $"{rrg.RrgConst.RusName} {rrg.RrgConst.DevNum}";
                win.DataContext = rrg;
                win.ShowDialog();
            }


        }

        private void TextBox_SetTemp_Copy_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
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
