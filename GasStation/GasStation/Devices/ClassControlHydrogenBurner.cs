using ChartApplication.points;
using GasStation.Controllers;
using GasStation.Elements.Data;
using GasStation.Mathem.Bubbler;
using GasStation.Mathem.Chamber;
using GasStation.Mathem.Hydrogen;
using GasStation.Status;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.XmlScript;
using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GasStation.Devices
{
    public class ClassControlHydrogenBurner : ClassBaseDevices
    {
        /// <summary>
        /// Класс параметров  шага устройства
        /// </summary>
        private XmlClassHydrogenBurning _classHydrogenBurnerStep => (XmlClassHydrogenBurning)_clsDevStep;

        private ClassBaseTd _currTdBurner;
        /// <summary>
        /// Данные по термодатчику
        /// </summary>
        protected ClassBaseTd CurrTdBurner
        {
            get
            {
                if (_currTdBurner != null)
                    return _currTdBurner;
                _currTdBurner = new ClassBaseTd(_classHydrogenBurnerStep.HydrogenBurnerConst.TdBurner);
                return _currTdBurner;
            }
        }


        private ClassBaseTd _currTdFire;
        /// <summary>
        /// Данные по термодатчику
        /// </summary>
        protected ClassBaseTd CurrTdFire
        {
            get
            {
                if (_currTdFire != null)
                    return _currTdFire;
                _currTdFire = new ClassBaseTd(_classHydrogenBurnerStep.HydrogenBurnerConst.TdFire);
                return _currTdFire;
            }
        }



        ///// <summary>
        ///// Список для хранения принятых или отправляемых данных
        ///// </summary>
        //public List<ClassDataHydrogenBurner> DataList;

        public ClassControlHydrogenBurner(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        /// <summary>
        /// Экземпляр класса вычисления текущего расчета
        /// </summary>
        private ClassHydrogenSetEnable _clsHydrogenSetEnable;

        private ClassHydrogenPidRegulation _hydrogenPidRegulation;
        /// <summary>
        /// Класс пид регулятора
        /// </summary>
        private ClassHydrogenPidRegulation ClsHydrogenPidRegulation
        {
            get
            {
                if (_hydrogenPidRegulation != null)
                    return _hydrogenPidRegulation;
                _hydrogenPidRegulation = new ClassHydrogenPidRegulation();
                return _hydrogenPidRegulation;
            }
        }

        private double LastSetTempOut = -25;

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
                _hydrogenPidRegulation = null;
                _usePid = value;
            }
        }
        /// <summary>
        /// Расчеты на каждом последующем шаге
        /// </summary>
        private bool firstStateWater = true;
        private bool firstStateFire = true;

        protected override void NextStepFunc()
        {

            var last = (ClassDataHydrogenBurner)LastData;
            ReadTd();

            #region Расчет заданой температуры
            double setTemp = _clsHydrogenSetEnable.NextStep(_classDataTime);
            #endregion
            LastSetTempOut = setTemp;

            ClassDataHydrogenBurner curr = _hydrogenPidRegulation.NextStep(_classDataTime, CurrTdBurner.AverTd, setTemp);

            curr.SetupTemp = _classHydrogenBurnerStep.SetupValue;
            curr.UseHydrogen = _classHydrogenBurnerStep.Heat;
            curr.StateWater = GetPortWaterState();
            curr.StateFire = GetPortFireState();

            var relay = false;
            if (curr.UseHydrogen)
            {
                relay = SerRelayValuePid(curr.ClassPidOut);
            }
            curr.StateRelay = relay;

            SetPortState(_classHydrogenBurnerStep.HydrogenBurnerView.Relay);
            SetPortHeatState(curr.UseHydrogen);
            var dateNow = DateTime.Now;

            if (last != null)
            {
                if (curr.StateWater != last.StateWater || firstStateWater)
                {
                    if (!curr.StateWater)

                    {
                        ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                        conJumpArgs.NumDev = _classHydrogenBurnerStep.Num;
                        conJumpArgs.NameDev = "Горелка";
                        conJumpArgs.CurrValue = Convert.ToInt32(curr.StateWater);
                        conJumpArgs.TextError = "Водяное охлаждение отсутствует";
                        conJumpArgs.Conditional = 1;
                        conJumpArgs.TypeConditional = TypeConditional.Alarm;
                        AlarmError(this, conJumpArgs);
                        firstStateWater = false;
                    }
                }

                if (curr.StateFire != last.StateFire || firstStateFire)
                {
                    if (curr.StateFire)
                    {
                        ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                        conJumpArgs.NumDev = _classHydrogenBurnerStep.Num;
                        conJumpArgs.NameDev = "Горелка";
                        conJumpArgs.CurrValue = Convert.ToInt32(curr.StateFire);
                        conJumpArgs.TextError = "Сработал датчик пламени";
                        conJumpArgs.Conditional = 1;
                        conJumpArgs.TypeConditional = TypeConditional.Text;
                        AlarmError(this, conJumpArgs);
                        firstStateFire = false;
                    }
                }

                if (curr.UseHydrogen != last.UseHydrogen)
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _classHydrogenBurnerStep.Num;
                    conJumpArgs.NameDev = "Использование горелки";
                    conJumpArgs.CurrValue = Convert.ToInt32(curr.UseHydrogen);
                    conJumpArgs.TextError = "Состояние использования горелки изменено";
                    conJumpArgs.Conditional = 1;
                    conJumpArgs.TypeConditional = TypeConditional.Text;
                    AlarmError(this, conJumpArgs);
                }
            }

            if (AddData(curr))
            {
                _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdHeaterHydrogenBurner.PointsPrepare.Clr(Cnt);
                if (_classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdFire != null)
                    _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdFire.PointsPrepare.Clr(Cnt);
                _classHydrogenBurnerStep.HydrogenBurnerView.SeriesSetTemp.PointsPrepare.Clr(Cnt);
            }


            #region Вывод на форму  
            _classHydrogenBurnerStep.HydrogenBurnerView.DataHydrogen = curr;
            _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdHeaterHydrogenBurner.Add(new PointTime(curr.CurrDate, CurrTdBurner.AverTd));

            if (_classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdFire != null)
                _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdFire.Add(new PointTime(curr.CurrDate, CurrTdFire.AverTd));

            _classHydrogenBurnerStep.HydrogenBurnerView.SeriesSetTemp.Add(new PointTime(curr.CurrDate, LastSetTempOut));
            _classHydrogenBurnerStep.HydrogenBurnerView.IsFire = curr.StateFire;
            _classHydrogenBurnerStep.HydrogenBurnerView.IsWater = curr.StateWater;
            _classHydrogenBurnerStep.HydrogenBurnerView.TdFire = CurrTdFire.AverTd;
            _classHydrogenBurnerStep.HydrogenBurnerView.Relay = curr.StateRelay;
            _classHydrogenBurnerStep.HydrogenBurnerView.TdHeaterHydrogenBurner = CurrTdBurner.AverTd;
            _classHydrogenBurnerStep.Heat = curr.UseHydrogen;
            #endregion
            CheckStatus(curr);
        }
        private void ReadTd()
        {
            #region Расчет температуры по внешнему термодатчику
            var uTdOut = GetAcp(_classHydrogenBurnerStep.HydrogenBurnerConst.TdBurner);
            CurrTdBurner.Add(uTdOut);
            #endregion

        }

        /// <summary>
        /// Чтение АЦП
        /// </summary>
        /// <returns>Значение прочитанное с АЦП</returns>
        public double GetAcp(XmlTdConst tdConst)
        {
            var contr7018 = LstContr[tdConst.MasAcpConst[0].ContrNum] as ClassController7018;
            if (contr7018 != null)
                return contr7018.AcpValues[tdConst.MasAcpConst[0].Port];

            var contr87017 = LstContr[tdConst.MasAcpConst[0].ContrNum] as ClassController87017;
            if (contr87017 != null)
                return contr87017.AcpValues[tdConst.MasAcpConst[0].Port];

            return 0.0;
        }

        private bool SerRelayValuePid(ClassDataPid pid)
        {
            if (pid.DeltaValue < 0)
                return false;
            return true;
        }
        private bool SerRelayValue()
        {
            if (CurrTdBurner.AverTd > LastSetTempOut)
                return false;

            return true;
        }

        public void SetPortHeatState(bool portState)
        {
            var constHydrogen = _classHydrogenBurnerStep.HydrogenBurnerConst;
            if (constHydrogen.DioHeaterConst == null)
                return;


            var contr = LstContr[constHydrogen.DioHeaterConst.ContrNum] as ClassController87057;
            contr.MasPortsState[constHydrogen.DioHeaterConst.Port] = portState;
        }
        public void SetPortState(bool portState)
        {
            var constHydrogen = _classHydrogenBurnerStep.HydrogenBurnerConst;
            if (constHydrogen.DioRealyConst == null)
                return;


            var contr = LstContr[constHydrogen.DioRealyConst.ContrNum] as ClassController87057;
            contr.MasPortsState[constHydrogen.DioRealyConst.Port] = portState;
        }

        public void DataStep(XmlClassHydrogenBurning clsHydrogenStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            Init();
            _clsDevStep = clsHydrogenStep;
            _states = states.ToArray();
            _stepParams = stepParams;


            ClsHydrogenPidRegulation.DataStep(_classHydrogenBurnerStep.HydrogenBurnerConst, _classHydrogenBurnerStep);

            ReadTd();

            LastSetTempOut = LastSetTempOut == -25 ? CurrTdBurner.AverTd : LastSetTempOut;
            _clsHydrogenSetEnable = new ClassHydrogenSetEnable(_classHydrogenBurnerStep, LastSetTempOut);

            if (File.Exists($"{path}\\HydrogenBurner{_classHydrogenBurnerStep.Num}.txt"))
                File.Delete($"{path}\\HydrogenBurner{_classHydrogenBurnerStep.Num}.txt");
        }


        /// <summary>
        /// Расчеты на первом шаге
        /// </summary>
        public override void Init()
        {
            FlagStop = false;
            base.Init();
        }

        private bool prevStateWater = true;


        private int currWaterTimer = 60;


        public double GetAcpTdBurner()
        {
            var contr = LstContr[_classHydrogenBurnerStep.HydrogenBurnerConst.Td[0].MasAcpConst[0].ContrNum] as ClassController7018;
            return contr.AcpValues[_classHydrogenBurnerStep.HydrogenBurnerConst.Td[0].MasAcpConst[0].Port];
        }

        public double GetAcpTdFire()
        {
            var contr = LstContr[_classHydrogenBurnerStep.HydrogenBurnerConst.Td[1].MasAcpConst[0].ContrNum] as ClassController7018;
            return (contr.AcpValues[_classHydrogenBurnerStep.HydrogenBurnerConst.Td[1].MasAcpConst[0].Port]);
        }

        public void SetPortHeater(bool portState)
        {
            var contr = LstContr[_classHydrogenBurnerStep.HydrogenBurnerConst.DioHeaterConst.ContrNum] as ClassController87057;
            contr.MasPortsState[_classHydrogenBurnerStep.HydrogenBurnerConst.DioHeaterConst.Port] = portState;
        }

        public void SetPortRelay(bool portState)
        {
            var contr = LstContr[_classHydrogenBurnerStep.HydrogenBurnerConst.DioRealyConst.ContrNum] as ClassController87057;
            contr.MasPortsState[_classHydrogenBurnerStep.HydrogenBurnerConst.DioRealyConst.Port] = portState;
        }

        private int waterTimerAlarm = 60;

        public bool GetPortWaterState()
        {

            var con = _classHydrogenBurnerStep.HydrogenBurnerConst.DioWaterConst;
            if (con is null)
                return true;

            var contr = LstContr[con.ContrNum] as ClassController87053;

            var val = contr.DioValues[con.Port];
            var state = con.IsInverted ? !val : val;
            return state;

        }
        public bool GetPortFireState()
        {
            var contr = LstContr[_classHydrogenBurnerStep.HydrogenBurnerConst.DioFireConst.ContrNum] as ClassController87053;

            var val = contr.DioValues[_classHydrogenBurnerStep.HydrogenBurnerConst.DioFireConst.Port];
            var isInverted = _classHydrogenBurnerStep.HydrogenBurnerConst.DioFireConst.IsInverted;
            var returnVal = (!isInverted ? val : !val);

            return returnVal;
        }

        public void CheckStatus(ClassDataHydrogenBurner curr)
        {
            #region Проверка температуры пламени
            var StatesT = _states.Where(dat => dat.DevName == "Т пламени").Where(dat1 => dat1.DevNum == _classHydrogenBurnerStep.HydrogenBurnerConst.TdFireNum).ToList();
            for (int i = 0; i < StatesT.Count; i++)
            {
                {
                    if ((curr.TimeStep > StatesT[i].Timer) && (CurrTdFire.AverTd < StatesT[i].ValueBegin) || (CurrTdFire.AverTd > StatesT[i].ValueEnd))
                    {
                        FlagStop = true;

                        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                        conJumpArgs.NumDev = _classHydrogenBurnerStep.Num;
                        conJumpArgs.NameDev = "Горелка";
                        conJumpArgs.CurrValue = curr.TdFire;
                        conJumpArgs.TextError = "Значение температуры за границей допустимого диапазона";
                        conJumpArgs.Conditional = StatesT[i].NumStep;
                        conJumpArgs.TypeConditional = TypeConditional.Error;
                        StateError?.Invoke(this, conJumpArgs);
                        Console.WriteLine($"{curr.TimeStep} {conJumpArgs.TextError}");
                    }
                }
            }
            #endregion

            #region Проверка температуры нагревателя
            var StatesTHeat = _states.Where(dat => dat.DevName == "Т горелки").Where(dat1 => dat1.DevNum == _classHydrogenBurnerStep.HydrogenBurnerConst.TdBurnerNum).ToList();

            for (int i = 0; i < StatesTHeat.Count; i++)
            {
                if ((curr.TimeStep > StatesTHeat[i].Timer) && (CurrTdBurner.AverTd < StatesTHeat[i].ValueBegin) || (CurrTdBurner.AverTd > StatesTHeat[i].ValueEnd))
                {
                    FlagStop = true;

                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _classHydrogenBurnerStep.Num;
                    conJumpArgs.NameDev = "Горелка";
                    conJumpArgs.CurrValue = CurrTdBurner.AverTd;
                    conJumpArgs.TextError = "Значение температуры горелки за границей допустимого диапазона";
                    conJumpArgs.Conditional = StatesTHeat[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);

                }
            }
            #endregion

            #region Проверка датчика пламени
            var StatesDFire =
                _states.Where(dat => dat.DevName == "Д пламени")
                    .Where(dat1 => dat1.DevNum == _classHydrogenBurnerStep.HydrogenBurnerConst.DioFireConst.DevNum)
                    .ToList();

            for (int i = 0; i < StatesDFire.Count; i++)
            {
                var stateFire = Convert.ToBoolean(StatesDFire[i].CurState);
                if ((curr.TimeStep > StatesDFire[i].Timer) && (curr.StateFire != stateFire))
                {
                    FlagStop = true;
                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _classHydrogenBurnerStep.HydrogenBurnerConst.DioFireConst.DevNum;
                    conJumpArgs.NameDev = "Д пламени";
                    conJumpArgs.CurrValue = Convert.ToDouble(curr.StateFire);
                    conJumpArgs.TextError = "Пламя отсутсвует";
                    conJumpArgs.Conditional = StatesDFire[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);

                }
            }
            #endregion

            #region Проверка датчика воды
            var StatesDWater =
                _states.Where(dat => dat.DevName == "Д воды на охлаждение горелки")
                    .Where(dat1 => dat1.DevNum == _classHydrogenBurnerStep.HydrogenBurnerConst.DioWaterConst.DevNum)
                    .ToList();

            for (int i = 0; i < StatesDWater.Count; i++)
            {
                if ((curr.TimeStep > StatesDWater[i].Timer) && (Convert.ToInt32(curr.StateWater) != StatesDWater[i].CurState))
                {
                    FlagStop = true;
                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _classHydrogenBurnerStep.HydrogenBurnerConst.DioFireConst.DevNum;
                    conJumpArgs.NameDev = "Д воды на охлаждение горелки";
                    conJumpArgs.CurrValue = Convert.ToDouble(curr.StateWater);
                    conJumpArgs.TextError = "Нет охлаждения горелки";
                    conJumpArgs.Conditional = StatesDWater[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);

                }
            }
            #endregion
        }
    }
}
