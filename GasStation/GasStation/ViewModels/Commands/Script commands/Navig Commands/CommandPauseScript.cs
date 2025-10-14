using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.xml.Const.Elements;
using System.IO;
using System.Windows;
using GasStation.Mathem.Chamber;

namespace GasStation.ViewModels.Commands
{
    public class CommandPauseScript : ICommand
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public CommandPauseScript(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_mainWindowViewModel.ClsScript == null)
            {
                MessageBox.Show("Скрипт не создан");
                return;
            }
            //_mainWindowViewModel.ClassProcessingScript.Pause();

            //var message = $"{DateTime.Now.ToLongTimeString()} Пауза";
            _mainWindowViewModel.ClsScript.ClsDataTime.FlagPause = true;
            //_mainWindowViewModel.ClsScript.ViewData.MessageOut = _mainWindowViewModel.ClsScript.ViewData.MessageOut.Insert(_mainWindowViewModel.ClsScript.ViewData.MessageOut.Count(), message + $"{Environment.NewLine}");
        }



        public event EventHandler CanExecuteChanged;
    }


}
