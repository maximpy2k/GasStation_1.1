using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GasStation.Annotations;

namespace GasStation.ViewModels.Elements
{
    public class ClassFlapView: INotifyPropertyChanged
    {


        private bool _flapState;

        /// <summary>
        /// Состояние клапана, true - открыт, false - закрыт
        /// </summary>
        public bool FlapState
        {
            get { return _flapState; }

            set { _flapState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("FlapState"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
