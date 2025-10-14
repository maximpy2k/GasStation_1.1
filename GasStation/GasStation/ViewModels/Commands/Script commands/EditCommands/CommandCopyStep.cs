using GasStation.xml.Script.Security;
using System.Linq;

namespace GasStation.ViewModels.Commands
{
    /// <summary>
    /// Класс копирования шага
    /// </summary>
    public class CommandCopyStep:ClassCommandBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="viewMod">Ссылка на ViewMod</param>
        public CommandCopyStep(MainWindowViewModel viewMod):base(viewMod)
        {
        }

        /// <summary>
        /// Функцчия изменения состояния комманды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Состояние коммманды</returns>
        public override bool CanExecute(object parameter)
        {
            return ViewMod.ClsScript.Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeScript);
        }
        /// <summary>
        /// Функция комманды
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            ViewMod.ClsScript.CopyStep();
        }
    }
}
