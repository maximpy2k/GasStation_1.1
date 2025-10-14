using System;
using System.IO.Ports;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.Controllers.DO;

namespace GasStation.Controllers
{
    public class ClassControllerShim : BaseDOController
    {
        ViewModelControllerTMPower _viewModel;
        public ClassControllerShim(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers view) : base(logpath, contrConst, sp)
        {
            _viewModel = view as ViewModelControllerTMPower;
        }

        public override void CheckWdt()
        { }
        public override void ResetWdt()
        { }
        public override void PostExecute()
        {
            if (!ControllerStatus)
                _viewModel.Avalible = ControllerStatus;
            else
                _viewModel.Avalible = ControllerStatus;

            var cmd1 = $"@{contrConst.Pa:X2}{setVal:X4}";

            SendCmd(cmd1);
        }

        private int setVal = 0;
        public override void SetValue(int val)
        {
            setVal = val;
            _viewModel.SetPower = val;
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
