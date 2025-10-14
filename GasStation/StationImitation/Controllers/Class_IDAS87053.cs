using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;

namespace StationImitation.Controllers
{
    public class Class_IDAS87053 : ClassBaseController
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="addr">Адрес контроллера</param>
        public Class_IDAS87053(int addr) : base(addr)
        {
        }

        ViewModelControllerDio _viewController = new ViewModelControllerDio();
        public Class_IDAS87053(XmlClassControllerConst controllerConst, BaseClassViewControllers viewController) : base(controllerConst, viewController)
        {
            _viewController = viewController as ViewModelControllerDio;
            _viewController.DioValues = new bool[16];
        }

        /// <summary>
        /// 
        /// </summary>
        public int PortState = 0xABCD;

        /// <summary>
        /// Ответ на строку запрса напряжения портов #adr
        /// </summary>
        /// <returns>Ответ на строку запрса напряжения портов #adr</returns>
        protected override string DogCmds(string quest)
        {            
            var addr = Convert.ToInt32(quest.Substring(1, 2), 16);
            if (addr != this.addr)
                return "";
            return $">{Result:X4}";
        }

        public int Result
        {
            get
            {
                var res = _viewController.DioValues;
                Int32 c = 0;
                for (int i = 0; i < 16; i++)
                {

                    c |= Convert.ToInt32(Convert.ToInt32(res[i]) << i);
                }

                return c;
            }
        }
    }
}
