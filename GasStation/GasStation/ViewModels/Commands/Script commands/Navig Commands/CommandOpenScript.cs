using GasStation.xml;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.Status;
using GasStation.xml.Script.EnumConst;
using GasStation.Elements.Data;

namespace GasStation.ViewModels.Commands
{
    public class CommandOpenScript:ICommand
    {
        private MainWindowViewModel mainWindowViewModel;
        public CommandOpenScript(MainWindowViewModel mainWindowViewModel)
        {
            this.mainWindowViewModel = mainWindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            OpenFileDialog openDlg = new OpenFileDialog();
            openDlg.Filter = "*.xml|*.xml";
            if (openDlg.ShowDialog() == false)
                return;
            if (mainWindowViewModel.ClsScript != null)
                mainWindowViewModel.MessageOut = mainWindowViewModel.ClsScript.ViewData.MessageOut;

            mainWindowViewModel.ClsScript = new ClassScript(openDlg.FileName, ClassScript.DefaultConstPath,openDlg.SafeFileName);

            var message = $"Открыт файл Техпроцесса: {openDlg.FileName}";


            var log = new ClassDataMainLog(DateTime.Now.ToLongTimeString(), message);

            log.AppendToFile($"{mainWindowViewModel.ClsScript.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");

            mainWindowViewModel.ClsScript.ViewData.MessageOut = mainWindowViewModel.ClsScript.ViewData.MessageOut.Insert(mainWindowViewModel.ClsScript.ViewData.MessageOut.Count(), log.ToString() + $"{Environment.NewLine}");
            
            //var log = new ClassDataMainLog(DateTime.Now.ToLongTimeString(), message);

            //log.AppendToFile($"{mainWindowViewModel.ClsScript.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");
            //mainWindowViewModel.MessageOut += log.ToString();

            //mainWindowViewModel.ClsScript.ViewData.MessageOut = mainWindowViewModel.ClsScript.ViewData.MessageOut.Insert(mainWindowViewModel.ClsScript.ViewData.MessageOut.Count(), mainWindowViewModel.MessageOut);


        }
    }
}
