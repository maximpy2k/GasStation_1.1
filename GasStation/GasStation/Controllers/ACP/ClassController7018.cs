using System;
using GasStation.Controllers.ACP;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using System.IO.Ports;
using System.Linq;

namespace GasStation.Controllers
{
    /// <summary>
    /// Контроллер АЦП
    /// </summary>
    public class ClassController7018 : BaseAcpController
    {
        private ViewModelControllerAcp _viewController;
        public ClassController7018(string logpath, XmlClassControllerConst contrConst, SerialPort sp,
            BaseClassViewControllers viewController) :
            base(logpath, contrConst, sp, viewController)
        {
            _viewController = viewController as ViewModelControllerAcp;
        }
        public override void PostExecute()
        {
            if (!ControllerStatus)
            {
                _viewController.Avalible = ControllerStatus;
                AcpValues = new Double[] {10000, 10000 , 10000, 10000 , 10000 , 10000, 10000 , 10000, 10000};
            }
            else
                _viewController.Avalible = ControllerStatus;
        }
    }
}