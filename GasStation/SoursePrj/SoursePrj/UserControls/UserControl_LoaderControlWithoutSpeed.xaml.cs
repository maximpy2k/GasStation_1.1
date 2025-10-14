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
    /// Interaction logic for UserControl_LoaderControlWithoutSpeed.xaml
    /// </summary>
    public partial class UserControl_LoaderControlWithoutSpeed : UserControl
    {
        public UserControl_LoaderControlWithoutSpeed()
        {
            InitializeComponent();
        }

        private void MenuItemParams(object sender, RoutedEventArgs e)
        {
            var win = new WindowLoaderParams();
            if (DataContext == null)
            {
                return;
            }

            var loader = (XmlClassLoader)DataContext;

            win.DataContext = loader;
            win.ShowDialog();
        }

        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !((Char.IsDigit(e.Text, 0) || ((e.Text == System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString()) &&
                                                       (DS_Count(((TextBox)sender).Text) < 1))));
        }

        private Visibility visible = Visibility.Collapsed;

        public Visibility Visible
        {
            get
            {
                return visible;
            }
            set
            {
                visible = value;
            }
        }

        public int DS_Count(string s)
        {
            string substr = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0].ToString();
            int count = (s.Length - s.Replace(substr, "").Length) / substr.Length;
            return count;
        }
    }
}
