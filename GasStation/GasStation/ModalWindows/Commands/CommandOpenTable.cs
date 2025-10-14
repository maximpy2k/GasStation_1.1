using GasStation.ModalWindows.View;
using GasStation.xml.Constant.XmlConst;
using GasStation.xml.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml;

namespace GasStation.ModalWindows.Commands
{
    public class CommandOpenTable:ICommand
    {
        public CommandOpenTable()
        {

        }

        public event EventHandler CanExecuteChanged;

        public bool IsEnabled { get; set; } = true;
        public bool CanExecute(object parameter)
        {
            return IsEnabled;
        }

        public void Execute(object parameter)
        {
            var scriptStep = parameter as XmlClassChamberSection;
            

            XmlNode xmlNode = scriptStep != null ? scriptStep.ThermoSectionConst.Tables[scriptStep.TabNum].XmlNode : parameter as XmlNode;
            
            var win = new WindowCorrTable();
            var data= new XmlTableConst(xmlNode);
            win.DataContext = data;
            win.ShowDialog();
            if (scriptStep != null)
                scriptStep.RefrashTemp();
            
        }
    }
}
