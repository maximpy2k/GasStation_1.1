using GasStation.Elements.Data;
using GasStation.Mathem.Pid;
using GasStation.xml.Const.Elements;
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
        /// Константы для термосекции
        /// </summary>
        XmlClassBubblerConst _bubblerConst { get; set; }


        XmlClassBubbler _bubblerStep;

        /// <summary>
        /// Конструктор класса
        /// </summary>        
        public ClassBubblerPidRegulation()
        {

        }
        /// <summary>
        /// Новый шаг скрипта
        /// </summary>
        /// <param name="constPidOut">Константы для внешнего ПИД регулятора</param>
        /// <param name="constPidIn">Константы для внутреннего ПИД регулятора</param>
        public void DataStep(XmlClassBubblerConst bubblerConst, XmlClassBubbler bubblerStep)
        {
            _bubblerConst = bubblerConst;
            _bubblerStep = bubblerStep;
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

                _сlassPidRegulatorOut = new ClassPidRegulator(_bubblerConst.Pid);
                return _сlassPidRegulatorOut;
            }
            set
            {
                _сlassPidRegulatorOut = value;
            }
        }


        /// <summary>
        /// Список данных по камере
        /// </summary>
        public List<ClassDataChamber> LstDataChamber = new List<ClassDataChamber>();



        private double? SetupTemp;
        private double? TdIn;
        private double? TdOut;

        /// <summary>
        /// Добавление нового шага управления
        /// </summary>
        /// <param name="tdOut">Температура с термодатчика внешнего контура</param>
        /// <param name="setupTemp">Заданная температура</param>
        public ClassDataBubler NextStep_MaximumSpeed(ClassDataTime classDataTime, double tdOut, double setupTemp)
        {
            double setTemp = setupTemp;
            var data = new ClassDataBubler(classDataTime, _bubblerConst);
            data.DataPid = null;
            data.SetPower = data.ClassPidOut.DeltaValue;// EvalfPower(data.ClassPidOut.DeltaValue);
            return data;
        }
        public ClassDataBubler NextStep_InnerPidRegulation(ClassDataTime classDataTime, double tdOut, double setupTemp)
        {
            var data = new ClassDataBubler(classDataTime, _bubblerConst);
            var setTemp = setupTemp;

            data.ClassPidOut = ClassPidRegulatorOut.NextStep(tdOut, setTemp);
            data.SetPower = data.ClassPidOut.DeltaValue;// EvalfPower(data.ClassPidOut.DeltaValue);
            return data;
        }
        public ClassDataBubler NextStep(ClassDataTime classDataTime, double tdOut, double setupTemp)
        {
            if (SetupTemp != setupTemp)
                SetupTemp = null;

            if (SetupTemp == null)
            {
                SetupTemp = setupTemp;
                TdOut = tdOut;
            }

            if (_bubblerStep.UsePid)
            {
                return NextStep_InnerPidRegulation(classDataTime, tdOut, setupTemp);
            }
            else
                return NextStep_MaximumSpeed(classDataTime, tdOut, setupTemp);
        }
    }
}
