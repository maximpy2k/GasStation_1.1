using GasStation.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.ViewModels
{
    public class ViewModelControllerAcp:BaseClassViewControllers,INotifyPropertyChanged
    {
        public ViewModelControllerAcp()
        {
                         
        }
        private double[] acpValues;

        public event PropertyChangedEventHandler PropertyChanged;

        public double[] AcpValues
        {
            get { return acpValues; }
            set
            {
                acpValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AcpValues"));
            }
        }

        private bool avalible=true;
        public bool Avalible
        {
            get { return avalible; }
            set
            {
                avalible = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Avalible"));
            }
        }

    }
}
