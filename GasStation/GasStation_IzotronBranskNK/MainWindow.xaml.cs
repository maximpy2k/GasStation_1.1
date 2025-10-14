using GasStation.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GasStation_IzotronUlyanovskNK
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

        MainWindowViewModel viewmodel;

        private void MenuShowPrjWin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuShowSchemaWin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuShowSettWin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuShowGraphWin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuShowCtrlWin_Click(object sender, RoutedEventArgs e)
        {
        }

        private void MenuShowControlPanel_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Закрыть программу?", null, MessageBoxButton.YesNo, MessageBoxImage.Question);
            viewmodel = (MainWindowViewModel)FindResource("viewMod1");


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
