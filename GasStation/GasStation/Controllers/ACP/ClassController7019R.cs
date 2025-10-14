using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;

namespace GasStation.Controllers.ACP
{
    public class ClassController7019R:BaseAcpController
    {
        ViewModelControllerAcp _viewController;
        public ClassController7019R(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers viewController) : base(logpath, contrConst, sp, viewController)
        {
            _viewController = viewController as ViewModelControllerAcp;
        }
        public override void CheckWdt()
        { }
        public override void ResetWdt()
        { }
        protected override void PrevExecuteFunc()
        {


            // Считать все каналы
            var cmd = $"#{contrConst.Pa:X2}";
            var answer = SendCmdWithAnswer(cmd);
            if (!ControllerStatus)
                _viewController.Avalible = ControllerStatus;
            else
                _viewController.Avalible = ControllerStatus;
            if (answer == "")
                return;
            if (answer == null)
                return;

            var acpValues = new double[8];

            #region Разбор answer
            if (answer.Length != 57)
            {
                ErrAnswerString++;
                return;
            }

            if (answer.Substring(0, 1) != ">")
            {
                ErrAnswerString++;
                return;
            }


            for (int idx = 1, ch = 0; idx < answer.Length; idx += 7, ch++)
            {
                var strCh = answer.Substring(idx, 7);
                if (!double.TryParse(strCh, out acpValues[ch]))
                {
                    ErrAnswerString++;
                    return;
                }
            }
            #endregion

            AcpValues = acpValues;
            _viewController.AcpValues = AcpValues;


        }

    }
}
