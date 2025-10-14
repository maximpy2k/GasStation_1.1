using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace GasStation.ViewModels.Converters
{
    public class ClassConverterListDevices:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch ((string)value)
            {
                case "rrg":
                    return "РРГ";
                case "flap":
                    return "Клапан";
                case "chamber":
                    return "Термокамера";
                case "burner":
                    return "Горелка";
                case "TimeStep":
                    return "За время шага";
                default:
                    return "";
            }

            
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
