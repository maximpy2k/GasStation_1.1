using GasStation.ModalWindows.View;
using GasStation.xml.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace GasStation.ViewModels.Commands
{
    public class CommandStepParamsFlap :ICommand
    {

        public CommandStepParamsFlap ()
        {
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var win = new WindowFlapParams();
            
            var flap = (XmlClassFlap)parameter;

            win.DataContext = flap;
            win.ShowDialog();
        }

        public event EventHandler CanExecuteChanged;
    }
}
