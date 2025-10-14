using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;

namespace StationImitation.Controllers
{
    public class Class_IDAS87024 : ClassBaseController
    {
        private readonly XmlClassControllerConst _controllerConst;
        ViewModelControllerCap _viewController;
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="addr">Адрес контроллера</param>
        public Class_IDAS87024(int addr):base(addr)
        {
        }
        public Class_IDAS87024(XmlClassControllerConst controllerConst, BaseClassViewControllers viewController) : base(controllerConst, viewController)
        {
            _controllerConst = controllerConst;
            _viewController = viewController as ViewModelControllerCap;
            _viewController.CapValues = new double[4];
        }

        private double[] cap = new double[4];
        protected override string SharpCmds(string quest)
        {
            var addr = Convert.ToInt32(quest.Substring(1, 2), 16);
            var port = Convert.ToInt32(quest.Substring(3, 1));

            if (addr != this.addr)
                return "";
            cap[port] = Convert.ToDouble(quest.Substring(4, 7));

            _viewController.CapValues = cap;
            return "";
        }
    }
}
