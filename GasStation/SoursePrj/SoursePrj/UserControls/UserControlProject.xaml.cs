using GasStation.ViewModels;
using GasStation.ViewModels.Converters;
using GasStation.xml.Const.Elements;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.Constant;
using GasStation.xml.Script.XmlScript;
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

namespace SoursePrj.UserControls
{
    /// <summary>
    /// Interaction logic for UserControlProject.xaml
    /// </summary>
    public partial class UserControlProject : UserControl
    {
        public UserControlProject()
        {
            InitializeComponent();
        }


        ResourceDictionary rd;
        MainWindowViewModel viewMod;
        private void Lst_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            rd = new ResourceDictionary() { Source = new Uri("/GasStation;component/Styles/HierarchicalDataTemplate/ConstDictionary.xaml", UriKind.Relative) };

            viewMod = (MainWindowViewModel)(e.NewValue);

            GridView gridView = new GridView();

            gridView.Columns.Add(NewColStepNum());
            gridView.Columns.Add(NewColStepName());
            gridView.Columns.Add(NewColIntervalType());
            gridView.Columns.Add(NewColTimeOfInterval());
            gridView.Columns.Add(NewColBranch(viewMod));

            foreach (var pumpSys in viewMod.ClsScript.Consts.PumpSysConst)
            {
                gridView.Columns.Add(NewColVacuumPump(pumpSys));
                gridView.Columns.Add(NewColRoughingPump(pumpSys));
            }

            foreach (var cham in viewMod.ClsScript.Consts.ConstCham)
                gridView.Columns.Add(NewCol(cham));

            foreach (var burner in viewMod.ClsScript.Consts.ConstBurner)
                gridView.Columns.Add(NewCol(burner));

            foreach (var rrg in viewMod.ClsScript.Consts.ConstRrgs)
                gridView.Columns.Add(NewCol(rrg));

            foreach (var freqGen in viewMod.ClsScript.Consts.ConstFreqGenerator)
                gridView.Columns.Add(NewCol(freqGen));

            foreach (var shutter in viewMod.ClsScript.Consts.ConstShutters)
                gridView.Columns.Add(NewCol(shutter));

            foreach (var flap in viewMod.ClsScript.Consts.ConstFlaps)
                gridView.Columns.Add(NewCol(flap));

            foreach (var bubbler in viewMod.ClsScript.Consts.ConstBubblers)
                gridView.Columns.Add(NewCol(bubbler));

            foreach (var loader in viewMod.ClsScript.Consts.ConstLoader)
                gridView.Columns.Add(NewCol(loader));

            foreach (var gate in viewMod.ClsScript.Consts.GateConst)
            {
                if (gate.GateControl)
                    gridView.Columns.Add(NewCol(gate));
            }


            Lst.View = gridView;
        }

        /// <summary>
        /// Добавление в проект новой колонки название интервала
        /// </summary>
        /// <returns></returns>
        private GridViewColumn NewColStepNum()
        {
            FrameworkElementFactory txtBox = new FrameworkElementFactory(typeof(TextBlock));
            txtBox.SetBinding(TextBlock.TextProperty, new Binding($"StepParams.NumStep") { Mode = BindingMode.OneWay });
            txtBox.SetValue(WidthProperty, 15.0);

            DataTemplate template = new DataTemplate();
            template.VisualTree = txtBox;
            GridViewColumn col = new GridViewColumn();
            col.Header = "№";
            col.CellTemplate = template;
            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки название интервала
        /// </summary>
        /// <returns></returns>
        private GridViewColumn NewColStepName()
        {
            FrameworkElementFactory txtBox = new FrameworkElementFactory(typeof(TextBox));
            txtBox.SetBinding(TextBox.TextProperty, new Binding($"StepParams.NameStep") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBox.SetValue(WidthProperty, 190.0);

            DataTemplate template = new DataTemplate();
            template.VisualTree = txtBox;
            GridViewColumn col = new GridViewColumn();
            col.Header = "Название интервала";
            col.CellTemplate = template;
            return col;
        }

        ClassConverterStepParams convStepParams = new ClassConverterStepParams();

        /// <summary>
        /// Добавление в проект новой колонки типа интервала
        /// </summary>
        /// <returns></returns>
        private GridViewColumn NewColIntervalType()
        {
            FrameworkElementFactory cmboBox = new FrameworkElementFactory(typeof(ComboBox));
            cmboBox.SetValue(WidthProperty, 100.0);
            cmboBox.SetValue(ComboBox.ItemsSourceProperty, convStepParams.Items);
            cmboBox.SetValue(StyleProperty, rd["IntervalType"]);
            cmboBox.SetBinding(ComboBox.IsEnabledProperty, new Binding("StepParams.EnabledChangedScript") { Mode = BindingMode.OneWay });
            cmboBox.SetBinding(ComboBox.TextProperty, new Binding("StepParams.TypeStep") { Mode = BindingMode.TwoWay, Converter = convStepParams });

            DataTemplate template = new DataTemplate();
            template.VisualTree = cmboBox;
            GridViewColumn col = new GridViewColumn();
            col.Header = "Тип интервала";
            col.CellTemplate = template;
            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки Время интервала
        /// </summary>
        /// <returns></returns>
        private GridViewColumn NewColTimeOfInterval()
        {
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory txtBoxHow = new FrameworkElementFactory(typeof(TextBox));
            txtBoxHow.SetValue(WidthProperty, 30.0);
            txtBoxHow.SetBinding(TextBox.TextProperty, new Binding("StepParams.TimeStepH") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            FrameworkElementFactory txtBoxMin = new FrameworkElementFactory(typeof(TextBox));
            txtBoxMin.SetValue(WidthProperty, 30.0);
            txtBoxMin.SetBinding(TextBox.TextProperty, new Binding("StepParams.TimeStepM") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            FrameworkElementFactory txtBoxSec = new FrameworkElementFactory(typeof(TextBox));
            txtBoxSec.SetValue(WidthProperty, 30.0);
            txtBoxSec.SetBinding(TextBox.TextProperty, new Binding("StepParams.TimeStepS") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            templateCell.AppendChild(txtBoxHow);
            templateCell.AppendChild(txtBoxMin);
            templateCell.AppendChild(txtBoxSec);

            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = "Время интервала";
            col.CellTemplate = template;
            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки Условные переходы
        /// </summary>
        /// <returns></returns>
        private GridViewColumn NewColBranch(MainWindowViewModel viewMod)
        {
            FrameworkElementFactory btn = new FrameworkElementFactory(typeof(Button));
            btn.SetValue(Button.ContentProperty, "Условия перехода");
            btn.SetBinding(Button.CommandParameterProperty, new Binding());
            btn.SetBinding(Button.CommandProperty, new Binding("CmdGoBranch") { Source = viewMod });

            DataTemplate template = new DataTemplate();
            template.VisualTree = btn;
            GridViewColumn col = new GridViewColumn();
            col.Header = "Усл. Переход";
            col.CellTemplate = template;
            return col;
        }

        private ContextMenu CreateContextMenu(string path, ICommand cmd)
        {
            ContextMenu ctMenu = new ContextMenu();
            MenuItem menuItem0 = new MenuItem() { Header = "Настройки", Command = cmd };
            menuItem0.SetBinding(MenuItem.CommandParameterProperty, new Binding(path));
            ctMenu.Items.Add(menuItem0);
            return ctMenu;
        }
        /// <summary>
        /// Добавление в проект новой колонки Вакуумный насос
        /// </summary>
        /// <returns></returns>
        private GridViewColumn NewColVacuumPump(XmlClassPumpSysConst con)
        {
            FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
            chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"PumpSys[{con.DevNumMas}].UseVacuumPump") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox.SetValue(HeightProperty, 20.0);
            chkBox.SetValue(WidthProperty, 20.0);
            chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);
            //chkBox.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"PumpSys[{con.DevNumMas}]", viewMod.CmdPumpView Params));

            DataTemplate template = new DataTemplate();
            template.VisualTree = chkBox;
            GridViewColumn col = new GridViewColumn();
            col.Header = "Форвак.насос";
            col.CellTemplate = template;
            return col;
        }
        /// <summary>
        /// Добавление в проект новой колонки Форвакуумный насос
        /// </summary>
        /// <returns></returns>
        private GridViewColumn NewColRoughingPump(XmlClassPumpSysConst con)
        {
            FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
            chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"PumpSys[{con.DevNumMas}].UseRoughingPump") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox.SetValue(HeightProperty, 20.0);
            chkBox.SetValue(WidthProperty, 20.0);
            chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);

            DataTemplate template = new DataTemplate();
            template.VisualTree = chkBox;
            GridViewColumn col = new GridViewColumn();
            col.Header = "Вак.насос";
            col.CellTemplate = template;
            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки генератора
        /// </summary>
        /// <param name="con">Класс констант генератора</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassFreqGeneratorConst con)
        {
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory txtBoxImpulsDuration1 = new FrameworkElementFactory(typeof(TextBox));
            txtBoxImpulsDuration1.SetValue(WidthProperty, 20.0);
            txtBoxImpulsDuration1.SetBinding(TextBox.TextProperty, new Binding($"FreqGenerators[{con.DevNumMas}].ImpulsDuration1") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBoxImpulsDuration1.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"FreqGenerators[{con.DevNumMas}]", viewMod.CmdStepParamsFreqGenerator));

            FrameworkElementFactory txtBoxImpulsDelay1 = new FrameworkElementFactory(typeof(TextBox));
            txtBoxImpulsDelay1.SetValue(WidthProperty, 20.0);
            txtBoxImpulsDelay1.SetBinding(TextBox.TextProperty, new Binding($"FreqGenerators[{con.DevNumMas}].ImpulsDelay1") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBoxImpulsDelay1.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"FreqGenerators[{con.DevNumMas}]", viewMod.CmdStepParamsFreqGenerator));

            FrameworkElementFactory txtBoxImpulsDuration2 = new FrameworkElementFactory(typeof(TextBox));
            txtBoxImpulsDuration2.SetValue(WidthProperty, 20.0);
            txtBoxImpulsDuration2.SetBinding(TextBox.TextProperty, new Binding($"FreqGenerators[{con.DevNumMas}].ImpulsDuration2") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBoxImpulsDuration2.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"FreqGenerators[{con.DevNumMas}]", viewMod.CmdStepParamsFreqGenerator));

            FrameworkElementFactory txtBoxImpulsDelay2 = new FrameworkElementFactory(typeof(TextBox));
            txtBoxImpulsDelay2.SetValue(WidthProperty, 20.0);
            txtBoxImpulsDelay2.SetBinding(TextBox.TextProperty, new Binding($"FreqGenerators[{con.DevNumMas}].ImpulsDelay2") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBoxImpulsDelay2.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"FreqGenerators[{con.DevNumMas}]", viewMod.CmdStepParamsFreqGenerator));

            FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
            chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"FreqGenerators[{con.DevNumMas}].StartupSwitch") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            chkBox.SetValue(HeightProperty, 20.0);
            chkBox.SetValue(WidthProperty, 20.0);
            chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);

            templateCell.AppendChild(txtBoxImpulsDuration1);
            templateCell.AppendChild(txtBoxImpulsDelay1);
            templateCell.AppendChild(txtBoxImpulsDuration2);
            templateCell.AppendChild(txtBoxImpulsDelay2);
            templateCell.AppendChild(chkBox);


            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }

        private GridViewColumn NewCol(XmlClassGateConst con)
        {
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory chkBoxLeft = new FrameworkElementFactory(typeof(CheckBox));
            chkBoxLeft.SetValue(CheckBox.IsCheckedProperty, new Binding($"Gates[{con.DevNumMas}].GateOpen") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBoxLeft.SetValue(HeightProperty, 20.0);
            chkBoxLeft.SetValue(WidthProperty, 44.0);
            chkBoxLeft.SetValue(StyleProperty, rd["CheckBoxGateOpen"]);


            FrameworkElementFactory chkBoxRight = new FrameworkElementFactory(typeof(CheckBox));
            chkBoxRight.SetValue(CheckBox.IsCheckedProperty, new Binding($"Gates[{con.DevNumMas}].GateClose") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBoxRight.SetValue(HeightProperty, 20.0);
            chkBoxRight.SetValue(WidthProperty, 46.0);
            chkBoxRight.SetValue(StyleProperty, rd["CheckBoxGateClose"]);

            templateCell.AppendChild(chkBoxLeft);
            templateCell.AppendChild(chkBoxRight);

            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }
        /// <summary>
        /// Добавление в проект новой колонки РРГ
        /// </summary>
        /// <param name="con">Класс констант РРГ</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassChamberConst con)
        {      
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory txtBox = new FrameworkElementFactory(typeof(TextBox));
            txtBox.SetValue(WidthProperty, 40.0);
            txtBox.SetBinding(TextBox.TextProperty, new Binding($"Chambers[{con.DevNumMas}].SetupTemp") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBox.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"Chambers[{con.DevNumMas}]", viewMod.ComStepParamsChamber));

            FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
            chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"Chambers[{con.DevNumMas}].Heat") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox.SetValue(HeightProperty, 20.0);
            chkBox.SetValue(WidthProperty, 20.0);
            chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);

            templateCell.AppendChild(txtBox);
            templateCell.AppendChild(chkBox);


            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки РРГ
        /// </summary>
        /// <param name="con">Класс констант РРГ</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassHydrogenBurnerConst con)
        {
           

            FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
            chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"Burners[{con.DevNumMas}].Heat") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox.SetValue(HeightProperty, 20.0);
            chkBox.SetValue(WidthProperty, 20.0);
            chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);

            DataTemplate template = new DataTemplate();
            template.VisualTree = chkBox;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки РРГ
        /// </summary>
        /// <param name="con">Класс констант РРГ</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassShutterConst con)
        {
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory chkBox0 = new FrameworkElementFactory(typeof(CheckBox));
            chkBox0.SetValue(CheckBox.IsCheckedProperty, new Binding($"Shutters[{con.DevNumMas}].Flaps[0].FlapView") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox0.SetValue(HeightProperty, 20.0);
            chkBox0.SetValue(WidthProperty, 20.0);
            chkBox0.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);
            //chkBox.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"Shutters[{con.DevNumMas}]", viewMod.Cmds));

            FrameworkElementFactory chkBox1 = new FrameworkElementFactory(typeof(CheckBox));
            chkBox1.SetValue(CheckBox.IsCheckedProperty, new Binding($"Shutters[{con.DevNumMas}].Flaps[1].FlapView") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox1.SetValue(HeightProperty, 20.0);
            chkBox1.SetValue(WidthProperty, 20.0);
            chkBox1.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);

            templateCell.AppendChild(chkBox0);
            templateCell.AppendChild(chkBox1);


            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки РРГ
        /// </summary>
        /// <param name="con">Класс констант РРГ</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassRrgConst con)
        {
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory txtBox = new FrameworkElementFactory(typeof(TextBox));
            txtBox.SetValue(WidthProperty, 40.0);
            txtBox.SetBinding(TextBox.TextProperty, new Binding($"Rrgs[{con.DevNumMas}].SetupValue") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBox.SetValue(CheckBox.IsEnabledProperty, new Binding($"ClsScript.Consts.ConstRrgs[{con.DevNumMas}].IsFlap") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBox.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"Rrgs[{con.DevNumMas}]", viewMod.CmdRrgStepParams));
            if (con.IsFlap)
            {
                FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
                chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"Rrgs[{con.DevNumMas}].Flap.FlapView") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                chkBox.SetValue(CheckBox.IsEnabledProperty, new Binding($"ClsScript.Consts.ConstRrgs[{con.DevNumMas}].IsFlap") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
                chkBox.SetValue(HeightProperty, 20.0);
                chkBox.SetValue(WidthProperty, 20.0);
                chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);
                templateCell.AppendChild(chkBox);
            }
            templateCell.AppendChild(txtBox);
            


            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки клапана
        /// </summary>
        /// <param name="rrgCon">Класс констант клапана</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassFlapConst con)
        {
            FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
            chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"Flaps[{con.DevNumMas}].FlapView") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox.SetValue(HeightProperty, 20.0);
            chkBox.SetValue(WidthProperty, 20.0);
            chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);
            chkBox.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"Flaps[{con.DevNumMas}]", viewMod.CmdStepParamsFlap));


            DataTemplate template = new DataTemplate();
            template.VisualTree = chkBox;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки барботера
        /// </summary>
        /// <param name="con">Класс констант барботера</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassBubblerConst con)
        {
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory txtBox = new FrameworkElementFactory(typeof(TextBox));
            txtBox.SetValue(WidthProperty, 40.0);
            txtBox.SetBinding(TextBox.TextProperty, new Binding($"Bubblers[{con.DevNumMas}].SetupValue") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });

            FrameworkElementFactory chkBox = new FrameworkElementFactory(typeof(CheckBox));
            chkBox.SetValue(CheckBox.IsCheckedProperty, new Binding($"Bubblers[{con.DevNumMas}].UseBubbler") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBox.SetValue(HeightProperty, 20.0);
            chkBox.SetValue(WidthProperty, 20.0);
            chkBox.SetValue(StyleProperty, rd["CheckBoxFlapStyle"]);

            templateCell.AppendChild(txtBox);
            templateCell.AppendChild(chkBox);

            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }

        /// <summary>
        /// Добавление в проект новой колонки клапана
        /// </summary>
        /// <param name="rrgCon">Класс констант клапана</param>
        /// <returns></returns>
        private GridViewColumn NewCol(XmlClassLoaderConst con)
        {
            FrameworkElementFactory templateCell = new FrameworkElementFactory(typeof(StackPanel));
            templateCell.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory chkBoxLeft = new FrameworkElementFactory(typeof(CheckBox));
            chkBoxLeft.SetValue(CheckBox.IsCheckedProperty, new Binding($"Loaders[{con.DevNumMas}].DestLoad") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBoxLeft.SetValue(HeightProperty, 20.0);
            chkBoxLeft.SetValue(WidthProperty, 20.0);
            chkBoxLeft.SetValue(StyleProperty, rd["CheckBoxLoad"]);

            FrameworkElementFactory txtBox = new FrameworkElementFactory(typeof(TextBox));
            txtBox.SetValue(WidthProperty, 40.0);
            txtBox.SetBinding(TextBox.TextProperty, new Binding($"Loaders[{con.DevNumMas}].SetupSpeed") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            txtBox.SetValue(TextBox.ContextMenuProperty, CreateContextMenu($"Loaders[{con.DevNumMas}]", viewMod.CmdStepParamsLoader));

            FrameworkElementFactory chkBoxRight = new FrameworkElementFactory(typeof(CheckBox));
            chkBoxRight.SetValue(CheckBox.IsCheckedProperty, new Binding($"Loaders[{con.DevNumMas}].DestUnload") { Mode = BindingMode.TwoWay, UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged });
            chkBoxRight.SetValue(HeightProperty, 20.0);
            chkBoxRight.SetValue(WidthProperty, 20.0);
            chkBoxRight.SetValue(StyleProperty, rd["CheckBoxUnLoad"]);

            templateCell.AppendChild(chkBoxLeft);
            templateCell.AppendChild(txtBox);
            templateCell.AppendChild(chkBoxRight);

            DataTemplate template = new DataTemplate();
            template.VisualTree = templateCell;
            GridViewColumn col = new GridViewColumn();
            col.Header = $"{con.RusName} {con.DevNum}";
            col.CellTemplate = template;

            return col;
        }
    }
}
