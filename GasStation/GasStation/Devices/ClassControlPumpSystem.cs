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

namespace GasStation.Devices
{
    public class ClassControlPumpSystem : ClassBaseDevices
    {

        /// <summary>
        /// Параметры текущего шага
        /// </summary>
        private XmlClassPumpSys _clsPumpSystemStep=>(XmlClassPumpSys)_clsDevStep;

        /// <summary>
        /// Список для хранения принятых или отправляемых данных
        /// </summary>
        public List<ClassDataPumpSysyem> DataList = new List<ClassDataPumpSysyem>();
        
        public ClassControlPumpSystem(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

    
        public void DataStep(XmlClassPumpSys clsPumpSystemStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = clsPumpSystemStep;
            startTimerVacuum = false;
            _states = states.ToArray();
            
            _stepParams = stepParams;
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

        /// <summary>
        /// Расчеты на каждом последующем шаге
        /// </summary>
        protected override void NextStepFunc()
        {
            var dateNow = DateTime.Now;
            ClassDataPumpSysyem classData = new ClassDataPumpSysyem(_classDataTime) { LampRoughingPump = GetPortStatusRoughingPumpState(), 
                                                                                      LampVaccuumPump = GetPortStatusVacuumPumpState() };
            classData.UseVacuumPump = _clsPumpSystemStep.UseVacuumPump;
            classData.UseRoughingPump = _clsPumpSystemStep.UseRoughingPump;
            #region Сообщения по Вакуумному насосу
            if (DataList.Count != 0 && classData.UseVacuumPump != DataList.Last().UseVacuumPump)
            {
                ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                conJumpArgs.NumDev = _clsPumpSystemStep.Num;
                conJumpArgs.NameDev = "Клапан";
                conJumpArgs.CurrValue = 0;
                conJumpArgs.TextError = classData.UseVacuumPump == true ? "Вакуумный насос включен" : "Вакуумный насос выключен";
                conJumpArgs.Conditional = 1;
                conJumpArgs.TypeConditional = TypeConditional.Text;
                AlarmError(this, conJumpArgs);
            }

            if (classData.UseVacuumPump == true && classData.LampVaccuumPump == false)
            {
                ConJumpArgs conJumpArgs1 = new ConJumpArgs(_classDataTime.TimeStep);
                conJumpArgs1.NumDev = _clsPumpSystemStep.Num;
                conJumpArgs1.NameDev = "Вакуумный насос";
                conJumpArgs1.CurrValue = Convert.ToInt32(classData.UseVacuumPump);
                conJumpArgs1.TextError = $"Вакуумный насос выключен";
                conJumpArgs1.Conditional = 0;
                conJumpArgs1.TypeConditional = TypeConditional.Alarm;
                AlarmError?.Invoke(this, conJumpArgs1);
            }
            #endregion


            #region Сообщения по Вакуумному насосу
            if (DataList.Count != 0 && classData.UseRoughingPump != DataList.Last().UseRoughingPump)
            {
                ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                conJumpArgs.NumDev = _clsPumpSystemStep.Num;
                conJumpArgs.NameDev = "Клапан";
                conJumpArgs.CurrValue = 0;
                conJumpArgs.TextError = classData.UseRoughingPump == true ? "Форвакуумный насос включен" : "Форвакуумный насос выключен";
                conJumpArgs.Conditional = 1;
                conJumpArgs.TypeConditional = TypeConditional.Text;
                AlarmError(this, conJumpArgs);
            }

            if (classData.UseRoughingPump == true && classData.LampRoughingPump == false)
            {
                ConJumpArgs conJumpArgs1 = new ConJumpArgs(_classDataTime.TimeStep);
                conJumpArgs1.NumDev = _clsPumpSystemStep.Num;
                conJumpArgs1.NameDev = "Форвакуумный насос";
                conJumpArgs1.CurrValue = Convert.ToInt32(classData.LampRoughingPump);
                conJumpArgs1.TextError = $"Форвакуумный насос выключен";
                conJumpArgs1.Conditional = 0;
                conJumpArgs1.TypeConditional = TypeConditional.Alarm;
                AlarmError?.Invoke(this, conJumpArgs1);
            }
            #endregion

            DataList.Add(classData);


            #region Управлением  вакуметром
            if (_clsPumpSystemStep.PumpSysConst.VacuumetrConst != null)
            {
                classData.CurrPress = GetPress();
                SetVacuumetDoState(Convert.ToDouble(classData.CurrPress));
                _clsPumpSystemStep.PumpSysView.VacuumetrView.CurrentPress = Convert.ToDouble(classData.CurrPress);
                _clsPumpSystemStep.PumpSysView.VacuumetrView.SeriesReadPress.Add(new PointTime(dateNow, Convert.ToDouble(classData.CurrPress)));
            }
            #endregion

            classData.UseVacuumPump = _clsPumpSystemStep.UseVacuumPump;
            classData.UseRoughingPump = _clsPumpSystemStep.UseRoughingPump;

            //classData.RealStateVaccuumPump = classData.UseVacuumPump && (classData.CurrPress < _clsPumpSystemStep.PumpSysConst.VacuumPump.MinPressure);
            classData.RealStateVaccuumPump =  classData.UseVacuumPump;
            classData.RealStateRoughingPump = classData.UseRoughingPump && (classData.CurrPress < _clsPumpSystemStep.PumpSysConst.RoughingPump.MinPressure);

            CheckStatus();

            SetVacuumPumpState(classData.RealStateVaccuumPump);
            SetRoughingPumpState(classData.RealStateRoughingPump);
            
            #region Обнуление

            var cnt = 3600;
            if (DataList.Count > cnt && _stepParams.NumStep == 1)
            {
                _clsPumpSystemStep.PumpSysView.VacuumetrView.SeriesReadPress.PointsPrepare.Clr(cnt);                
                DataList = DataList.Where((dat, idx) => idx >= DataList.Count - cnt).ToList();
            }

            #endregion

            _clsPumpSystemStep.PumpSysView.WorkUseRoughingPump = classData.RealStateRoughingPump;

            _clsPumpSystemStep.PumpSysView.RoughingPumpLamp = classData.LampRoughingPump;
            _clsPumpSystemStep.PumpSysView.VacuumPumpLamp = classData.LampVaccuumPump;
            //var step = new XmlClassStep(_clsPumpSystemStep.XmlNode.ParentNode);
            //log.AddToLog(_clsPumpSystemStep.XmlNode, DataList.Last(), step.StepParams.NumStep);
        }
                
        /// <summary>
        /// Чтение датчика давления
        /// </summary>
        /// <returns>Значение прочитанное с АЦП</returns>
        public double GetPress()
        {
            var con = _clsPumpSystemStep.PumpSysConst.VacuumetrConst;
            var contr = LstContr[con.AcpConst.ContrNum] as ClassController87017;
            var u = contr.AcpValues[con.AcpConst.Port];
            return con.AcpConst.GetValue(u);
        }

        public bool GetPortStatusVacuumPumpState()
        {
            if (_clsPumpSystemStep.PumpSysConst.VacuumPump.StatusConst == null)
                return false;
            var contr = LstContr[_clsPumpSystemStep.PumpSysConst.VacuumPump.StatusConst.VacuumPumpLamp.ContrNum] as ClassController87053;
            return (contr.DioValues[_clsPumpSystemStep.PumpSysConst.VacuumPump.StatusConst.VacuumPumpLamp.Port]);
        }

        public bool GetPortStatusRoughingPumpState()
        {
            if (_clsPumpSystemStep.PumpSysConst.RoughingPump.StatusConst == null)
                return false;
            var contr = LstContr[_clsPumpSystemStep.PumpSysConst.RoughingPump.StatusConst.RoughingPumpLamp.ContrNum] as ClassController87053;
            return (contr.DioValues[_clsPumpSystemStep.PumpSysConst.RoughingPump.StatusConst.RoughingPumpLamp.Port]);
        }

        /// <summary>
        /// Управление включением вакуумного насоса
        /// </summary>
        /// <param name="portState">Состояние насоса</param>
        public void SetVacuumPumpState(bool portState)
        {
            var con = _clsPumpSystemStep.PumpSysConst.VacuumPump;

            var contr = LstContr[con.PumpDioConst.ContrNum] as ClassController87057;
            contr.MasPortsState[con.PumpDioConst.Port] = portState;
        }

        /// <summary>
        /// Управление включением форвакуумного насоса
        /// </summary>
        /// <param name="portState">Состояние насоса</param>
        public void SetRoughingPumpState(bool portState)
        {
            var con = _clsPumpSystemStep.PumpSysConst.RoughingPump;
            var contr = LstContr[con.PumpDioConst.ContrNum] as ClassController87057;
            contr.MasPortsState[con.PumpDioConst.Port] = portState;

        }

        /// <summary>
        /// Управление выходом вакуметра
        /// </summary>
        /// <param name="press">Показание вакуметра</param>
        public void SetVacuumetDoState(double press)
        {
            var con = _clsPumpSystemStep.PumpSysConst.VacuumetrConst;
            if (con.DioConst == null)
                return;
            var contr = LstContr[con.DioConst.ContrNum] as ClassController87057;
            _clsPumpSystemStep.PumpSysView.VacuumetrView.PressDio = con.CheckCondition(press);
            contr.MasPortsState[con.DioConst.Port] = con.CheckCondition(press);

        }


        public DateTime timeErrorDataVacuum = DateTime.Now;
        public bool startTimerVacuum = false;

        public void CheckStatus()
        {
            var States = _states.Where(dat => dat.DevName == "Система создания вакуума 1 Вакууметр").Where(dat1 => dat1.DevNum == _clsPumpSystemStep.Num).ToList();

            if(DataList.Count<1)
                return;

            var readPress = DataList.Last().CurrPress;

            for (int i = 0; i < States.Count; i++)
            {
                var time = DataList.Last().TimeStep;


                if (readPress < States[i].ValueBegin || readPress > States[i].ValueEnd)
                {
                    if (startTimerVacuum == false)
                    {
                        timeErrorDataVacuum = DateTime.Now;
                        startTimerVacuum = true;
                    }
                }
                else
                {
                    startTimerVacuum = false;
                }


                if (((DateTime.Now - timeErrorDataVacuum).TotalSeconds >= States[i].Timer) && (readPress < States[i].ValueBegin || readPress > States[i].ValueEnd) )
                {
                    FlagStop = true;

                    ConJumpArgs conJumpArgs=new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev= _clsPumpSystemStep.Num;
                    conJumpArgs.NameDev = "Вакууметр";
                    conJumpArgs.CurrValue = Convert.ToDouble(readPress);
                    conJumpArgs.TextError = "Значение давления за границей допустимого диапазона";
                    conJumpArgs.Conditional = States[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError(this, conJumpArgs);
                }
            }

        }
    }
    
}
