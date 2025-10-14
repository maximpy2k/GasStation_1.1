using GasStation.Status;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.Security;
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
    /// Комманда ручного перехода на указанный шаг скрипта
    /// </summary>
    public class CommandGoToStep:ClassCommandBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="viewMod">Ссылка на ViewMod</param>
        public CommandGoToStep(MainWindowViewModel viewMod):base(viewMod)
        {
        }

        /// <summary>
        /// Функцчия изменения состояния комманды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Состояние коммманды</returns>
        public override bool CanExecute(object parameter)
        {
            return ViewMod.ClsScript.Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.GoStepScript);
        }
        /// <summary>
        /// Функция комманды
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if(ViewMod.ClsScript.CurrStep == null)
               return;

            var args = new ConJumpArgs(0.0)
            {
                TextError = "",
                Conditional = ViewMod.ClsScript.CurrStep.StepParams.NumStep,
                TypeConditional = TypeConditional.Manual
            };

            if (ViewMod.ClassProcessingScript == null)
            {
                MessageBox.Show("Не выбран техпроцесс");
                return;
            }
            ViewMod.ClassProcessingScript.StateError?.Invoke(this, args);           
        }
    }
}
