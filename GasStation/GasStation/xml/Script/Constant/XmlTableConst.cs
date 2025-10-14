using GasStation.UserControls.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;

namespace GasStation.xml.Constant.XmlConst
{
    public class XmlTableConst : INotifyPropertyChanged
    {
        public XmlNode XmlNode { get; set; }

        public XmlTableConst(XmlNode xmlNode)
        {
            XmlNode = xmlNode;
            CmdAdd = new CommandClass(Add);
            CmdRemove = new CommandClass(Remove);
        }
        /// <summary>
        /// Комманда добавления новой точки в таблицу
        /// </summary>
        public CommandClass CmdAdd { get; set; }
        /// <summary>
        /// Комманда удаления точки из таблицы
        /// </summary>
        public CommandClass CmdRemove { get; set; }

        public ObservableCollection<XmlTableFild> points;
        /// <summary>
        /// Табличные значения
        /// </summary>
        public ObservableCollection<XmlTableFild> Points
        {
            get
            {
                if (points != null)
                    return points;

                var nodes = XmlNode.SelectNodes("field");
                points = new ObservableCollection<XmlTableFild>();
                for (var idx = 0; idx < nodes.Count; idx++)
                {
                    double x = -1;
                    double.TryParse(nodes[idx].Attributes["valueX"].Value, out x);

                    double y = -1;
                    double.TryParse(nodes[idx].Attributes["valueY"].Value, out y);

                    bool check = true;
                    bool.TryParse(nodes[idx].Attributes["check"].Value, out check);

                    points.Add( new XmlTableFild(x, y, check));
                    points.Last().PropertyChanged += Refrash;
                }
                

                return points;
            }

            set
            {
                Points = value;
            }
        }
        
        
        /// <summary>
        /// Используемые точки
        /// </summary>
        public List<XmlTableFild> UsedPoints
        {
            get
            {            
                return Points.Where(dat=>dat.Check).ToList();
            }            
        }
        /// <summary>
        /// Выбранное поле таблицы
        /// </summary>
        public XmlTableFild SelectedFild { get; set; }

        /// <summary>
        /// Добавление новой точки в таблицу
        /// </summary>
        /// <param name="parametr">Параметр не используется</param>
        public void Add(object parametr)
        {
            var x = Points.Count < 2 ? 0 : Points.Last().X;
            var y = Points.Count < 2 ? 0 : Points.Last().Y;
            var check= Points.Count < 2 ? false : Points.Last().Check;
            var xml = Points.Last().Node;
            var importNode = XmlNode.OwnerDocument.ImportNode(xml, true);
            XmlNode.AppendChild(importNode);
            Points.Add(new XmlTableFild(x, y, check));
            points.Last().PropertyChanged += Refrash;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UsedPoints"));
        }
        /// <summary>
        /// Удаление точки из таблицы
        /// </summary>
        /// <param name="parametr"></param>
        public void Remove(object parametr)
        {
            if (points.Count <= 2)
                return;
            if (SelectedFild == null)
                return;

            var idx = Points.IndexOf(SelectedFild);
            points[idx].PropertyChanged -= Refrash;

            var nodes = XmlNode.SelectNodes("field");
            XmlNode.RemoveChild(nodes[idx]);
            Points.Remove(SelectedFild);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UsedPoints"));
        }
        /// <summary>
        /// Обновление таблицы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Refrash(object sender, PropertyChangedEventArgs e)
        {
            var fild = (XmlTableFild)sender;
            var idx = Points.IndexOf(fild);
            var nodes = XmlNode.SelectNodes("field");
            nodes[idx].Attributes["valueX"].Value = fild.X.ToString();
            nodes[idx].Attributes["valueY"].Value = fild.Y.ToString();
            nodes[idx].Attributes["check"].Value = fild.Check.ToString();
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UsedPoints"));            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Получение значения с использованием таблицы
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public double GetVal(double arg)
        {
            var points = UsedPoints.OrderBy(dat => dat.X).Distinct().ToArray();

            if (points.Length == 0)
                return arg;

            if (arg < points[0].X)
                return arg;
            if (arg > points.Last().X)
                return arg;

            var minMas = points.Where(dat => dat.X <= arg).ToArray();
            var maxMas = points.Where(dat => dat.X > arg).ToArray();

            if (minMas.Length != 0 && maxMas.Length == 0)
                return minMas.Last().Y;
            if (minMas.Length == 0 && maxMas.Length != 0)
                return maxMas[0].Y;


            var beg = minMas.Last();
            var end = maxMas.First();

            var k = (beg.Y - end.Y) / (beg.X - end.X);
            var b = beg.Y - k * beg.X;

            return k * arg + b;
        }
        /// <summary>
        /// Максимальное значнение
        /// </summary>
        public double MaxVal => UsedPoints.Select(dat => dat.X).Max();
        /// <summary>
        /// Минимальное значение
        /// </summary>
        public double MinVal => UsedPoints.Select(dat => dat.X).Min();

        public string LegendX 
        {
            get
            {
                var legendX = "T(зад)";
                if(XmlNode.Attributes["rusNameX"]!=null)
                    legendX=XmlNode.Attributes["rusNameX"].Value;

                return legendX;
            }
        }

        public string LegendY
        {
            get
            {
                var legendX = "T(уст)";
                if (XmlNode.Attributes["rusNameY"] != null)
                    legendX = XmlNode.Attributes["rusNameY"].Value;

                return legendX;
            }
        }
    }


}
