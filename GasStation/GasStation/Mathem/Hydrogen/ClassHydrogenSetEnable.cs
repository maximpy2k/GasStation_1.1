using System;
using System.Drawing.Imaging;
using GasStation.Elements.Data;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.XmlScript;

namespace GasStation.Mathem.Hydrogen
{
    /// <summary>
    /// Класс вычисления значения расхода газа
    /// </summary>
    public class ClassHydrogenSetEnable
    {

        /// <summary>
        /// Начальная температура внешнего термодатчик
        /// </summary>
        private double _begTdOut = 0.0;

        /// <summary>
        /// Параметры шага скрипта
        /// </summary>
        private XmlClassHydrogenBurning _hydrogenStep;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="thermoSectionConst">Константы термосекции</param>
        /// <param name="chamberSectionStep">Параметры шага скрипта</param>
        /// <param name="tdOut">Начальная температура внешнего термодатчика</param>
        /// <param name="tdIn">Начальная температура внутреннего термодатчика</param>
        public ClassHydrogenSetEnable(XmlClassHydrogenBurning hydrogenStep, double tdOut)
        {
            _hydrogenStep = hydrogenStep;
            _begTdOut = tdOut;
        }

        private double GetMaximumSpeed(ClassDataTime classDataTime)
        {
            var setTemp = _hydrogenStep.SetupValue;

            var begTd = _begTdOut;

            double speed = _hydrogenStep.HydrogenBurnerConst.UpSpeed;
            if (setTemp < begTd)
                speed = -_hydrogenStep.HydrogenBurnerConst.DownSpeed;

            var currSetTemp = begTd + classDataTime.TimeStep * speed / 60.0;

            if (setTemp >= begTd && currSetTemp > setTemp)
                return setTemp;

            if (setTemp <= begTd && currSetTemp < setTemp)
                return setTemp;

            return currSetTemp;
        }

        public double TimeInterval(ClassDataTime classDataTime)
        {
            var endTd = _hydrogenStep.SetupValue;
            var begTd = _begTdOut;


            var endTime = classDataTime.StepLengh;
            var begTime = 0;

            if (endTime - begTime <= 0)
                return GetMaximumSpeed(classDataTime);

            var k = (begTd - endTd) / (begTime - endTime);
            var b = begTd - k * begTime;

            return k * classDataTime.TimeStep + b;
        }

        public double NextStep(ClassDataTime classDataTime)
        {
            switch (_hydrogenStep.TypeReg)
            {
                case Regims.DefaultSpeed:
                    return GetMaximumSpeed(classDataTime);
                case Regims.MaximumSpeed:
                    return _hydrogenStep.SetupValue;
                case Regims.TimeInterval:
                    return TimeInterval(classDataTime);
            }
            return _hydrogenStep.SetupValue;

        }

    }

}

