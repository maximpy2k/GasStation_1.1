using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.ModalWindows.View;
using GasStation.xml.Script;

namespace GasStation.ViewModels.Commands
{
    public class CommandStepParamsLoader : ICommand
    {
        public CommandStepParamsLoader()
        {
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var win = new WindowLoaderParams();

            var par = (XmlClassLoader)parameter;

            win.DataContext = par;
            win.ShowDialog();

        }
    }
}
