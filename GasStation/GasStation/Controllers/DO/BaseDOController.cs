using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Controllers.DO
{
    public abstract class BaseDOController : BaseClassController
    {
        public BaseDOController(string logpath, XmlClassControllerConst contrConst, SerialPort sp) : base(logpath, contrConst, sp)
        {
            for (int i = 0; i < MasPortsState.Length; i++)
            {
                var val = contrConst.DefaultState;
                MasPortsState[i] = Convert.ToBoolean((val&(1<<i))>>i);
            }
            
        }

        public abstract void SetValue(int val);
        

        protected virtual byte LowByte { get; set; }
        protected virtual byte HiByte { get; set; }

        /// <summary>
        /// Массив состояний портов
        /// </summary>
        public bool[] MasPortsState { get; set; } = new bool[16];


    }
}
