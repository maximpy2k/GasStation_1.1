using GasStation.Elements.Data;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;

namespace GasStation.Mathem.Chamber
{
    /// <summary>
    /// Класс задания температуры термосекции
    /// </summary>
    public class ClassChamberSetThermo
    {
        /// <summary>
        /// Начальная температура внешнего термодатчик
        /// </summary>
        private double _begTdOut=0.0;
        /// <summary>
        /// Начальная температура внутреннего термодатчика
        /// </summary>
        private double _begTdIn=0.0;
        double td = 25;

        /// <summary>
        /// Параметры шага скрипта
        /// </summary>
        private XmlClassChamberSection _chamberSectionStep;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="thermoSectionConst">Константы термосекции</param>
        /// <param name="chamberSectionStep">Параметры шага скрипта</param>
        /// <param name="tdOut">Начальная температура внешнего термодатчика</param>
        /// <param name="tdIn">Начальная температура внутреннего термодатчика</param>
        public ClassChamberSetThermo(XmlClassChamberSection chamberSectionStep, double tdOut, double tdIn)
        {
            _chamberSectionStep = chamberSectionStep;
            _begTdOut = tdOut;
            _begTdIn = tdIn;
        }

        private double GetMaximumSpeed(ClassDataTime classDataTime)
        {
            var setTemp = _chamberSectionStep.SetupTemp;

            var begTd = _begTdOut;
            if (_chamberSectionStep.UsePid)
                begTd = _begTdIn;

            double speed = _chamberSectionStep.ThermoSectionConst.MaxSpeedUp;
            if (setTemp < begTd)
                speed = -_chamberSectionStep.ThermoSectionConst.MaxSpeedDown;

            var currSetTemp = begTd + classDataTime.TimeStep * speed/60.0;

            if (setTemp >= begTd && currSetTemp > setTemp)
                return setTemp;

            if (setTemp <= begTd && currSetTemp < setTemp)
                return setTemp;

            return currSetTemp;
        }

        public double TimeInterval(ClassDataTime classDataTime)
        {
            var endTd = _chamberSectionStep.SetupTemp;
            var begTd = _begTdOut;

            if (_chamberSectionStep.UsePid)
                begTd = _begTdIn;

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
            switch (_chamberSectionStep.TypeReg)
            {
                case Regims.MaximumSpeed:
                    return _chamberSectionStep.SetupTemp;
                case Regims.TimeInterval:
                    return TimeInterval(classDataTime);
                case Regims.DefaultSpeed:
                    return GetMaximumSpeed(classDataTime);
            }
            return _chamberSectionStep.SetupTemp;
        }
    }
}
