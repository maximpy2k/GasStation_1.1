using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using System.IO.Ports;

namespace GasStation.Controllers
{
    /// <summary>
    /// Контроллер ЦАП
    /// </summary>
    public class ClassController87024 : BaseClassController
    {

        ViewModelControllerCap _viewController;
        public ClassController87024(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers viewController) : base(logpath, contrConst, sp)
        {
            _viewController = viewController as ViewModelControllerCap;
        }

        public void SetCap(double u, int ch)
        {
            if (!ControllerStatus)
                _viewController.Avalible = ControllerStatus;
            else
                _viewController.Avalible = ControllerStatus;

            MasCap[ch] = u;
            var cmd = $"#{contrConst.Pa:X2}{ch}+{MasCap[ch]:00.000}";
            SendCmd(cmd);

            _viewController.CapValues = MasCap;
        }
        public override void PostExecute()
        {

        }

        /// <summary>
        /// Значения цап для установки
        /// </summary>
        public double[] MasCap = new double[4];
    }
}
