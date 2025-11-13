using System;
using System.Collections.Generic;
using System.Linq;
using GasStation.Elements.Data;
using GasStation.Mathem.RRG;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.Controllers;
using GasStation.Status;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using ChartApplication.points;
using GasStation.Mathem.Chamber;
using GasStation.Mathem.Bubbler;
using System.IO;

namespace GasStation.Devices
{
    public class ClassControlBubbler : ClassBaseDevices
    {
        /// <summary>
        /// Класс параметров  шага устройства
        /// </summary>
        XmlClassBubbler _clsBubblerStep => (XmlClassBubbler)_clsDevStep;

        public ClassControlBubbler(ClassDataTime classDataTime) : base(classDataTime)
        {

        }

        /// <summary>
        /// Экземпляр класса вычисления текущего расчета
        /// </summary>
        private ClassBubblerSetEnable _clsBubblerSetEnable;

        private ClassBubblerPidRegulation _bubblerPidRegulation;
        /// <summary>
        /// Класс пид регулятора
        /// </summary>
        private ClassBubblerPidRegulation ClsBubblerPidRegulation
        {
            get
            {
                if (_bubblerPidRegulation != null)
                    return _bubblerPidRegulation;
                _bubblerPidRegulation = new ClassBubblerPidRegulation();
                return _bubblerPidRegulation;
            }
        }

        private double LastSetTempOut = -25;


        private ClassBaseTd _currTd;
        /// <summary>
        /// Данные по термодатчику
        /// </summary>
        protected ClassBaseTd CurrTd
        {
            get
            {
                if (_currTd != null)
                    return _currTd;
                _currTd = new ClassBaseTd(_clsBubblerStep.BubblerConst.Td);
                return _currTd;
            }
        }

        private bool _usePid;
        /// <summary>
        /// Флаг использования Pid регулятора
        /// </summary>
        protected bool UsePid
        {
            get
            {
                return _usePid;
            }
            set
            {
                _bubblerPidRegulation = null;
                _usePid = value;
            }
        }
        public void DataStep(XmlClassBubbler clsBublerStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            Init();
            _clsDevStep = clsBublerStep;
            _states = states.ToArray();
            _stepParams = stepParams;


            ClsBubblerPidRegulation.DataStep(_clsBubblerStep.BubblerConst, _clsBubblerStep);

            ReadTd();

            LastSetTempOut = LastSetTempOut == -25 ? CurrTd.AverTd : LastSetTempOut;
            _clsBubblerSetEnable = new ClassBubblerSetEnable(_clsBubblerStep, LastSetTempOut);

            if (File.Exists($"{path}\\Bubbler{_clsBubblerStep.Num}.txt"))
                File.Delete($"{path}\\Bubbler{_clsBubblerStep.Num}.txt");
        }

        /// <summary>
        /// Расчеты на первом шаге
        /// </summary>
        public override void Init()
        {
            FlagStop = false;
            base.Init();
        }

        /// <summary>
        /// Расчеты на каждом последующем шаге
        /// </summary>
        protected override void NextStepFunc()
        {

            var last = (ClassDataBubler)LastData;
            ReadTd();

            #region Расчет заданой температуры
            double setTemp = _clsBubblerSetEnable.NextStep(_classDataTime);
            #endregion
            LastSetTempOut = setTemp;

            ClassDataBubler curr = _bubblerPidRegulation.NextStep(_classDataTime, CurrTd.AverTd, setTemp);

            curr.SetupTemp = _clsBubblerStep.SetupValue;
            curr.UseBubbler = _clsBubblerStep.UseBubbler;


            var relay = false;
            if (curr.UseBubbler)
            {

                if (_clsBubblerStep.UsePid)
                    relay = SerRelayValuePid(curr.ClassPidOut);
                else relay = SerRelayValue();
            }
            curr.Relay = relay;
            SetPortState(_clsBubblerStep.BubblerView.Relay);
            var dateNow = DateTime.Now;

            if (last != null)
            {
                if (curr.UseBubbler != last.UseBubbler)
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _clsBubblerStep.Num;
                    conJumpArgs.NameDev = "Использование барботера";
                    conJumpArgs.CurrValue = Convert.ToInt32(curr.UseBubbler);
                    conJumpArgs.TextError = "Состояние использования барботера изменено";
                    conJumpArgs.Conditional = 1;
                    conJumpArgs.TypeConditional = TypeConditional.Text;
                    AlarmError(this, conJumpArgs);
                }
            }

            if (AddData(curr))
            {
                _clsBubblerStep.BubblerView.SeriesReadTemp.PointsPrepare.Clr(Cnt);
                _clsBubblerStep.BubblerView.SeriesSetTemp.PointsPrepare.Clr(Cnt);
            }


            #region Вывод на форму  
            _clsBubblerStep.BubblerView.DataBubbler = curr;
            _clsBubblerStep.BubblerView.SeriesReadTemp.Add(new PointTime(curr.CurrDate, CurrTd.AverTd));
            _clsBubblerStep.BubblerView.SeriesSetTemp.Add(new PointTime(curr.CurrDate, LastSetTempOut));

            _clsBubblerStep.BubblerView.CurrentTd = CurrTd.AverTd;
            _clsBubblerStep.BubblerView.Relay = curr.Relay;
            #endregion
        }

        private void PidFunc()
        {

        }

        private void ReadTd()
        {
            #region Расчет температуры по внешнему термодатчику
            var uTdOut = GetAcp();
            CurrTd.Add(uTdOut);
            #endregion

        }

        private bool SerRelayValuePid(ClassDataPid pid)
        {
            if (pid.DeltaValue < 0)
                return false;

            return true;
        }
        private bool SerRelayValue()
        {
            if (CurrTd.AverTd > LastSetTempOut)
                return false;

            return true;
        }

        /// <summary>
        /// Чтение АЦП
        /// </summary>
        /// <returns>Значение прочитанное с АЦП</returns>
        public double GetAcp()
        {
            var contr7018 = LstContr[_clsBubblerStep.BubblerConst.Td.MasAcpConst[0].ContrNum] as ClassController7018;
            if (contr7018 != null)
                return contr7018.AcpValues[_clsBubblerStep.BubblerConst.Td.MasAcpConst[0].Port];

            var contr87017 = LstContr[_clsBubblerStep.BubblerConst.Td.MasAcpConst[0].ContrNum] as ClassController87017;
            if (contr87017 != null)
                return contr87017.AcpValues[_clsBubblerStep.BubblerConst.Td.MasAcpConst[0].Port];

            return 0.0;
        }

        public void SetPortState(bool portState)
        {
            var constBubler = _clsBubblerStep.BubblerConst;
            if (constBubler.MasDioConst == null)
                return;


            var contr = LstContr[constBubler.MasDioConst[0].ContrNum] as ClassController87057;
            contr.MasPortsState[constBubler.MasDioConst[0].Port] = portState;
        }

        public void CheckStatus()
        {

        }



    }

}
