using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.ModalWindows.View;

namespace GasStation.ViewModels.Commands
{
    public class CommandPumpView:ICommand
    {
        private MainWindowViewModel _model;

        public CommandPumpView(MainWindowViewModel model)
        {
            _model = model;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var win = new WindowPumpView();
            win.DataContext = parameter;
            win.ShowDialog();
        }

        public event EventHandler CanExecuteChanged;
    }
}
