using System;
using System.Drawing.Imaging;
using GasStation.Elements.Data;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;

namespace GasStation.Mathem.RRG
{
    /// <summary>
    /// Класс вычисления значения расхода газа
    /// </summary>
    public class ClassRrgSetRaise
    {
        /// <summary>
        /// Параметры шага РРГ
        /// </summary>
        private XmlClassRrg _clsRrgStep;
        
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
        public ClassRrgSetRaise(ClassDataTime classDataTime, double currSetRaise = 0)
        {
            _classDataTime = classDataTime;
            CurrSetRaise = currSetRaise;
        }

        /// <summary>
        /// Запуск нового шага скрпта
        /// </summary>
        /// <param name="clsRrgStep">Параметры шага РРГ</param>
        /// <param name="stepParams">Параметры шага скрипта</param>
        public void NewScriptStep(XmlClassRrg clsRrgStep, XmlClassStepParams stepParams)
        {
            _clsRrgStep = clsRrgStep;
            _stepParams = stepParams;

            CurrTypeReg = clsRrgStep.TypeReg;

            BegTime = 0;
            EndTime = stepParams.TimeStep.TotalSeconds;

            BegRaise = CurrSetRaise;
            EndRaise = clsRrgStep.SetupValue;            
        }

        /// <summary>
        /// Получение значения расхода на циклическом шаге при скорости заданной в константах
        /// </summary>
        /// <returns>Расход РРГ</returns>
        public double NextCycleStepDefaultSpeed()
        {
            //Время прошедшее с момента изменения задания расхода
            var time = _classDataTime.TimeStep - BegTime;
            double k = EndRaise >= BegRaise ? _clsRrgStep.RrgConst.RaiseSpeed : -_clsRrgStep.RrgConst.DownSpeed;

            var currSetRaise = BegRaise + time * k;

            if (k >= 0 && currSetRaise >= EndRaise)
                currSetRaise = EndRaise;

            if (k<=0 && currSetRaise <= EndRaise)
                currSetRaise = EndRaise;
            
            return currSetRaise;
        }

        /// <summary>
        /// Получение расхода на новом циклическом шаге
        /// </summary>
        /// <returns></returns>
        public double NextCycleStep()
        {
            #region Изменение данных шага
            if (CurrTypeReg != _clsRrgStep.TypeReg)
            {
                BegTime = _classDataTime.TimeStep;
                BegRaise = CurrSetRaise;
                EndRaise = _clsRrgStep.SetupValue;
            }

            if (EndRaise != _clsRrgStep.SetupValue)
            {
                BegTime = _classDataTime.TimeStep;
                BegRaise = CurrSetRaise;
                EndRaise = _clsRrgStep.SetupValue;
            }

            if (_stepParams.TimeStep.TotalSeconds != EndTime)
            {
                BegTime = _classDataTime.TimeStep;
                EndTime = _stepParams.TimeStep.TotalSeconds;
                BegRaise = CurrSetRaise;
            } 
            #endregion

            switch (_clsRrgStep.TypeReg)
            {
                case Regims.TimeInterval:
                    CurrSetRaise = ClsLineFuncCoefs.GetRaise(_classDataTime.TimeStep);
                    break;
                case Regims.DefaultSpeed:
                    CurrSetRaise = NextCycleStepDefaultSpeed();
                    break;
                case Regims.MaximumSpeed:
                    CurrSetRaise = _clsRrgStep.SetupValue;
                    break;
            }
            CurrTypeReg = _clsRrgStep.TypeReg;
            TimeStep = _classDataTime.TimeStep;

            return CurrSetRaise;
        }
    }
}
