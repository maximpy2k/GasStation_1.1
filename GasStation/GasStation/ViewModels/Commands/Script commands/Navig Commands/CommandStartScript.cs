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
using GasStation.Elements.Data;

namespace GasStation.ViewModels.Commands
{
    public class CommandStartScript:ICommand
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        public CommandStartScript(MainWindowViewModel mainWindowViewModel)
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
            if (_mainWindowViewModel.ClsScript.Steps.Count == 0)
            {
                MessageBox.Show("В скрипте нет комманд");
                return;
            }

            _mainWindowViewModel.ClassProcessingScript = new ClassProcessingScript(_mainWindowViewModel.ClsScript, new ClassSerialParams(_mainWindowViewModel.ClsScript.Consts.ConstProgramm));
            _mainWindowViewModel.ClassProcessingScript.Start();


            var log = new ClassDataMainLog($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}  ", $"Запуск техпроцесса");

            log.AppendToFile($"{_mainWindowViewModel.ClsScript.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");

            _mainWindowViewModel.ClsScript.ViewData.MessageOut = _mainWindowViewModel.ClsScript.ViewData.MessageOut.Insert(_mainWindowViewModel.ClsScript.ViewData.MessageOut.Count(), log.ToString() + $"{Environment.NewLine}");
        }

        

        public event EventHandler CanExecuteChanged;
    }

    
}
