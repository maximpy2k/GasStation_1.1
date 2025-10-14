using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.ModalWindows.View;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript;
using GasStation.ModalWindows.View.WindowsParams;

namespace GasStation.ViewModels.Commands
{
    public class CommandStepParamsFreqGenerator : ICommand
    {
        public CommandStepParamsFreqGenerator()
        {
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            var win = new WindowFreqGeneratorParams();

            var dat = (XmlClassFreqGenerator)parameter;

            win.DataContext = dat;
            win.ShowDialog();

        }
    }
}
