using GasStation.Elements.Data;
using GasStation.Mathem.Pid;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Mathem.Bubbler
{
    public class ClassBubblerPidRegulation
    {
        /// <summary>
        /// Временной информации
        /// </summary>
        private ClassDataTime _classDataTime;
        /// <summary>
        /// Параметры шага РРГ
        /// </summary>
        private XmlClassBubbler _clsBubblerStep;
        /// <summary>
        /// Параметры шага скрипта
        /// </summary>
        private XmlClassStepParams _stepParams;

        /// <summary>
        /// Начальное показание температуры
        /// </summary>
        private double? _firstTemp { get; set; }


        /// <summary>
        /// Текущее установочное значение температуры
        /// </summary>
        public double CurrSetupValue;        
        
        /// <summary>
        /// Измеренная температура
        /// </summary>
        public double? CurrMeasTemp { get; set; }

        /// <summary>
        /// Время шага
        /// </summary>
        public double CurrTimeStep;

        /// <summary>
        /// Класс ПИД регулятора
        /// </summary>
        private ClassPidRegulator _clsPidRegulator { get; set; }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="classDataTime">Временная иныформация</param>        
        public ClassBubblerPidRegulation(ClassDataTime classDataTime)
        {
            _classDataTime = classDataTime;
        }

        /// <summary>
        /// Запуск нового шага скрпта
        /// </summary>
        /// <param name="clsBubblerStep">Параметры шага РРГ</param>
        /// <param name="stepParams">Параметры шага скрипта</param>
        /// <param name="firstPress">Начальное показание вакуметра</param>
        public void NewScriptStep(XmlClassBubbler clsBubblerStep, XmlClassStepParams stepParams, double? firstTemp)
        {
            _clsBubblerStep = clsBubblerStep;
            _stepParams = stepParams;

            if (_firstTemp == null)
                _firstTemp = firstTemp;

            if (_clsPidRegulator == null)
                _clsPidRegulator = new ClassPidRegulator(_clsBubblerStep.BubblerConst.Pid);

            _clsPidRegulator.DataStep();

            
        }
        

        /// <summary>
        /// Вычисление мощности РРГ
        /// </summary>
        /// <param name="deltaVal">Величина воздействия</param>
        /// <returns>Код для нагревателя</returns>
        public double EvalfRelay(double deltaVal)
        {
            if (deltaVal < 0)
                return 0;

            //var k = (_clsBubblerStep.Const.MasCapConst[0 ].Table.MinVal - _clsRrgStep.RrgConst.MasCapConst[0].Table.MaxVal) / (0.0 - 100.0);
            //var b = _clsRrgStep.RrgConst.MasCapConst[0].Table.MinVal - k * 0.0;

            return 100.0;// (deltaVal * 100) * k + b;
        }

        public ClassDataPid NextCycleStep(double currTemp)
        {
            CurrTimeStep = _classDataTime.TimeStep;
            CurrSetupValue = _clsBubblerStep.SetupValue;
            CurrMeasTemp = currTemp;

            var classDataPid = _clsPidRegulator.NextStep(currTemp, CurrSetupValue);
            return classDataPid;
        }
    }
}
