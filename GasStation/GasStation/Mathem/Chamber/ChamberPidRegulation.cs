using GasStation.Elements.Data;
using GasStation.Mathem.Pid;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using System.Collections.Generic;

namespace GasStation.Mathem.Chamber
{   
    public class ChamberPidRegulation
    {
        /// <summary>
        /// Константы для термосекции
        /// </summary>
        XmlClassThermoSectionConst _sectionConst { get; set; }

        XmlClassChamberSection _sectionStep;

        /// <summary>
        /// Конструктор класса
        /// </summary>        
        public ChamberPidRegulation()
        {
            
        }
        /// <summary>
        /// Новый шаг скрипта
        /// </summary>
        /// <param name="constPidOut">Константы для внешнего ПИД регулятора</param>
        /// <param name="constPidIn">Константы для внутреннего ПИД регулятора</param>
        public void DataStep(XmlClassThermoSectionConst sectionConst, XmlClassChamberSection sectionStep)
        {
            _sectionConst = sectionConst;
            _sectionStep = sectionStep;
            
            if (!_sectionStep.UsePid)
                ClassPidRegulatorIn = null;
        }

        protected ClassPidRegulator _сlassPidRegulatorOut;
        /// <summary>
        /// Класс внешнего ПИД регулятора
        /// </summary>
        protected ClassPidRegulator ClassPidRegulatorOut
        {
            get
            {
                if (_сlassPidRegulatorOut != null)
                    return _сlassPidRegulatorOut;

                _сlassPidRegulatorOut = new ClassPidRegulator(_sectionConst.Pid[0]);
                return _сlassPidRegulatorOut;
            }
            set
            {
                _сlassPidRegulatorOut = value;
            }
        }

        protected ClassPidRegulator _сlassPidRegulatorIn;
        /// <summary>
        /// Класс внутреннего ПИД регулятора
        /// </summary>
        /// 
        protected ClassPidRegulator ClassPidRegulatorIn
        {
            get
            {
                if (_сlassPidRegulatorIn != null)
                    return _сlassPidRegulatorIn;

                _сlassPidRegulatorIn = new ClassPidRegulator(_sectionConst.Pid[1]);
                return _сlassPidRegulatorIn;
            }
            set
            {
                _сlassPidRegulatorIn = value;
            }
        }

        /// <summary>
        /// Список данных по камере
        /// </summary>
        public List<ClassDataChamber> LstDataChamber = new List<ClassDataChamber>();

        ///// <summary>
        ///// Вычисление мощности нагревателя
        ///// </summary>
        ///// <param name="setPid">Величина % / Град</param>
        ///// <returns>Код для нагревателя</returns>
        //public int EvalfPower(double setPid)
        //{
        //    if (setPid < 0)
        //        return 0;

        //    var k = (_sectionConst.MinKey - _sectionConst.MaxKey) / (0.0 - 100.0);
        //    var b = _sectionConst.MinKey - k * 0.0;

        //    return (int)(setPid *100 ждю* k + b);
        //}

        private double? SetupTemp;
        private double? TdIn;
        private double? TdOut;
        //DateTime beginTime;

        /// <summary>
        /// Добавление нового шага управления
        /// </summary>
        /// <param name="tdOut">Температура с термодатчика внешнего контура</param>
        /// <param name="tdIn">Температура с термодатчика внутреннего контура</param>
        /// <param name="setupTemp">Заданная температура</param>
        public ClassDataChamber NextStep_MaximumSpeed(ClassDataTime classDataTime, double tdOut, double tdIn, double setupTemp)
        {
            double setTemp = setupTemp;
            var data = new ClassDataChamber(classDataTime, _sectionConst);
            data.ClassPidOut = ClassPidRegulatorOut.NextStep(tdOut, setTemp);
            data.ClassPidIn = new ClassDataPid(tdIn, setTemp, _sectionConst.Pid[1]);
            data.SetPower = data.ClassPidOut.DeltaValue;// EvalfPower(data.ClassPidOut.DeltaValue);
            //data.CurrDate = DateTime.Now;
            return data;
        }
        public ClassDataChamber NextStep_InnerPidRegulation(ClassDataTime classDataTime, double tdOut, double tdIn, double setupTemp)
        {
            var data = new ClassDataChamber(classDataTime, _sectionConst);
            var setTemp = setupTemp;
            data.ClassPidIn = ClassPidRegulatorIn.NextStep(tdIn, setupTemp);
            setTemp = tdIn + data.ClassPidIn.DeltaValue;        
            
            data.ClassPidOut = ClassPidRegulatorOut.NextStep(tdOut, setTemp);
            data.SetPower = data.ClassPidOut.DeltaValue;// EvalfPower(data.ClassPidOut.DeltaValue);
            //data.CurrDate = DateTime.Now;
            return data;
        }
        public ClassDataChamber NextStep(ClassDataTime classDataTime, double tdOut, double tdIn, double setupTemp)
        {
            if (SetupTemp != setupTemp)
                SetupTemp = null;

            if (SetupTemp == null)
            {
                SetupTemp = setupTemp;
                TdIn = tdIn;
                TdOut = tdOut;
                //beginTime = DateTime.Now;
            }      

            if (_sectionStep.UsePid)
            {
                return NextStep_InnerPidRegulation(classDataTime, tdOut, tdIn, setupTemp);
            }
            else
                return NextStep_MaximumSpeed(classDataTime, tdOut, tdIn, setupTemp);            
        }
    }
}
