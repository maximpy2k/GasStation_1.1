using GasStation.xml.Script;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace GasStation.ViewModels.Converters
{
    public class CompareConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                // Проверяем, что у нас есть все необходимые значения
                if (values.Length < 4 || values[0] == null || values[1] == null)
                    return Brushes.Transparent;

                // Получаем значения
                var currentItem = values[0];
                var itemsSource = values[1] as IEnumerable;
                var autoSelectedIndex = values[2] as int? ?? -1;
                var isSelected = values[3] as bool? ?? false;

                // Определяем, что нужно вернуть (фон или цвет текста)
                string returnType = parameter as string ?? "Background";

                // Если запрошен цвет текста
                if (returnType == "Foreground")
                {
                    return isSelected ? Brushes.White : Brushes.Black;
                }

                // Если элемент выбран пользователем
                if (isSelected)
                {
                    return Brushes.DodgerBlue;
                }

                // Проверяем авто-выделение по индексу
                if (autoSelectedIndex >= 0 && itemsSource != null)
                {
                    // Пытаемся получить элемент по индексу
                    int currentIndex = -1;
                    int index = 0;

                    foreach (var item in itemsSource)
                    {
                        if (item == currentItem)
                        {
                            currentIndex = index;
                            break;
                        }
                        index++;
                    }

                    // Если текущий индекс совпадает с авто-выделенным
                    if (currentIndex == autoSelectedIndex)
                    {
                        return Brushes.Green;
                    }
                }

                return Brushes.Transparent;
            }
            catch
            {
                return Brushes.Transparent;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}