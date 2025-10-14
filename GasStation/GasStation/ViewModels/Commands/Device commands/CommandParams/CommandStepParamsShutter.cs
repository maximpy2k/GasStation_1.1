using GasStation.ModalWindows.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace GasStation.ViewModels.Commands
{
    public class CommandStepParamsShutter :ICommand
    {
        public CommandStepParamsShutter()
        {
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
           // var win = new WindowShutterParams();
            //win.DataContext = parameter;
            //win.ShowDialog();
        }
    }
}
