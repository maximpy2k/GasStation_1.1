using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;

namespace StationImitation.Controllers
{
    public class Class_Shim : ClassBaseController
    {
        public Class_Shim(int addr) : base(addr)
        {
        }
        ViewModelControllerTMPower _viewController = new ViewModelControllerTMPower();
        public Class_Shim(XmlClassControllerConst controllerConst, BaseClassViewControllers viewController) : base(controllerConst, viewController)
        {
            _viewController = viewController as ViewModelControllerTMPower;
            _viewController.DioValues = new bool[16];
        }

        public int PortState;
        /// <summary>
        /// Ответ на строку запрса напряжения портов #adr
        /// </summary>
        /// <returns>Ответ на строку запрса напряжения портов #adr</returns>
        protected override string DogCmds(string quest)
        {
            var addr = Convert.ToInt32(quest.Substring(1, 2), 16);

            if (addr != this.addr)
                return "";

                PortState = Convert.ToInt32(quest.Substring(3),16);
                _viewController.SetPower = PortState;

            return "";
        }

    }
}
