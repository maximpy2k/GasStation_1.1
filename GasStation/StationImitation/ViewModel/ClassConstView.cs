using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.ViewModels.Elements;
using GasStation.xml.Constant;

namespace StationImitation.ViewModel
{
    public class ClassConstView: INotifyPropertyChanged
    {
        string _path;
        public event PropertyChangedEventHandler PropertyChanged;

        public ClassConstView(String path)
        {
            _path = path;
            classXmlConst=new XmlClassConst(path);
        }

        private XmlClassConst classXmlConst;
        public XmlClassConst ClassXmlConst
        {
            get { return classXmlConst; }
            set
            {
                classXmlConst = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClassXmlConst"));
            }
        }

        /// <summary>
        /// Класс отображения данных
        /// </summary>
        private ClassViewDataClass _viewData { get; set; }
        public ClassViewDataClass ViewData
        {
            get
            {
                if (_viewData != null)
                    return _viewData;
                _viewData = new ClassViewDataClass(classXmlConst);
                return _viewData;
            }
            set
            {
                _viewData = value;

            }
        }
    }
}
