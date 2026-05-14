using GasStation.Elements.Data;
using GasStation.Mathem.Pid;
using GasStation.xml.Const.Elements;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript;
using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Mathem.Hydrogen
{
    public class ClassHydrogenPidRegulation
    {
        /// <summary>
        /// Константы для термосекции
        /// </summary>
        XmlClassHydrogenBurnerConst _hydrogenConst { get; set; }


        XmlClassHydrogenBurning _hydrogenStep;

        /// <summary>
        /// Конструктор класса
        /// </summary>        
        public ClassHydrogenPidRegulation()
        {

        }
        /// <summary>
        /// Новый шаг скрипта
        /// </summary>
        /// <param name="constPidOut">Константы для внешнего ПИД регулятора</param>
        /// <param name="constPidIn">Константы для внутреннего ПИД регулятора</param>
        public void DataStep(XmlClassHydrogenBurnerConst hydrogenConst, XmlClassHydrogenBurning hydrogenStep)
        {
            _hydrogenConst = hydrogenConst;
            _hydrogenStep = hydrogenStep;
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

                _сlassPidRegulatorOut = new ClassPidRegulator(_hydrogenConst.Pid);
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
        public ClassDataHydrogenBurner NextStep_MaximumSpeed(ClassDataTime classDataTime, double tdOut, double setupTemp)
        {
            double setTemp = setupTemp;
            var data = new ClassDataHydrogenBurner(classDataTime, _hydrogenConst);
            data.DataPid = null;
            var impact = tdOut < setupTemp ? 100 : -100;
            data.ClassPidOut.DeltaValue = impact;
            return data;
        }
        public ClassDataHydrogenBurner NextStep_InnerPidRegulation(ClassDataTime classDataTime, double tdOut, double setupTemp)
        {
            var data = new ClassDataHydrogenBurner(classDataTime, _hydrogenConst);
            var setTemp = setupTemp;
            data.ClassPidOut = ClassPidRegulatorOut.NextStep(tdOut, setTemp);
            return data;
        }
        public ClassDataHydrogenBurner NextStep(ClassDataTime classDataTime, double tdOut, double setupTemp)
        {
            if (SetupTemp != setupTemp)
                SetupTemp = null;

            if (SetupTemp == null)
            {
                SetupTemp = setupTemp;
                TdOut = tdOut;
            }

            if (_hydrogenStep.UsePid&&_hydrogenConst.Pid!=null)
            {
                return NextStep_InnerPidRegulation(classDataTime, tdOut, setupTemp);
            }
            else
                return NextStep_MaximumSpeed(classDataTime, tdOut, setupTemp);
        }
    }
}
