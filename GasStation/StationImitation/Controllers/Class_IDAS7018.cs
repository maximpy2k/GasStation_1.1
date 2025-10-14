using GasStation.Elements.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GasStation.xml.Constant.XmlConst.Elements;

namespace StationImitation.Controllers
{
    public class Class_IDAS7018 : ClassBaseController
    { 
        ViewModelControllerAcp _viewController;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="addr">Адрес контроллера</param>
        public Class_IDAS7018(int addr) :base(addr)
        {
        }
        public Class_IDAS7018(XmlClassControllerConst controllerConst, BaseClassViewControllers viewController) : base(controllerConst, viewController)
        {
            _viewController = viewController as ViewModelControllerAcp;
            _viewController.AcpValues=new double[8];
        }

        /// <summary>
        /// Показания АЦП
        /// </summary>
        public double[] Acp = new double[8]{0.00, 0.015,-0.016,0.0,0.0,0.0,0.0,0.0 };
        public double[] acp = new double[8];

        /// <summary>
        /// Ответ на строку запрса напряжения портов #adr
        /// </summary>
        /// <returns>Ответ на строку запрса напряжения портов #adr</returns>
        protected override string SharpCmds(string quest)
        {
            if (quest.Substring(1) != "18")
                return "";

            var s = ">";
            for (int ch = 0; ch < acp.Length; ch++)
            {
                acp = _viewController.AcpValues;
                
               // var uKey=rand.Next(2, 6);

                //Acp[ch] = uKey / 1000.0;
                s += $"{acp[ch]:+00.000;-00.000;+00.000}";

            }
            return s;
        }


    }
}
