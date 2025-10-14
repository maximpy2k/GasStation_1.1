using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Controllers.DI
{
    public class BaseDIController: BaseClassController
    {
        ViewModelControllerDio _viewController;
        public BaseDIController(string logpath, XmlClassControllerConst contrConst, SerialPort sp, BaseClassViewControllers viewController) : base(logpath, contrConst, sp)
        {
            _viewController = viewController as ViewModelControllerDio;
        }

        public override void CheckWdt()
        { }
        public override void ResetWdt()
        { }

        /// <summary>
        /// Значение из ЦП
        /// </summary>
        public int Result;
        protected override void PrevExecuteFunc()
        {
            if (!ControllerStatus)
                _viewController.Avalible = ControllerStatus;
            else
                _viewController.Avalible = ControllerStatus;

            // Считать все каналы
            var cmd = $"@{contrConst.Pa:X2}";
            var answer = SendCmdWithAnswer(cmd);

            if (string.IsNullOrEmpty(answer))
                return;

            if (answer.Length != 5)
            {
                ErrAnswerString++;
                return;
            }

            if (answer.Substring(0, 1) != ">")
            {
                ErrAnswerString++;
                return;
            }

            try
            {
                var res = Convert.ToInt32(answer.Substring(1, 4), 16);
                Result = res;
                _viewController.DioValues = DioValues;
            }
            catch
            {
                ErrAnswerString++;
                return;
            }
        }

        /// <summary>
        /// Результат оцифровки
        /// </summary>
        public bool[] DioValues
        {
            get
            {
                var res = new bool[16];
                for (int i = 0; i < res.Length; i++)
                    res[i] = Convert.ToBoolean((Result & (1 << i)) >> i);
                return res;
            }
        }
    }
}
