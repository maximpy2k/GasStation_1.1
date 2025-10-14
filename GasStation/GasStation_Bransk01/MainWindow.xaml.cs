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
using GasStation.ViewModels;
using System.Diagnostics;

namespace GasStation_Bransk01
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

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //viewmodel.ClassProcessingScript.AbortScript = true;
            var result = MessageBox.Show("Закрыть программу?", null, MessageBoxButton.YesNo, MessageBoxImage.Question);
            var viewmodel = (MainWindowViewModel)FindResource("viewMod");


            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            if (viewmodel.ClassProcessingScript != null)
                //viewmodel.ClassProcessingScript.AbortScript = true;
                viewmodel.ClassProcessingScript.Stop();
            Process.GetCurrentProcess().Kill();
        }
    }
}
