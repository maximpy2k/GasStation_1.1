using GasStation.Elements.Data;
using GasStation.Mathem.Pid;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Mathem.RRG
{
    public class ClassRrgPidRegulation
    {
        /// <summary>
        /// Временной информации
        /// </summary>
        private ClassDataTime _classDataTime;
        /// <summary>
        /// Параметры шага РРГ
        /// </summary>
        private XmlClassRrg _clsRrgStep;
        /// <summary>
        /// Параметры шага скрипта
        /// </summary>
        private XmlClassStepParams _stepParams;

        /// <summary>
        /// Начальное показание вакуметра
        /// </summary>
        private double? _firstPress { get; set; }

        /// <summary>
        /// Текущее установочное значение вакуума
        /// </summary>
        public double CurrSetupValue;        
        /// <summary>
        /// Измеренное давление
        /// </summary>
        public double? CurrMeasPress { get; set; }

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
        public ClassRrgPidRegulation(ClassDataTime classDataTime)
        {
            _classDataTime = classDataTime;
        }

        /// <summary>
        /// Запуск нового шага скрпта
        /// </summary>
        /// <param name="clsRrgStep">Параметры шага РРГ</param>
        /// <param name="stepParams">Параметры шага скрипта</param>
        /// <param name="firstPress">Начальное показание вакуметра</param>
        public void NewScriptStep(XmlClassRrg clsRrgStep, XmlClassStepParams stepParams, double? firstPress)
        {
            _clsRrgStep = clsRrgStep;
            _stepParams = stepParams;

            if (_firstPress == null)
                _firstPress = firstPress;

            if (_clsPidRegulator == null)
                _clsPidRegulator = new ClassPidRegulator(_clsRrgStep.RrgConst.Pid);

            _clsPidRegulator.DataStep();

            
        }
        

        /// <summary>
        /// Вычисление мощности РРГ
        /// </summary>
        /// <param name="deltaVal">Величина воздействия</param>
        /// <returns>Код для нагревателя</returns>
        public double EvalfPower(double deltaVal)
        {
            if (deltaVal < 0)
                return 0;

            var k = (_clsRrgStep.RrgConst.MasCapConst[0].Table.MinVal - _clsRrgStep.RrgConst.MasCapConst[0].Table.MaxVal) / (0.0 - 100.0);
            var b = _clsRrgStep.RrgConst.MasCapConst[0].Table.MinVal - k * 0.0;

            return (deltaVal * 100) * k + b;
        }

        public ClassDataPid NextCycleStep(double currPress)
        {
            CurrTimeStep = _classDataTime.TimeStep;
            CurrSetupValue = _clsRrgStep.SetupValue;
            CurrMeasPress = currPress;

            var classDataPid = _clsPidRegulator.NextStep(currPress, CurrSetupValue);
            return classDataPid;
        }
    }
}
