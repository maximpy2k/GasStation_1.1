using GasStation.Elements.Controllers;
using StationImitation.Controllers;
using StationImitation.xml;
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

namespace StationImitation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ControlConsts_Loaded(object sender, RoutedEventArgs e)
        {

        }

        //   private void buttonSp1_Click(object sender, RoutedEventArgs e)
        //   {
        //       //ClassImitator imit = new ClassImitator(textBox_sp1.Text);
        //       //imit.Start();
        //       //buttonSp1.IsEnabled = false;
        //   }
        //
        //   private void buttonSp2_Click(object sender, RoutedEventArgs e)
        //   {
        //       //ClassImitator imit = new ClassImitator(textBox_sp2.Text);
        //      // imit.Start();
        //       //buttonSp2.IsEnabled = false;
        //   }
        //
        //   private void buttonSp3_Click(object sender, RoutedEventArgs e)
        //   {
        //
        //     //  ClassImitator imit = new ClassImitator(textBox_sp3.Text);
        //     //  imit.Start();
        //     //  buttonSp3.IsEnabled = false;
        //
        //   }

    }
}
