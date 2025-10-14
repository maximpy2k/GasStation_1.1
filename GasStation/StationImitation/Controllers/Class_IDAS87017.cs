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
    public class Class_IDAS87017 : ClassBaseController
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="addr">Адрес контроллера</param>
        public Class_IDAS87017(int addr):base(addr)
        {
        }

        ViewModelControllerAcp _viewController = new ViewModelControllerAcp();
        public Class_IDAS87017(XmlClassControllerConst controllerConst, BaseClassViewControllers viewController) : base(controllerConst, viewController)
        {
            _viewController = viewController as ViewModelControllerAcp;
            _viewController.AcpValues = new double[8];
        }

        /// <summary>
        /// Показания АЦП
        /// </summary>
        public double[] Acp = new double[8]{0.00, 0.015,-0.016,0.0,0.0,0.0,0.0,0.0 };

        public double[] acp=new double[8];
        /// <summary>
        /// Ответ на строку запрса напряжения портов #adr
        /// </summary>
        /// <returns>Ответ на строку запрса напряжения портов #adr</returns>
        protected override string SharpCmds(string quest)
        {
            //if (quest.Substring(1) != "11")
            //    return "";

            acp = _viewController.AcpValues;

            var s = ">";
            for (int ch = 0; ch < acp.Length; ch++)
            {

                s += $"{acp[ch]:+00.000;-00.000;+00.000}";
            }
            return s;
        }


    }
}
