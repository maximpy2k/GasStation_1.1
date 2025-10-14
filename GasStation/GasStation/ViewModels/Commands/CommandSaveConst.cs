using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;
using GasStation.xml.Constant;
using GasStation.xml;

namespace GasStation.ViewModels.Commands
{
    public class CommandSaveConst:ICommand
    {
        private MainWindowViewModel _mainWindowViewModel;

        public CommandSaveConst(MainWindowViewModel main)
        {
            _mainWindowViewModel = main;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _mainWindowViewModel.ClsScript.Consts.Save();
            _mainWindowViewModel.ClsScript = new ClassScript(ClassScript.DefaultConstPath);

            var message = $"{DateTime.Now.ToLongTimeString()} Сохранение констант";

            _mainWindowViewModel.ClsScript.ViewData.MessageOut = _mainWindowViewModel.ClsScript.ViewData.MessageOut.Insert(_mainWindowViewModel.ClsScript.ViewData.MessageOut.Count(), message + $"{Environment.NewLine}");
        }
    }
}
