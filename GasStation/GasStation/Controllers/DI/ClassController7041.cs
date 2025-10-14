using GasStation.Controllers.DI;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using System;
using System.IO.Ports;

namespace GasStation.Controllers
{
    public class ClassController7041 : BaseDIController
    {
        public ClassController7041(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers viewController) : base(logpath, contrConst, sp, viewController)
        { }
    }
}
