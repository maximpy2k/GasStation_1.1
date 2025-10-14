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

namespace StationImitation.ControllerElements
{
    /// <summary>
    /// Interaction logic for Controller_7018.xaml
    /// </summary>
    public partial class Controller_7018 : UserControl
    {
        public Controller_7018()
        {
            InitializeComponent();
        }
        public String Num
        {
            set { ContrNum.Text = value; }
        }
   

        #region Размерность

        public String Razmernost
        {
            get { return (String)GetValue(RazmernostProperty); }
            set { SetValue(RazmernostProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Razmernost.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RazmernostProperty =
            DependencyProperty.Register("Razmernost", typeof(String), typeof(Controller_7018), new PropertyMetadata("mV"));

        #endregion


      

    }
}
