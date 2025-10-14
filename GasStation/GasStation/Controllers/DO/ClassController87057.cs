using GasStation.Controllers.DO;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using System;
using System.IO.Ports;

namespace GasStation.Controllers
{
    /// <summary>
    /// Контроллер ДО (Клапана)
    /// </summary>
    public class ClassController87057 : BaseDOController
    {

        ViewModelControllerDio _viewController;
        public ClassController87057(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers viewController) : base(logpath, contrConst, sp)
        {
            _viewController = viewController as ViewModelControllerDio;
        }

        public override void PostExecute()
        {
            if (!ControllerStatus)
                _viewController.Avalible = ControllerStatus;
            else
                _viewController.Avalible = ControllerStatus;
            var cmd1 = $"#{contrConst.Pa:X2}0A{LowByte:X2}";
            var cmd2 = $"#{contrConst.Pa:X2}0B{HiByte:X2}";

            SendCmd(cmd1);
            SendCmd(cmd2);

            _viewController.DioValues = MasPortsState;
        }

        /// <summary>
        /// Младший байт
        /// </summary>
        protected override byte LowByte
        {
            get
            {
                byte b = 0;
                for (int i = 0; i < 8; i++)
                    b = Convert.ToByte(b | (Convert.ToInt32(MasPortsState[i]) << i));
                return b;

            }
            set { }
        }
        /// <summary>
        /// Старший байт
        /// </summary>
        protected override byte HiByte
        {
            get
            {
                byte b = 0;
                for (int i = 8; i < 16; i++)
                    b = Convert.ToByte(b | (Convert.ToInt32(MasPortsState[i]) << (i-8)));
                return b;
            }
            set { }
        }

        public override void SetValue(int val)
        {
            var setVal = val & 0xFFFF;
                       
            bool[] dioValues = new bool[14];
            for (int i = 0; i < 14; i++)
                MasPortsState[i] = Convert.ToBoolean((val & (1 << i)) >> i);

            LowByte = Convert.ToByte(setVal & 0x000000FF);
            HiByte = Convert.ToByte((setVal & 0x0000FF00) >> 8);
        }

        ///// <summary>
        ///// Массив состояний портов
        ///// </summary>
        //public bool[] MasPortsState { get; set; } = new bool[16];

        protected override void PrevExecuteFunc()
        {
            if (!ControllerStatus)
                _viewController.Avalible = ControllerStatus;
            else
                _viewController.Avalible = ControllerStatus;
        }

    }
}
