using GasStation.xml;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GasStation.Elements.Controllers
{
    /// <summary>
    /// Interaction logic for UserControlControllers.xaml
    /// </summary>
    public partial class UserControlControllers : UserControl
    {
        public UserControlControllers()
        {
            InitializeComponent();
            this.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        private const double ZoomSpeed = 0.05;
        private const double MinZoom = 1;
        private const double MaxZoom = 5;
        private bool mouseEnter = false;

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (!mouseEnter)
                return;
            double zoomFactor = e.Delta > 0 ? ZoomSpeed : -ZoomSpeed;
            double newZoom = ZoomTransform.ScaleX + zoomFactor;

            newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, newZoom));
            ZoomTransform.ScaleX = newZoom;
            ZoomTransform.ScaleY = newZoom;
            e.Handled = true;
        }

        private void ScrollViewer_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseEnter = true;
        }

        private void ScrollViewer_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseEnter = false;
        }

        //<contr:Controller_7018 Grid.Row="0" Grid.Column= "0" Grid.RowSpan= "2" Num= "1"
        //    DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[0]}" />
        //< contr:Controller_87017 Grid.Row= "0" Grid.Column= "1" Grid.RowSpan= "2" Num= "1"
        //                        DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[6]}" />
        //< contr:Controller_87053 Grid.Row= "0" Grid.Column= "2" Grid.RowSpan= "2" Num= "1"
        //                        DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[10]}" />
        //< contr:Controller_87057 Grid.Row= "0" Grid.Column= "3" Grid.RowSpan= "2" Num= "1"
        //                        DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[9]}" />
        //< Grid Grid.Row= "0" Grid.Column= "4" Grid.RowSpan= "2" >

        //    < Grid.RowDefinitions >
        //        < RowDefinition Height= "*" />
        //        < RowDefinition Height= "*" />
        //        < RowDefinition Height= "*" />
        //    </ Grid.RowDefinitions >

        //    < contr:Controller_Tehmash_power Grid.Row= "0" Num= "1" MinHeight= "50" Margin= "0,0,0,16"
        //                                    DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[1]}" />
        //    < contr:Controller_Tehmash_power Grid.Row= "1" Num= "2"
        //                                    DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[2]}" />
        //    < contr:Controller_Tehmash_power Grid.Row= "2" Num= "3" Margin= "0,16,0,0"
        //                                    DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[3]}" />
        //</ Grid >

        //< contr:Controller_Tehmash_ports Grid.Row= "0" Grid.Column= "5" Margin= "2,0"
        //                                    DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[4]}" />
        //< contr:Controller_Tehmash_portsDI Grid.Row= "1" Grid.Column= "5" Margin= "2,0"
        //                                  DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[5]}" />

        //< contr:Controller_87024 Grid.Row= "0" Grid.Column= "6" Margin= "2,0" Grid.RowSpan= "2"
        //                        DataContext= "{Binding Source={StaticResource viewMod1},Path=ClsScript.ViewData.ViewControllers[7]}" />
        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var script = DataContext as ClassScript;
            if (script == null)
                return;
            var con = script.Consts.ConstControllers;

            Controllers.Children.Clear();
            for (int i = 0; i < con.Length; i++)
            {
                switch (con[i].NameController)
                {
                    case "IDAS 7018":
                        var contr7018 = new Controller_7018();
                        var bind7018 = new Binding($"ViewData.ViewControllers[{i}]");
                        contr7018.SetBinding(DataContextProperty, bind7018);
                        Controllers.Children.Add(contr7018);
                        break;
                    case "IDAS 87057":
                        var contr87057 = new Controller_87057();
                        var bind87057 = new Binding($"ViewData.ViewControllers[{i}]");
                        contr87057.SetBinding(DataContextProperty, bind87057);
                        Controllers.Children.Add(contr87057);
                        break;
                    case "IDAS 87053":
                        var contr87053 = new Controller_87053();
                        var bind87053 = new Binding($"ViewData.ViewControllers[{i}]");
                        contr87053.SetBinding(DataContextProperty, bind87053);
                        Controllers.Children.Add(contr87053);
                        break;
                    case "IDAS 87024":
                        var contr87024 = new Controller_87024();
                        var bind87024 = new Binding($"ViewData.ViewControllers[{i}]");
                        contr87024.SetBinding(DataContextProperty, bind87024);
                        Controllers.Children.Add(contr87024);
                        break;
                    case "IDAS 87017":
                        var contr87017 = new Controller_87017();
                        var bind87017 = new Binding($"ViewData.ViewControllers[{i}]");
                        contr87017.SetBinding(DataContextProperty, bind87017);
                        Controllers.Children.Add(contr87017);
                        break;
                    case "TM 7042P":
                        var contr7042P = new Controller_Tehmash_power();
                        var bind7042P = new Binding($"ViewData.ViewControllers[{i}]");
                        contr7042P.SetBinding(DataContextProperty, bind7042P);
                        Controllers.Children.Add(contr7042P);
                        break;
                    case "TM 7042":
                        var contr7042 = new Controller_Tehmash_ports();
                        var bind7042 = new Binding($"ViewData.ViewControllers[{i}]");
                        contr7042.SetBinding(DataContextProperty, bind7042);
                        Controllers.Children.Add(contr7042);
                        break;
                    case "TM 7041":
                        var contr7041 = new Controller_Tehmash_portsDI();
                        var bind7041 = new Binding($"ViewData.ViewControllers[{i}]");
                        contr7041.SetBinding(DataContextProperty, bind7041);
                        Controllers.Children.Add(contr7041);
                        break;
                    case "TM SHIM":
                        var contrshim = new Controller_Shim();
                        var bindContrshim = new Binding($"ViewData.ViewControllers[{i}]");
                        contrshim.SetBinding(DataContextProperty, bindContrshim);
                        Controllers.Children.Add(contrshim);
                        break;
                }
            }                       
        }
    }
}
