using GasStation.xml.Constant;
using Microsoft.Win32;
using StationImitation.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StationImitation.Commands
{
    public class CommandOpenPort : ICommand
    {
        private ViewModelChannel ViewModel;

        public CommandOpenPort(ViewModelChannel viewModel)
        {
            this.ViewModel = viewModel;
        }
        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ClassImitator imit = new ClassImitator(ViewModel);
            imit.Start();

            //ViewModel.ViewData.ViewControllers[0]
        }

    }
}
