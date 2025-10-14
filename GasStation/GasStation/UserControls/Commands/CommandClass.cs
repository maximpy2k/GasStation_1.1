using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GasStation.UserControls.Commands
{
    public class CommandClass : ICommand
    {
        public CommandClass(Action<object> executeHandler, Predicate<object> canExecuteHandler=null)
        {
            CanExecuteHandler = canExecuteHandler;
            ExecuteHandler = executeHandler;
        }



        public Predicate<object> CanExecuteHandler;
        public Action<object> ExecuteHandler;
  

        public event EventHandler CanExecuteChanged;
               

        public bool CanExecute(object parameter)
        {
            if (CanExecuteHandler == null)
                return true;
            return CanExecuteHandler.Invoke(parameter);
        }

        public void Execute(object parameter)
        {
            ExecuteHandler.Invoke(parameter);
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
