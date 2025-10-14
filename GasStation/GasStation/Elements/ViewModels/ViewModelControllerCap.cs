using System.ComponentModel;

namespace GasStation.Elements.ViewModels
{
    public class ViewModelControllerCap:BaseClassViewControllers,INotifyPropertyChanged
    {
 
        public event PropertyChangedEventHandler PropertyChanged;

        private double[] _capValues;
        public double[] CapValues
        {
            get { return _capValues; }
            set
            {
                _capValues = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CapValues"));
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
