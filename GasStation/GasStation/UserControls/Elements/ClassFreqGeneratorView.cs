using ChartApplication.points;
using System.ComponentModel;
using System.Windows;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.xml.Constant;

namespace GasStation.ViewModels.Elements
{
    public class ClassFreqGeneratorView : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly XmlClassFreqGeneratorConst _const;

        public ClassFreqGeneratorView(XmlClassFreqGeneratorConst con)
        {
            _const = con;
        }

        /// <summary>
        /// Использовать ли привилегии
        /// </summary>
        public bool UsePriv { get; set; }
    }
}
