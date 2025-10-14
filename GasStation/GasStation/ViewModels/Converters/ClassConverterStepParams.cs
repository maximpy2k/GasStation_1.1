using GasStation.xml.Script.EnumConst;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GasStation.ViewModels.Converters
{
    public class ClassConverterStepParams : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var regStep = (RegimsStep)value;
            switch (regStep)
            {
                case RegimsStep.Normal:
                    return "Нормальный";
                case RegimsStep.Emergency:
                    return "Аварийный";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;
            switch (str)
            {
                case "Нормальный":
                    return RegimsStep.Normal;
                case "Аварийный":
                    return RegimsStep.Emergency;
                default:
                    return "";
            }            
        }

        public string[] Items
        {
            get
            {
                return new[] { "Нормальный", "Аварийный" };
            }
        }
    }
}
