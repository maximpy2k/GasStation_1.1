using GasStation.Elements.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GasStation.ViewModels.Commands
{
    public class CommandStopScript : ICommand
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public CommandStopScript(MainWindowViewModel mainWindowViewModel)
        {
            _mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _mainWindowViewModel.ClassProcessingScript.Stop();

            //_mainWindowViewModel.IsViewScriptStart = true;

            //var message = $"{DateTime.Now.ToLongTimeString()} Остановка техпроцесса";


            var log = new ClassDataMainLog(DateTime.Now.ToLongTimeString(), $"Остановка техпроцесса");

            log.AppendToFile($"{_mainWindowViewModel.ClsScript.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");

            _mainWindowViewModel.ClsScript.ViewData.MessageOut = _mainWindowViewModel.ClsScript.ViewData.MessageOut.Insert(_mainWindowViewModel.ClsScript.ViewData.MessageOut.Count(), log.ToString() + $"{Environment.NewLine}");
           
            // _mainWindowViewModel.ClsScript.ViewData.MessageOut = _mainWindowViewModel.ClsScript.ViewData.MessageOut.Insert(_mainWindowViewModel.ClsScript.ViewData.MessageOut.Count(), message + $"{Environment.NewLine}");
        }
    }
}
