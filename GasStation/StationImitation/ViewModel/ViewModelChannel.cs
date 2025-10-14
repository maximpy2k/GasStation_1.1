using GasStation.ViewModels.Elements;
using GasStation.xml;
using GasStation.xml.Constant;
using StationImitation.Commands;
using StationImitation.Controllers;
using StationImitation.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StationImitation.ViewModel
{
    public class ViewModelChannel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelChannel()
        {
            CmdOpenConst = new CommandOpenConst(this);
            CmdOpenPort = new CommandOpenPort(this);
        }


        private String port;
        public String Port
        {
            get { return port; }
            set { port = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Port")); }
        }


        private String path;
        public String Path
        {
            get { return path; }
            set { path = value;PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Path")); }
        }

        private CommandOpenConst cmdOpenConst;
        public CommandOpenConst CmdOpenConst
        {
            get { return cmdOpenConst; }
            set { cmdOpenConst = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CmdOpenConst")); }
        }

        private CommandOpenPort cmdOpenPort;
        public CommandOpenPort CmdOpenPort
        {
            get { return cmdOpenPort; }
            set { cmdOpenPort = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CmdOpenPort")); }
        }


        private ClassImitator clsImitator;
        public ClassImitator ClsImitator
        {
            get { return clsImitator; }
            set { clsImitator = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClsImitator")); }
        }

        private ClassConstView constView;
        public ClassConstView ConstView
        {
            get { return constView; }
            set { constView = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ConstView")); }
        }


    }
}
