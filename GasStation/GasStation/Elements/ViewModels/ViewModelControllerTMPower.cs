using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using GasStation.Annotations;

namespace GasStation.Elements.ViewModels
{
    public class ViewModelControllerTMPower : BaseClassViewControllers, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int setPower;
        public int SetPower
        {
            get { return setPower; }
            set
            {
                setPower = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SetPower"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewPower"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewValue"));
            }
        }

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


        private int viewPower;
        public int ViewPower
        {
            get { return SetPower>>1; }

            set
            {
                viewPower = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewPower"));
            }
               
        }

        private int viewValue;
        public int ViewValue
        {
            get { return SetPower; }

            set
            {
                viewValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ViewValue"));
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
