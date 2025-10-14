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
    public class Class_IDAS87057 : ClassBaseController
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="addr">Адрес контроллера</param>
        public Class_IDAS87057(int addr) : base(addr)
        {
        }

        ViewModelControllerDio _viewController = new ViewModelControllerDio();

        public Class_IDAS87057(XmlClassControllerConst controllerConst, BaseClassViewControllers viewController) : base(controllerConst, viewController)
        {
            _viewController = viewController as ViewModelControllerDio;
        }

        /// <summary>
        /// 
        /// </summary>
        public int PortState;

        /// <summary>
        /// Ответ на строку запрса напряжения портов #adr
        /// </summary>
        /// <returns>Ответ на строку запрса напряжения портов #adr</returns>
        protected override string SharpCmds(string quest)
        {            
            var addr = Convert.ToInt32(quest.Substring(1, 2), 16);

            if (addr != this.addr)
                return "";

            if (quest.Substring(3, 2) == "0A")
            {
                PortState &= 0xFF00;
                PortState |= Convert.ToInt32(quest.Substring(5), 16);
            }
            if (quest.Substring(3, 2) == "0B")
            {
                PortState &= 0x00FF;
                PortState = PortState | (Convert.ToInt32(quest.Substring(5), 16) << 8);
            }

            bool[] dioValues = new bool[16];
            for (int i = 0; i < 16; i++)
            {
                dioValues[i] = Convert.ToBoolean((PortState & (1 << i)) >> i);
            }

            _viewController.DioValues = dioValues;
            return "";
        }
    }
}
