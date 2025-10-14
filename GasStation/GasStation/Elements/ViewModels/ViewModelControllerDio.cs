using GasStation.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.ViewModels
{
    public class ViewModelControllerDio:BaseClassViewControllers,INotifyPropertyChanged
    {
        

        public event PropertyChangedEventHandler PropertyChanged;

        private bool[] dioValues;
        public bool[] DioValues
        {
            get { return dioValues; }
            set
            {
                dioValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DioValues"));
            }
        }
        private bool avalible = true;
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
