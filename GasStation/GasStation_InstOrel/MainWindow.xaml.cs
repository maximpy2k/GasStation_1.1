using GasStation.ViewModels;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;

namespace GasStation_InstOrel
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            var result = MessageBox.Show("Закрыть программу?", null, MessageBoxButton.YesNo, MessageBoxImage.Question);
            var viewmodel = (MainWindowViewModel)DataContext;


            if (result == MessageBoxResult.No)
            {
                e.Cancel = true;
                return;
            }
            if (viewmodel.ClassProcessingScript != null)
                viewmodel.ClassProcessingScript.Stop();
            Process.GetCurrentProcess().Kill();
        }
    }
}
