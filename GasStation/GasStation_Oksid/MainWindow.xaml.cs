using GasStation.ViewModels;
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
using GasStation;
using GasStation.Devices;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml;
using GasStation.ModalWindows.View;
using GasStation.xml.Script;

namespace GasStation_Oksid
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        private const double ZoomSpeed = 0.05;
        private const double MinZoom = 0.5;
        private const double MaxZoom = 5;
        private bool mouseEnter = false;
        private bool _isDragging = false;
        private Point _lastMousePosition;

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!mouseEnter)
                return;
            double zoomFactor=e.Delta>0?ZoomSpeed:-ZoomSpeed;
            double newZoom = ZoomTransform.ScaleX + zoomFactor;

            newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, newZoom));
            ZoomTransform.ScaleX = newZoom;
            ZoomTransform.ScaleY = newZoom;
            e.Handled = true;
        }
        private void ScrollViewer_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                _isDragging = true;
                _lastMousePosition = e.GetPosition(this);
                Mouse.OverrideCursor = Cursors.SizeAll;
                e.Handled = true;
            }
        }
        private void ScrollViewer_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDragging && ScrollViewerMain != null)
            {
                Point currentPosition = e.GetPosition(this);
                Vector delta = currentPosition - _lastMousePosition;

                // Применяем смещение к ScrollViewer
                ScrollViewerMain.ScrollToHorizontalOffset(ScrollViewerMain.HorizontalOffset - delta.X);
                ScrollViewerMain.ScrollToVerticalOffset(ScrollViewerMain.VerticalOffset - delta.Y);

                _lastMousePosition = currentPosition;
                e.Handled = true;
            }
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {


        }
        private void ScrollViewer_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDragging)
            {
                _isDragging = false;
                Mouse.OverrideCursor = null;
                e.Handled = true;
            }
        }
        MainWindowViewModel viewmodel;
        private void Pusk_Click(object sender, RoutedEventArgs e)
        {
            viewmodel = (MainWindowViewModel)FindResource("viewMod");
        }
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


        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
        }

        private void Параметры_Click(object sender, RoutedEventArgs e)
        {
            var item = (MenuItem)sender;
            var aa = item.DataContext;
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UserControl_Voronezj_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
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


        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
        }

        private void ScrollViewer_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseEnter = true;
        }

        private void ScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseEnter = false;
        }
    }
}


