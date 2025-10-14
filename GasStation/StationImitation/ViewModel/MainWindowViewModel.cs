 using GasStation.Elements.ViewModels;
using StationImitation.Commands;
using StationImitation.xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
 using System.IO;
 using System.Linq;
 using System.Runtime.Serialization.Formatters.Binary;
 using System.Text;
using System.Threading.Tasks;

namespace StationImitation.ViewModel
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Реализация интерфейса INotifyPropertyChanged
        /// <summary>
        /// Изменение Property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        public MainWindowViewModel()
        {

        }


        public ViewModelControllerAcp viewControllerAcp { get; set; }



        public ViewModelChannel[] ViewModelChannel { get; set; }= new ViewModelChannel[3] { new ViewModelChannel(), new ViewModelChannel(), new ViewModelChannel() };

    }

}

