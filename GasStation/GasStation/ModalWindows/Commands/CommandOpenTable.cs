using GasStation.ModalWindows.View;
using GasStation.xml.Constant.XmlConst;
using GasStation.xml.Script;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace GasStation.ModalWindows.Commands
{
    public class CommandOpenTable : ICommand
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
            XmlNode xmlNode = null;
            if (scriptStep != null)
            {
                var cascadePidUse = scriptStep.UseSetThermo;

                if (cascadePidUse)
                {
                    if (scriptStep.ThermoSectionConst.Td[0].CorrectionTable != null)
                        xmlNode = scriptStep.ThermoSectionConst.Td[0].CorrectionTable.XmlNode;
                    else
                    {
                        MessageBox.Show("Отсутствует корректировочная таблица для рабочего термодатчика");
                        return;
                    }
                }
                else
                {
                    if (scriptStep.ThermoSectionConst.Td[1].CorrectionTable != null)
                        xmlNode = scriptStep.ThermoSectionConst.Td[1].CorrectionTable.XmlNode;
                    else
                    {
                        MessageBox.Show("Отсутствует корректировочная таблица для контрольного термодатчика");
                        return;
                    }
                }

            }
            else
                xmlNode = parameter as XmlNode;

                var win = new WindowCorrTable();
                var data = new XmlTableConst(xmlNode);
                win.DataContext = data;
                win.ShowDialog();
                if (scriptStep != null)
                    scriptStep.RefrashTemp();

            }
        }
    }
