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
using System.Windows.Shapes;

namespace GasStation.ModalWindows.View
{
    /// <summary>
    /// Interaction logic for WindowPassword.xaml
    /// </summary>
    public partial class WindowPassword : Window
    {
        public WindowPassword()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Введенный пароль
        /// </summary>
        public string EnteredPass { get { return passBox.Password; } }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
