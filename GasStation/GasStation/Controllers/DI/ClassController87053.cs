using GasStation.Controllers.DI;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using System;
using System.IO.Ports;

namespace GasStation.Controllers
{
    /// <summary>
    /// Контроллер ДИ (Статусы)
    /// </summary>
    public class ClassController87053 : BaseDIController
    {
        public ClassController87053(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers viewController) : base(logpath, contrConst, sp, viewController)
        { }

    }
}
