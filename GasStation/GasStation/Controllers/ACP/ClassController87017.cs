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
    public class ClassController87017 : BaseAcpController
    {
        public ClassController87017(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers viewController) : 
                                    base(logpath, contrConst, sp, viewController)
        { }
        

    }
}
