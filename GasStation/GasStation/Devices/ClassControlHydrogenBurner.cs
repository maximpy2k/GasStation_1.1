using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Elements.Data;
using GasStation.ViewModels.Elements;
using GasStation.Controllers;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.xml.Script.XmlScript;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.Mathem.Chamber;
using GasStation.Status;
using System.Windows;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using ChartApplication.points;

namespace GasStation.Devices
{
    public class ClassControlHydrogenBurner : ClassBaseDevices
    {
        /// <summary>
        /// Класс параметров  шага устройства
        /// </summary>
        private XmlClassHydrogenBurning _classHydrogenBurnerStep => (XmlClassHydrogenBurning)_clsDevStep;

        private ClassBaseTd _tdBurner;
        private ClassBaseTd _tdFire;

        ///// <summary>
        ///// Список для хранения принятых или отправляемых данных
        ///// </summary>
        //public List<ClassDataHydrogenBurner> DataList;

        public ClassControlHydrogenBurner(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        public void DataStep(XmlClassHydrogenBurning classHydrogenBurningStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            //ClrDataList();

            _clsDevStep = classHydrogenBurningStep;
            _stepParams = stepParams;


            _tdBurner = new ClassBaseTd(_classHydrogenBurnerStep.HydrogenBurnerConst.Td[0]);

            if(_classHydrogenBurnerStep.HydrogenBurnerConst.Td.Length>1)
                _tdFire = new ClassBaseTd(_classHydrogenBurnerStep.HydrogenBurnerConst.Td[1]);

            _states = states.ToArray();
            newStep = true;
            Init();
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

        /// <summary>
        /// Расчеты на каждом последующем шаге
        /// </summary>
        protected override void NextStepFunc()
        {
            _tdBurner.Add(GetAcpTdBurner());
            if(_tdFire!= null)
                _tdFire.Add(GetAcpTdFire());

            var last = (ClassDataHydrogenBurner)LastData;
            var curr = new ClassDataHydrogenBurner(_classDataTime);

            curr.TdBurner = _tdBurner.AverTd;
           if(_tdFire!= null)
                curr.TdFire = _tdFire.AverTd;
            
            curr.StateWater = GetPortWaterState();
            curr.StateFire = GetPortFireState();


            #region Определение вкл/выкл нагрева
            if (curr.TdBurner < _classHydrogenBurnerStep.HydrogenBurnerConst.SetupTemp && _classHydrogenBurnerStep.Heat)
                curr.StateRelay = true;
            else
                curr.StateRelay = false;
            #endregion

            #region Предупреждение по датчику воды                      
            if ((last == null && !curr.StateWater) || (last != null && !curr.StateWater && last.StateWater))
            {
                ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep)
                {
                    NumDev = _classHydrogenBurnerStep.Num,
                    NameDev = "Горелка",
                    CurrValue = Convert.ToInt32(curr.StateWater),
                    Conditional = 0,
                    TextError = "Нет водяного охлаждения",
                    TypeConditional = TypeConditional.Alarm
                };
                AlarmError?.Invoke(this, conJumpArgs);
            }
            #endregion


            #region Предупреждение по датчику пламени                      
            if ((last == null && curr.StateFire) || (last != null && curr.StateFire && !last.StateFire))
            {
                ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep)
                {
                    NumDev = _classHydrogenBurnerStep.Num,
                    NameDev = "Горелка",
                    CurrValue = Convert.ToInt32(curr.StateFire),
                    Conditional = 0,
                    TextError = "Сработал датчик пламени",
                    TypeConditional = TypeConditional.Alarm
                };
                AlarmError?.Invoke(this, conJumpArgs);
            }
            #endregion

            CheckStatus(curr);
            SetPortRelay(_classHydrogenBurnerStep.Heat);
            SetPortHeater(_classHydrogenBurnerStep.HydrogenBurnerView.Relay);

            if (last != null)
            {
                if (curr.StateHeat != last.StateHeat)
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _classHydrogenBurnerStep.Num;
                    conJumpArgs.NameDev = "Нагреватель горелки";
                    conJumpArgs.CurrValue = Convert.ToInt32(curr.StateHeat);
                    conJumpArgs.TextError = "Состояние нагрева изменено";
                    conJumpArgs.Conditional = 1;
                    conJumpArgs.TypeConditional = TypeConditional.Text;
                    AlarmError(this, conJumpArgs);
                }
            }

            if (AddData(curr))
            {
                _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdFire.PointsPrepare.Clr(Cnt); ;
                _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdHeaterHydrogenBurner.PointsPrepare.Clr(Cnt);
            }

            if(_classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdFire!=null)
                _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdFire.Add(new PointTime(_classDataTime.BeginCycleStep, curr.TdFire));
            
            _classHydrogenBurnerStep.HydrogenBurnerView.SeriesTdHeaterHydrogenBurner.Add(new PointTime(_classDataTime.BeginCycleStep, curr.TdBurner));

            _classHydrogenBurnerStep.HydrogenBurnerView.TdHeaterHydrogenBurner = curr.TdBurner;
            _classHydrogenBurnerStep.HydrogenBurnerView.TdFire = curr.TdFire;
            _classHydrogenBurnerStep.HydrogenBurnerView.Relay = curr.StateRelay;
            _classHydrogenBurnerStep.HydrogenBurnerView.IsWater = curr.StateWater;
            _classHydrogenBurnerStep.HydrogenBurnerView.IsFire = curr.StateFire;
        }

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
            //var contr = LstContr[_classHydrogenBurnerStep.HydrogenBurnerConst.DioWaterConst.ContrNum] as ClassController87053;
            //return (contr.DioValues[_classHydrogenBurnerStep.HydrogenBurnerConst.DioWaterConst.Port]);

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


            return (!isInverted ? val : !val);
        }

        public void CheckStatus(ClassDataHydrogenBurner curr)
        {
            #region Проверка температуры пламени
            var StatesT = _states.Where(dat => dat.DevName == "Т пламени").Where(dat1 => dat1.DevNum == _classHydrogenBurnerStep.HydrogenBurnerConst.TdFireNum).ToList();
            for (int i = 0; i < StatesT.Count; i++)
            {
                {
                    if ((curr.TimeStep > StatesT[i].Timer) && (curr.TdFire < StatesT[i].ValueBegin) || (curr.TdFire > StatesT[i].ValueEnd))
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
                if ((curr.TimeStep > StatesTHeat[i].Timer) && (curr.TdBurner < StatesTHeat[i].ValueBegin) || (curr.TdBurner > StatesTHeat[i].ValueEnd))
                {
                    FlagStop = true;

                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _classHydrogenBurnerStep.Num;
                    conJumpArgs.NameDev = "Горелка";
                    conJumpArgs.CurrValue = curr.TdBurner;
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
