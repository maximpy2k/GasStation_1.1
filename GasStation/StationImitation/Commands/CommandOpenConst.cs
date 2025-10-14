using GasStation.xml;
using GasStation.xml.Constant;
using Microsoft.Win32;
using StationImitation.ViewModel;
using StationImitation.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StationImitation.Commands
{
    public class CommandOpenConst : ICommand
    {
        private ViewModelChannel ViewModel;

        public CommandOpenConst(ViewModelChannel ViewModel)
        {
            this.ViewModel = ViewModel;
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
            ViewModel.ConstView = new ClassConstView(openDlg.FileName);
            
            ViewModel.Path = openDlg.FileName;
            ViewModel.ConstView.ClassXmlConst = new XmlClassConst(ViewModel.Path);
            ViewModel.Port = ViewModel.ConstView.ClassXmlConst.ConstProgramm.Port.ToString();

        }
    }
}
