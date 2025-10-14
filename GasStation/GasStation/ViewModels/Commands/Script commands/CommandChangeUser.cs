using GasStation.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;
using GasStation.xml.Script.XmlScript;
using GasStation.ModalWindows.View;
using System.Windows;

namespace GasStation.ViewModels.Commands
{
    public class CommandChangeUser : ICommand
    {
        private MainWindowViewModel mainWindowViewModel;
        public CommandChangeUser(MainWindowViewModel mainwindowViewModel)
        {
            this.mainWindowViewModel = mainwindowViewModel;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {            
            return true;
        }

       
        public void Execute(object parameter)
        {
            if (mainWindowViewModel.ClsScript != null)
                mainWindowViewModel.MessageOut = mainWindowViewModel.ClsScript.ViewData.MessageOut;

            var usrName = (string)parameter;
            var user = mainWindowViewModel.ClsScript.Consts.SecuretyConst.Users.Where(dat => dat.UserName == usrName).First();

            if (user.Password == "")
            {
                mainWindowViewModel.ClsScript.Consts.SecuretyConst.CurrUserName = user.UserName;
                mainWindowViewModel.UpdateCanExecuteCommands();

                mainWindowViewModel.UpdateClsScript();
                return;
            }

            WindowPassword winPas = new WindowPassword();
            if (!(bool)winPas.ShowDialog())
                return;

            if (winPas.EnteredPass != user.Password)
            {
                MessageBox.Show("Введен неправильный пароль");
                return;
            }


            mainWindowViewModel.ClsScript.Consts.SecuretyConst.CurrUserName = user.UserName;
            mainWindowViewModel.UpdateCanExecuteCommands();
            mainWindowViewModel.UpdateClsScript();

            
        }
    }
}