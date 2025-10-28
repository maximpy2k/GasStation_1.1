using System;
using System.Drawing.Imaging;
using GasStation.Elements.Data;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;

namespace GasStation.Mathem.Bubbler
{
    /// <summary>
    /// Класс вычисления значения расхода газа
    /// </summary>
    public class ClassBubblerSetEnable
    {
        /// <summary>
        /// Параметры шага РРГ
        /// </summary>
        private XmlClassBubbler _clsBubblerStep;

        /// <summary>
        /// Параметры шага скрипта
        /// </summary>
        private XmlClassStepParams _stepParams;

        /// <summary>
        /// Временная информация
        /// </summary>
        private ClassDataTime _classDataTime;

        private ClassLineFuncCoefs _clsLineFuncCoefs;
        /// <summary>
        /// Класс расчета коэффициентов для задания расхода
        /// </summary>
        protected ClassLineFuncCoefs ClsLineFuncCoefs
        {
            get
            {
                if (_clsLineFuncCoefs != null)
                    return _clsLineFuncCoefs;
                _clsLineFuncCoefs = new ClassLineFuncCoefs(BegTime, EndTime, BegRaise, EndRaise);
                return _clsLineFuncCoefs;
            }
        }

        /// <summary>
        /// Текущий расход
        /// </summary>
        protected double CurrSetRaise;

        /// <summary>
        /// Тип рабочего интервала
        /// </summary>
        public Regims CurrTypeReg;

        /// <summary>
        /// Время шага РРГ
        /// </summary>
        public double TimeStep;

        private double _begRaise;
        /// <summary>
        /// Начальный расход
        /// </summary>
        protected double BegRaise
        {
            get
            {
                return _begRaise;
            }
            set
            {
                _begRaise = value;
                _clsLineFuncCoefs = null;
            }
        }

        private double _endRaise;
        /// <summary>
        /// Конечный расход
        /// </summary>        
        protected double EndRaise
        {
            get
            {
                return _endRaise;
            }
            set
            {
                _endRaise = value;
                _clsLineFuncCoefs = null;
            }
        }

        private double _begTime;
        /// <summary>
        /// Начальное время
        /// </summary>
        protected double BegTime
        {
            get
            {
                return _begTime;
            }
            set
            {
                _begTime = value;
                _clsLineFuncCoefs = null;
            }
        }

        private double _endTime;
        /// <summary>
        /// Конечное время
        /// </summary>
        protected double EndTime
        {
            get
            {
                return _endTime;
            }
            set
            {
                _endTime = value;
                _clsLineFuncCoefs = null;
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="clsRrgStep">Параметры шага РРГ</param>
        /// <param name="classDataTime">Временнаяинформация</param>
        public ClassBubblerSetEnable(ClassDataTime classDataTime, double currSetRaise = 0)
        {
            _classDataTime = classDataTime;
            CurrSetRaise = currSetRaise;
        }

        /// <summary>
        /// Запуск нового шага скрпта
        /// </summary>
        /// <param name="clsRrgStep">Параметры шага РРГ</param>
        /// <param name="stepParams">Параметры шага скрипта</param>
        public void NewScriptStep(XmlClassBubbler clsBubblerStep, XmlClassStepParams stepParams)
        {
            _clsBubblerStep = clsBubblerStep;
            _stepParams = stepParams;

            BegTime = 0;
            EndTime = stepParams.TimeStep.TotalSeconds;

            BegRaise = CurrSetRaise;
            EndRaise = clsBubblerStep.SetupValue;
        }

        /// <summary>
        /// Получение значения расхода на циклическом шаге при скорости заданной в константах
        /// </summary>
        /// <returns>Расход РРГ</returns>
        public double NextCycleStepDefaultSpeed()
        {
            ////Время прошедшее с момента изменения задания расхода
            //var time = _classDataTime.TimeStep - BegTime;
            //double k = EndRaise >= BegRaise ? _clsBubblerStep.BubblerConst.RaiseSpeed : -_clsBubblerStep.BubblerConst.DownSpeed;

            //var currSetRaise = BegRaise + time * k;

            //if (k >= 0 && currSetRaise >= EndRaise)
            //    currSetRaise = EndRaise;

            //if (k <= 0 && currSetRaise <= EndRaise)
            //    currSetRaise = EndRaise;

            //return currSetRaise;
            return 10;
        }

        /// <summary>
        /// Получение расхода на новом циклическом шаге
        /// </summary>
        /// <returns></returns>
        public double NextCycleStep()
        {
            //#region Изменение данных шага
            //if (CurrTypeReg != _clsBubblerStep.TypeReg)
            //{
            //    BegTime = _classDataTime.TimeStep;
            //    BegRaise = CurrSetRaise;
            //    EndRaise = _clsBubblerStep.SetupValue;
            //}

            //if (EndRaise != _clsBubblerStep.SetupValue)
            //{
            //    BegTime = _classDataTime.TimeStep;
            //    BegRaise = CurrSetRaise;
            //    EndRaise = _clsRrgStep.SetupValue;
            //}

            //if (_stepParams.TimeStep.TotalSeconds != EndTime)
            //{
            //    BegTime = _classDataTime.TimeStep;
            //    EndTime = _stepParams.TimeStep.TotalSeconds;
            //    BegRaise = CurrSetRaise;
            //}
            //#endregion


            CurrSetRaise = NextCycleStepDefaultSpeed();

            TimeStep = _classDataTime.TimeStep;

            return CurrSetRaise;
        }
    }
}
