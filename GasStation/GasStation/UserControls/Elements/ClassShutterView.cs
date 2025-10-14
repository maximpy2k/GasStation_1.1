using GasStation.xml.Const.Elements;
using GasStation.xml.Constant;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.ViewModels.Elements
{
    public class ClassShutterView : INotifyPropertyChanged
    {
        private XmlClassShutterConst _const;

        public ClassShutterView(XmlClassShutterConst con)
        {
            _const = con;

            FlapView = new ClassFlapView[con.Flaps.Length];

            for (int i = 0; i < con.Flaps.Length; i++)
                FlapView[i] = new ClassFlapView();
        }

        public ClassFlapView[] FlapView { get; set; }

        bool openShutterStatus;
        public bool OpenShutterStatus
        {
            get { return openShutterStatus; }
            set
            {
                openShutterStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OpenStatus"));
            }
        }

        public bool FlapLttleFlapIsEnabled => FlapView.Length == 2 ? true : false;

        bool closeShutterStatus;
        public bool CloseShutterStatus
        {
            get { return closeShutterStatus; }
            set
            {
                closeShutterStatus = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CloseShutterStatus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("OpenStatus"));
            }
        }

        public bool? OpenStatus
        {
            get
            {
                if (OpenShutterStatus)
                    return true;
                if (CloseShutterStatus)
                    return false;
                return null;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
