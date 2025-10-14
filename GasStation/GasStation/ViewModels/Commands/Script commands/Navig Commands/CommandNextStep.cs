using GasStation.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.xml.Script.EnumConst;

namespace GasStation.ViewModels.Commands
{
    public class CommandNextStep : ClassCommandBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="viewMod">Ссылка на ViewMod</param>
        public CommandNextStep(MainWindowViewModel viewMod):base(viewMod)
        {
        }

        /// <summary>
        /// Функция комманды
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            int stepNum = 0;
            bool noneTimeInterval = true;
            int cond = 0;

            for (; stepNum < ViewMod.ClsScript.Steps.Count; stepNum++)
            {
                if (ViewMod.ClsScript.Steps[stepNum].StepParams.TimeStep.TotalSeconds != 0)
                {
                    noneTimeInterval = false;
                    cond = stepNum + 1;
                    break;
                }
            }
            if (noneTimeInterval)
                cond = 1;

            var args = new ConJumpArgs(0.0)
            {
                TextError = "",
                Conditional = cond,
                TypeConditional = TypeConditional.Manual
            };
            ViewMod.ClassProcessingScript.StateError?.Invoke(this, args);
        }
    }
}
