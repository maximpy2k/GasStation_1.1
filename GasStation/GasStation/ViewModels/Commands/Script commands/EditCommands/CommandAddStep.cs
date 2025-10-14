using System.Linq;
using System.Windows;
using GasStation.xml.Script.Security;

namespace GasStation.ViewModels.Commands
{
    /// <summary>
    /// Класс комманды добавления нового шага
    /// </summary>
    public class CommandAddStep:ClassCommandBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="viewMod">Ссылка на ViewMod</param>
        public CommandAddStep(MainWindowViewModel viewMod):base(viewMod)
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
            if (ViewMod.ClsScript == null)
            {
                MessageBox.Show("Скрипт не создан");
                return;
            }
            ViewMod.ClsScript.CreateNewStep();            
        }
    }
}
