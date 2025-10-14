using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GasStation.ViewModels.Commands
{
    public abstract class ClassCommandBase : ICommand
    {
        #region Реализация интерфейса ICommand
        /// <summary>
        /// Событие изменения состояния комманды
        /// </summary>
        public event EventHandler CanExecuteChanged;
        /// <summary>
        /// Функцчия изменения состояния комманды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Состояние коммманды</returns>
        public virtual bool CanExecute(object parameter)
        {
            return true;
        }
        /// <summary>
        /// Функция комманды
        /// </summary>
        /// <param name="parameter"></param>
        public abstract void Execute(object parameter);
        #endregion

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="viewMod">Ссылка на ViewMOd</param>
        public ClassCommandBase(MainWindowViewModel viewMod)
        {
            ViewMod = viewMod;
            ViewMod.EventUpdateCanExecute += CheckCanExecute;
        }

        /// <summary>
        /// Активация события CanExecute
        /// </summary>
        public void CheckCanExecute(object o, EventArgs e)
        {
            CanExecuteChanged?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Ссылка на ViewModel
        /// </summary>
        protected MainWindowViewModel ViewMod;

        
    }
}
