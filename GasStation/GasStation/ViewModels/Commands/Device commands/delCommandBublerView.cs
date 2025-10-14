using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.ModalWindows.View;

namespace GasStation.ViewModels.Commands
{
    public class CommandBublerView:ICommand
    {
        private MainWindowViewModel _model;

        public CommandBublerView(MainWindowViewModel model)
        {
            _model = model;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
        }

        public event EventHandler CanExecuteChanged;
    }
}
