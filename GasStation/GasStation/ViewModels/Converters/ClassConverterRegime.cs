using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Xml;
using GasStation.xml.Script.EnumConst;

namespace GasStation.ViewModels.Converters
{
    public class ClassConverterRegime : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Regims regime = (Regims)value;
            switch (regime)
            {
                case Regims.MaximumSpeed:
                    return "Максимально быстро";
                case Regims.TimeInterval:
                    return "За время интервала";
                case Regims.DefaultSpeed:
                    return "Скорость заданная в константах";
                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {

            switch ((string)value)
            {
                case "Максимально быстро":
                    return Regims.MaximumSpeed;
                case "За время интервала":
                    return Regims.TimeInterval;
                case "Скорость заданная в константах":
                    return Regims.DefaultSpeed;
                default:
                    return "";
            }
        }

        public string[] Items
        {
            get
            {
                return new string[] { "Максимально быстро", "За время интервала", "Скорость заданная в константах" };                
            }
        }
    }
}
