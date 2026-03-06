using System;
using System.IO.Ports;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.Controllers.DO;

namespace GasStation.Controllers
{
    /// <summary>
    /// Контроллер нагревателя
    /// </summary>
    public class ClassController7042 : BaseDOController
    {

        ViewModelControllerTMPower _viewModel;
        public ClassController7042(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers view) : base(logpath, contrConst, sp)
        {
            _viewModel = view as ViewModelControllerTMPower;            
        }
        
        public override void PostExecute()
        {
            if (!ControllerStatus)
                _viewModel.Avalible = ControllerStatus;
            else
                _viewModel.Avalible = ControllerStatus;

            var cmd1 = $"@{contrConst.Pa:X2}{HiByte:X2}{LowByte:X2}";
            SendCmd(cmd1);
        }

        public override void SetValue(int val)
        {
            var setVal = val & 0xFFFF;

            _viewModel.SetPower = val;

            bool[] dioValues = new bool[14];
            for (int i = 0; i < 14; i++)
            {
                dioValues[i] = Convert.ToBoolean((val & (1 << i)) >> i);
            }
            _viewModel.DioValues = dioValues;

            LowByte = Convert.ToByte(setVal & 0x000000FF);
            HiByte = Convert.ToByte((setVal & 0x0000FF00) >> 8);
        }

        protected override void PrevExecuteFunc()
        {
            if (!ControllerStatus)
                _viewModel.Avalible = ControllerStatus;
            else
                _viewModel.Avalible = ControllerStatus;
        }
    }
}
