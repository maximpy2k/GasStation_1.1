using GasStation.Elements.ViewModels;
using GasStation.xml;
using GasStation.xml.Constant.XmlConst.Elements;
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
using GasStation.xml.Constant;
using StationImitation.ViewModel;

namespace StationImitation.ControllerElements
{
    /// <summary>
    /// Interaction logic for UserControlControllers.xaml
    /// </summary>
    public partial class UserControlControllers : UserControl
    {
        public UserControlControllers()
        {
            InitializeComponent();
        }


        private void UserControl_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {

            var script = DataContext as ClassConstView;

            if (script == null)
                return;
            var con = script.ClassXmlConst.ConstControllers;
            
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
