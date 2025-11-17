using GasStation.Controllers;
using GasStation.Controllers.DI;
using GasStation.Devices.ControlChamber;
using GasStation.Elements.Data;
using GasStation.Status;
using GasStation.ViewModels.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Documents;
using System.Windows.Forms;
using static GasStation.xml.Script.EnumConst.ClassEnumWaterSensor;

namespace GasStation.Devices
{
    public class ClassControlTermoChamber : ClassBaseDevices
    {
        public ClassControlThermoSection[] ThermoSections;
        public DateTime dt=DateTime.Now;
        private XmlClassChamber _classChamberStep => (XmlClassChamber)_clsDevStep;

        private bool HeapState = true;
        private bool StateWater = false;

        public ClassDataChamber curr = new ClassDataChamber();
        public ClassControlTermoChamber(ClassDataTime classDataTime) : base(classDataTime)
        {
            
            //prevStateWater = _classChamberStep.ChamberConst.DioWaterConst.IsInverted ? true : false; ;
            //prevStateWaterForward = _classChamberStep.ChamberConst.DioWaterForwardConst.IsInverted ? true : false; ;
            //prevStateWaterBackward = _classChamberStep.ChamberConst.DioWaterBackwardConst.IsInverted ? true : false; ;
        }
        public void DataStep(XmlClassChamber classChamberStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
           
            _clsDevStep = classChamberStep;
            _states = states.ToArray();
            FlagStop = false;
            Init();

            HeapState = _classChamberStep.Heat;
            
            //_classChamberStep.ChamberView.RelayState = HeapState;
            SetPortState(HeapState);

            for (int sectionNum = 0; sectionNum < _classChamberStep.ChamberSections.Length; sectionNum++)
                ThermoSections[sectionNum].DataStep(_classChamberStep.ChamberSections[sectionNum], _states, stepParams);


            TimeToOff = _classChamberStep.ChamberConst.TimeToOff;
            _classChamberStep.ChamberView.EnableButton = true;
            HeatOff = false;
        }

        public override void Init()
        {
            if (ThermoSections != null)
                return;

            ThermoSections = new ClassControlThermoSection[_classChamberStep.ChamberConst.ThermoSectionConst.Length];
            for (int num = 0; num < _classChamberStep.ChamberConst.ThermoSectionConst.Length; num++)
            {
                ThermoSections[num] = new ClassControlThermoSection(_classDataTime);
                ThermoSections[num].LstContr = LstContr;
            }
            for (int i = 0; i < ThermoSections.Length; i++)
                ThermoSections[i].StateError += StateErrorHandler;
        }

        private int[] TimeToOff;
        private bool prevStateWater = false;
        private bool prevStateWaterForward= false;
        private bool prevStateWaterBackward = false;
        private bool StartFunc = false;
        private bool HeatOff = false;
        protected override void NextStepFunc()
        {
            CheckWater(EnumWaterSensor.Center);
            CheckWater(EnumWaterSensor.Backward);
            CheckWater(EnumWaterSensor.Forward);

            var ret = GetPortVoltage24State();
            if (ret != null)
            {
                _classChamberStep.ChamberView.IsVoltage24 = ret.Value;
                CheckVoltage24State();
            }


            #region Проверка заслонки
            if (_classChamberStep.ChamberConst.DioDumperOpenConst != null)
                _classChamberStep.ChamberView.OpenDumperStatus = GetDumperOpenStatus();
            if(_classChamberStep.ChamberConst.DioDumperCloseConst != null)
             _classChamberStep.ChamberView.CloseDumperStatus = GetDumperCloseStatus();
            #endregion


            HeapState = CrashThermocouple==false && _classChamberStep.Heat;

            SetPortState(HeapState);

            for (int i = 0; i < _classChamberStep.ChamberSections.Length; i++)
            {
                ThermoSections[i].NumSec = i;
                ThermoSections[i].NextStep();

            }
            CheckStatus();
            curr.StateHeat = HeapState;
        }

        public bool GetPortWaterState()
        {
            if (_classChamberStep.ChamberConst.DioWaterConst == null)
                return _classChamberStep.ChamberConst.DioWaterConst.IsInverted ? true : false;

            var contr = LstContr[_classChamberStep.ChamberConst.DioWaterConst.ContrNum] as BaseDIController;
            //return (contr.DioValues[_classChamberStep.ChamberConst.DioWaterConst.Port]);
            return _classChamberStep.ChamberConst.DioWaterConst.IsInverted ? !(contr.DioValues[_classChamberStep.ChamberConst.DioWaterConst.Port]) : (contr.DioValues[_classChamberStep.ChamberConst.DioWaterConst.Port]);
        }
        public bool GetPortWaterForwardState()
        {
            if (_classChamberStep.ChamberConst.DioWaterForwardConst == null)
                return false;
            if (_classChamberStep.ChamberConst.DioWaterForwardConst == null)
                return _classChamberStep.ChamberConst.DioWaterForwardConst.IsInverted ? true : false;

            var contr = LstContr[_classChamberStep.ChamberConst.DioWaterForwardConst.ContrNum] as ClassController87053;
            //return contr.DioValues[_classChamberStep.ChamberConst.DioWaterForwardConst.Port];
            return _classChamberStep.ChamberConst.DioWaterForwardConst.IsInverted ? !(contr.DioValues[_classChamberStep.ChamberConst.DioWaterForwardConst.Port]) : (contr.DioValues[_classChamberStep.ChamberConst.DioWaterForwardConst.Port]);
        }
        public bool GetPortWaterBackwardState()
        {
            if (_classChamberStep.ChamberConst.DioWaterBackwardConst == null)
                return false;

            if (_classChamberStep.ChamberConst.DioWaterBackwardConst == null)
                return _classChamberStep.ChamberConst.DioWaterBackwardConst.IsInverted ? true : false;

            var contr = LstContr[_classChamberStep.ChamberConst.DioWaterBackwardConst.ContrNum] as ClassController87053;
            return _classChamberStep.ChamberConst.DioWaterBackwardConst.IsInverted ? !(contr.DioValues[_classChamberStep.ChamberConst.DioWaterBackwardConst.Port]) : (contr.DioValues[_classChamberStep.ChamberConst.DioWaterBackwardConst.Port]);
//            return (contr.DioValues[_classChamberStep.ChamberConst.DioWaterBackwardConst.Port]);
        }

        public bool? GetPortVoltage24State()
        {
            if (_classChamberStep.ChamberConst.DioVoltage24Const == null)
                return null;

            if (_classChamberStep.ChamberConst.DioVoltage24Const == null)
                return _classChamberStep.ChamberConst.DioVoltage24Const.IsInverted ? true : false;

            var contr = LstContr[_classChamberStep.ChamberConst.DioVoltage24Const.ContrNum] as ClassController87053;
            return _classChamberStep.ChamberConst.DioVoltage24Const.IsInverted ? !(contr.DioValues[_classChamberStep.ChamberConst.DioVoltage24Const.Port]) :
                                                                                  (contr.DioValues[_classChamberStep.ChamberConst.DioVoltage24Const.Port]);
            //            return (contr.DioValues[_classChamberStep.ChamberConst.DioWaterBackwardConst.Port]);
            
        }

        private void CheckVoltage24State()
        {
            if (_classChamberStep.ChamberView.IsVoltage24 == false)
            {
                ConJumpArgs conJumpArgs1 = new ConJumpArgs(_classDataTime.TimeStep);
                conJumpArgs1.NumDev = _classChamberStep.Num;
                conJumpArgs1.NameDev = "Камера";
                conJumpArgs1.CurrValue = Convert.ToInt32(_classChamberStep.ChamberView.IsVoltage24);
                conJumpArgs1.TextError = $"Нет питания 24В";
                conJumpArgs1.Conditional = 0;
                conJumpArgs1.TypeConditional = TypeConditional.Alarm;
                AlarmError?.Invoke(this, conJumpArgs1);
                //HeatOff = true;
                //return;
            }

        }


        public bool GetDumperOpenStatus()
        {
            if (_classChamberStep.ChamberConst.DioDumperOpenConst == null)
                return false;
            var contr = LstContr[_classChamberStep.ChamberConst.DioDumperOpenConst.ContrNum] as ClassController87053;
            return (contr.DioValues[_classChamberStep.ChamberConst.DioDumperOpenConst.Port]);
        }
        public bool GetDumperCloseStatus()
        {
            if (_classChamberStep.ChamberConst.DioDumperCloseConst == null)
                return false;
            var contr = LstContr[_classChamberStep.ChamberConst.DioDumperCloseConst.ContrNum] as ClassController87053;
            return (contr.DioValues[_classChamberStep.ChamberConst.DioDumperCloseConst.Port]);
        }


        public void SetPortState(bool portState)
        {
            
            var tmpContr = LstContr[_classChamberStep.ChamberConst.DioHeaterConst.ContrNum];
            

            if (tmpContr is ClassController7042)
                ((ClassController7042)tmpContr).SetValue(Convert.ToInt32(portState));

            if (tmpContr is ClassController87057)
            {
                var portNum = _classChamberStep.ChamberConst.DioHeaterConst.Port;
                ((ClassController87057)tmpContr).MasPortsState[portNum]=portState;
            }
            //contr = LstContr[_classChamberStep.ChamberConst.DioHeaterConst.ContrNum] as ClassController7042;

            //contr = LstContr[_classChamberStep.ChamberConst.DioHeaterConst.ContrNum] as ClassController7057;
            //contr.SetValue(Convert.ToInt32(portState));
        }

        public void StateErrorHandler(object o, EventArgs e)
        {

            StateError?.Invoke(o, e);
        }

        private bool CrashThermocouple = false;
        
        public void CheckStatus()
        {
            if (curr != null)
            {
                if (curr.StateHeat != HeapState)
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _classChamberStep.Num;
                    conJumpArgs.NameDev = "Термокамера";
                    conJumpArgs.CurrValue = Convert.ToInt32(curr.StateHeat);
                    conJumpArgs.TextError = "Нагрев";
                    conJumpArgs.Conditional = 1;
                    conJumpArgs.TypeConditional = TypeConditional.Text;
                    AlarmError(this, conJumpArgs);

                }
            }

            #region Проверка аварийной температуры

            var termoSec = ThermoSections;

            foreach (var ts in termoSec)
            {
                var currSectionData = (ClassDataChamber)ts.LastData;
                if (currSectionData == null)
                    continue;

                



                if (currSectionData.ClassPidIn.CurrValue >= _classChamberStep.ChamberConst.CrashTemp)
                {
                    HeapState = false;
                    SetPortState(HeapState);
                    ConJumpArgs conJumpArgs = new ConJumpArgs(currSectionData.TimeStep);
                    conJumpArgs.NumDev = _classChamberStep.Num;
                    conJumpArgs.NameDev = "Термокамера";
                    conJumpArgs.CurrValue = currSectionData.ClassPidIn.CurrValue;

                    if (currSectionData.ClassPidIn.CurrValue >= 10000)
                    {
                        CrashThermocouple = true;
                        _classChamberStep.Heat = false;
                        SetPortState(false);
                        conJumpArgs.TextError = "Произошел обрыв термопары, нагрев будет отключен";
                        _classChamberStep.ChamberView.EnableButton = false;
                    }
                    else
                    {
                        CrashThermocouple = true;
                        _classChamberStep.Heat = false;
                        SetPortState(false);
                        _classChamberStep.ChamberView.EnableButton = false;
                        conJumpArgs.TextError = "Тепература превысила максимально допустимую, нагрев будет отключен";

                    }

                    conJumpArgs.Conditional = 1;
                    conJumpArgs.TypeConditional = TypeConditional.Alarm;
                    AlarmError?.Invoke(this, conJumpArgs);
                    return;
                }
                if (currSectionData.ClassPidOut.CurrValue >= _classChamberStep.ChamberConst.CrashTemp)
                {
                    HeapState = false;
                    SetPortState(HeapState);
                    ConJumpArgs conJumpArgs = new ConJumpArgs(currSectionData.TimeStep);
                    conJumpArgs.NumDev = _classChamberStep.Num;
                    conJumpArgs.NameDev = "Термокамера";
                    conJumpArgs.CurrValue = currSectionData.ClassPidOut.CurrValue;

                    if (currSectionData.ClassPidOut.CurrValue >= 10000)
                    {
                        CrashThermocouple = true;
                        _classChamberStep.Heat = false;
                        SetPortState(false);
                        _classChamberStep.ChamberView.EnableButton = false;
                        conJumpArgs.TextError = "Произошел обрыв термопары, нагрев будет отключен";
                    }
                    else
                    {
                        CrashThermocouple = true;
                        _classChamberStep.Heat = false;
                        SetPortState(false);
                        _classChamberStep.ChamberView.EnableButton = false;
                        conJumpArgs.TextError = "Тепература превысила максимально допустимую, нагрев будет отключен";

                    }

                    conJumpArgs.Conditional = 1;
                    conJumpArgs.TypeConditional = TypeConditional.Alarm;
                    AlarmError?.Invoke(this, conJumpArgs);
                    return;
                }

            }
            #endregion

            #region Проверка датчика воды

            List<XmlStateConditionScript> statesDWater = _states.Where(dat => dat.DevName == "Д воды на охлаждение камеры").ToList();
            int curValDWater = 0;

            for (int i = 0; i < statesDWater.Count; i++)
            {
                if ((_classDataTime.TimeStep > statesDWater[i].Timer) && (Convert.ToInt32(StateWater) != statesDWater[i].CurState))
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _classChamberStep.Num;
                    conJumpArgs.NameDev = "Термокамера";
                    conJumpArgs.CurrValue = curValDWater;
                    conJumpArgs.TextError = "Отсутствует охлаждение термокамеры";
                    conJumpArgs.Conditional = statesDWater[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);

                }
            }
            #endregion

            #region Условные переходы камеры
            foreach (var ts in ThermoSections)
            {
                checkSectionStatus(ts);
            }
            #endregion
        }


        void checkSectionStatus(ClassControlThermoSection ts)
        {
            #region Проверка температуры (газ)                
            var States = _states.Where(dat => dat.DevName == ts.ClassChamberSectionStep.ThermoSectionConst.RusName).ToList();
            var curr = (ClassDataChamber)ts.LastData;

            var currValue = ts.ClassChamberSectionStep.UsePid ? curr.ClassPidIn.CurrValue : curr.ClassPidOut.CurrValue;
           

            for (int i = 0; i < States.Count; i++)
            {
                if ((curr.DataTime.TimeStep > States[i].Timer) && ((currValue < States[i].ValueBegin) || (currValue > States[i].ValueEnd)))
                {
                    //EmergencyTemp = true;
                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.DataTime.TimeStep);
                    conJumpArgs.NumDev = ts.ClassChamberSectionStep.Num;
                    conJumpArgs.NameDev = ts.ClassChamberSectionStep.ThermoSectionConst.RusName;
                    conJumpArgs.CurrValue = currValue;
                    conJumpArgs.TextError = $"Значение температуры за границей допустимого диапазона {conJumpArgs.NameDev}";
                    conJumpArgs.Conditional = States[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;

                    StateError?.Invoke(this, conJumpArgs);
                }
            }
            #endregion

            //#region Проверка температуры (Центр)
            //if (NumSec != 1)
            //    return;
            //List<XmlStateConditionScript> States_SecCent = _states.Where(dat => dat.DevName == "Термосекция(Центр)").ToList();
            //double currValue_States_SecCent = 0;
            //if (_classChamberSectionStep.UsePid)
            //    currValue_States_SecCent = curr.ClassPidIn.CurrValue;
            //if (!_classChamberSectionStep.UsePid)
            //    currValue_States_SecCent = curr.ClassPidOut.CurrValue;

            //for (int i = 0; i < States_SecCent.Count; i++)
            //{
            //    if ((curr.DataTime.TimeScript > States_SecCent[i].Timer) && ((currValue_States_SecCent < States_SecCent[i].ValueBegin) || (currValue_States_SecCent > States_SecCent[i].ValueEnd)))
            //    {
            //        EmergencyTemp = true;
            //        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
            //        conJumpArgs.NumDev = _classChamberSectionStep.Num;
            //        conJumpArgs.NameDev = "Термосекция(Центр)";
            //        conJumpArgs.CurrValue = currValue_States_SecCent;
            //        conJumpArgs.TextError = "Значение температуры за границей допустимого диапазона";
            //        conJumpArgs.Conditional = States_SecCent[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
            //        Console.WriteLine(DateTime.Now);
            //        StateError?.Invoke(this, conJumpArgs);
            //    }
            //}
            //#endregion

            //#region Проверка температуры (Загрузчик)
            //if (NumSec != 1)
            //    return;
            //List<XmlStateConditionScript> States_SecLoad = _states.Where(dat => dat.DevName == "Термосекция(Загр.)").ToList();
            //double currValue_States_SecLoad = 0;
            //if (_classChamberSectionStep.UsePid)
            //    currValue_States_SecLoad = curr.ClassPidIn.CurrValue;
            //if (!_classChamberSectionStep.UsePid)
            //    currValue_States_SecLoad = curr.ClassPidOut.CurrValue;

            //for (int i = 0; i < States_SecLoad.Count; i++)
            //{
            //    if ((curr.DataTime.TimeScript > States_SecLoad[i].Timer) && ((currValue_States_SecLoad < States_SecLoad[i].ValueBegin) || (currValue_States_SecLoad > States_SecLoad[i].ValueEnd)))
            //    {
            //        EmergencyTemp = true;
            //        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
            //        conJumpArgs.NumDev = _classChamberSectionStep.Num;
            //        conJumpArgs.NameDev = "Термосекция(Загр.)";
            //        conJumpArgs.CurrValue = currValue_States_SecLoad;
            //        conJumpArgs.TextError = "Значение температуры за границей допустимого диапазона";
            //        conJumpArgs.Conditional = States_SecLoad[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
            //        Console.WriteLine(DateTime.Now);
            //        StateError?.Invoke(this, conJumpArgs);
            //    }
            //}
            //#endregion
        }

        private void CheckWater(EnumWaterSensor sensor)
        {
            int numDWater=0;
            bool stateDWater=true;

            if (sensor == EnumWaterSensor.Forward&& _classChamberStep.ChamberConst.DioWaterForwardConst!=null)
            {
                stateDWater = GetPortWaterForwardState();
                prevStateWaterForward = stateDWater;
                _classChamberStep.ChamberView.IsWaterForward = _classChamberStep.ChamberConst.DioWaterForwardConst.IsInverted ? !stateDWater : stateDWater;
                numDWater = 0;
            }

            if (sensor == EnumWaterSensor.Backward&& _classChamberStep.ChamberConst.DioWaterBackwardConst!=null)
            {
                stateDWater = GetPortWaterBackwardState();
                prevStateWaterBackward = stateDWater;
                _classChamberStep.ChamberView.IsWaterBackward = _classChamberStep.ChamberConst.DioWaterBackwardConst.IsInverted ? !stateDWater : stateDWater;
                
                numDWater = 1;
            }

            if (sensor == EnumWaterSensor.Center&& _classChamberStep.ChamberConst.DioWaterConst!=null)
            {
                stateDWater = GetPortWaterState();
                prevStateWater = stateDWater;
                _classChamberStep.ChamberView.IsWater = _classChamberStep.ChamberConst.DioWaterConst.IsInverted ? !stateDWater : stateDWater;
                numDWater = 2;
            }

            if (!stateDWater)
            {
                if (TimeToOff[numDWater] < 1)
                {
                    _classChamberStep.Heat = false;
                    SetPortState(_classChamberStep.Heat);
                    _classChamberStep.ChamberView.EnableButton = false;

                    if (HeatOff == true)
                        return;


                    if (_classChamberStep.ChamberView.EnableButton == false)

                    {
                        ConJumpArgs conJumpArgs1 = new ConJumpArgs(_classDataTime.TimeStep);
                        conJumpArgs1.NumDev = _classChamberStep.Num;
                        conJumpArgs1.NameDev = "Камера";
                        conJumpArgs1.CurrValue = Convert.ToInt32(stateDWater);
                        conJumpArgs1.TextError = $"Нагрев печи отключен";
                        conJumpArgs1.Conditional = 0;
                        conJumpArgs1.TypeConditional = TypeConditional.Alarm;
                        AlarmError?.Invoke(this, conJumpArgs1);
                        HeatOff = true;
                        return;
                    }
                }

                TimeToOff[numDWater] = TimeToOff[numDWater] - 1;
                ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                conJumpArgs.NumDev = _classChamberStep.Num;
                conJumpArgs.NameDev = "Камера";
                conJumpArgs.CurrValue = Convert.ToInt32(stateDWater);
                conJumpArgs.TextError =$"Тревога! Нет водяного охлаждения, нагрев будет отключен через {TimeToOff[numDWater]} секунд";
                conJumpArgs.Conditional = 0;
                conJumpArgs.TypeConditional = TypeConditional.LogWrite;
                if(TimeToOff[numDWater]== _classChamberStep.ChamberConst.TimeToOff[numDWater]-1)
                    conJumpArgs.TypeConditional = TypeConditional.Alarm;
                else
                    conJumpArgs.TypeConditional = TypeConditional.LogWrite;

                AlarmError?.Invoke(this, conJumpArgs);
            }

            else
                TimeToOff[numDWater] = _classChamberStep.ChamberConst.TimeToOff[numDWater];



        }
    }
}
