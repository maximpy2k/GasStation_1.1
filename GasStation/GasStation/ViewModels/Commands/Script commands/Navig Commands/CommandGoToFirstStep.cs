using GasStation.Status;
using GasStation.xml.Script.EnumConst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml.Linq;

namespace GasStation.ViewModels.Commands
{
    /// <summary>
    /// Переход на шаг ожидания
    /// </summary>
    public class CommandGoToFirstStep : ClassCommandBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="viewMod">Ссылка на ViewMod</param>
        public CommandGoToFirstStep(MainWindowViewModel viewMod):base(viewMod)
        {
        }

        /// <summary>
        /// Функцчия изменения состояния комманды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Состояние коммманды</returns>
        public override void Execute(object parameter)
        {
            var result = MessageBox.Show("Остановить технологический процесс?", null, MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No)
                return;

            var args = new ConJumpArgs(0.0)
            {
                TextError = "",
                Conditional = 1,
                TypeConditional = TypeConditional.Manual
            };

            if (ViewMod.ClassProcessingScript == null)
                MessageBox.Show("не выбран техпроцесс");
            ViewMod.ClassProcessingScript.StateError?.Invoke(this, args);
        }
    }
}
