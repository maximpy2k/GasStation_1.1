using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GasStation.Elements.Data;
using GasStation.Devices.EventDevParams;
using GasStation.Mathem.RRG;
using GasStation.ViewModels.Elements;
using GasStation.xml;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.Controllers;
using GasStation.Status;
using GasStation.xml.Script.XmlScript;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using ChartApplication.points;
using GasStation.Mathem.Pid;

namespace GasStation.Devices
{
    public class ClassControlRRG : ClassBaseDevices
    {

        /// <summary>
        /// Параметры шага РРГ
        /// </summary>
        private XmlClassRrg _clsRrgStep => (XmlClassRrg)_clsDevStep;

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
                _clsRrgPidRegulation = null;
                _usePid = value;
            }
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="classDataTime">Временная информация</param>
        public ClassControlRRG(ClassDataTime classDataTime) : base(classDataTime)
        {
            _classRrgSetRaise = new ClassRrgSetRaise(classDataTime);
        }

        /// <summary>
        /// Экземпляр класса вычисления текущего расчета
        /// </summary>
        private ClassRrgSetRaise _classRrgSetRaise;

        ClassRrgPidRegulation _clsRrgPidRegulation;
        /// <summary>
        /// Экземпляр класса ПИД регулятора
        /// </summary>
        private ClassRrgPidRegulation ClsRrgPidRegulation
        {
            get
            {
                if (_clsRrgPidRegulation != null)
                    return _clsRrgPidRegulation;

                if (_clsRrgStep.UsePid && _clsRrgStep.RrgConst.Pid != null && _clsRrgPidRegulation == null)
                {
                    var press = GetPress();
                    _clsRrgPidRegulation = new ClassRrgPidRegulation(_classDataTime);
                    _clsRrgPidRegulation.NewScriptStep(_clsRrgStep, _stepParams, press);
                }
                return _clsRrgPidRegulation;
            }
        }

        /// <summary>
        /// Инициализация нового шага скрипта
        /// </summary>
        /// <param name="clsRrgStep">Параметры шага РРГ</param>
        /// <param name="states">Данные по условным переходам</param>
        /// <param name="stepParams">Параметры шага скрипта</param>
        public void DataStep(XmlClassRrg clsRrgStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = clsRrgStep;
            startTimerVacuum = false;
            startTimerRrg = false;
            _states = states.ToArray();
            _stepParams = stepParams;
            newStep = true;

            if (UsePid != _clsRrgStep.UsePid)
                UsePid = _clsRrgStep.UsePid;

            _classRrgSetRaise.NewScriptStep(clsRrgStep, stepParams);

            Init();
        }

        /// <summary>
        /// Расчеты на первом шаге
        /// </summary>
        public override void Init()
        {
            FlagStop = false;
            //_classRrgSetRaise = null;
            base.Init();
        }

        /// <summary>
        /// Расчеты на каждом последующем шаге
        /// </summary>
        protected override void NextStepFunc()
        {
            if (UsePid != _clsRrgStep.UsePid)
                UsePid = _clsRrgStep.UsePid;

            var last = (ClassDataRRG)LastData;
            var curr = new ClassDataRRG(_classDataTime);

            #region Чтение АЦП расхода
            curr.Acp = GetAcp();
            curr.ReadRaise = _clsRrgStep.RrgConst.MasAcpConst[0].GetValue(curr.Acp);
            #endregion

            #region Чтение показаний вакуметра
            if (_clsRrgStep.RrgConst.VacuumetrConst != null)
            {
                curr.CurrPress = GetPress();
                SetVacuumetDoState(curr.CurrPress);
            }
            #endregion


            if (!_clsRrgStep.UsePid)
            {
                #region Работа без ПИД регулятора                
                curr.SetupValue = _clsRrgStep.SetupValue;
                curr.CurrSetRaise = _classRrgSetRaise.NextCycleStep();
                curr.TimeStep = _classRrgSetRaise.TimeStep;
                curr.Cap = _clsRrgStep.RrgConst.MasCapConst[0].GetValue(curr.CurrSetRaise);

                #endregion
            }
            else
            {
                #region Работа с ПИД регулятором                
                curr.SetupValue = ClsRrgPidRegulation.CurrSetupValue;
                curr.DataPid = ClsRrgPidRegulation.NextCycleStep(curr.CurrPress);
                curr.CurrSetRaise = ClsRrgPidRegulation.EvalfPower(curr.DataPid.DeltaValue);
                curr.TimeStep = _classRrgSetRaise.TimeStep;
                curr.Cap = _clsRrgStep.RrgConst.MasCapConst[0].GetValue(curr.CurrSetRaise);
                #endregion
            }


            #region Управление ЦАП
            SetCap(curr.Cap);
            #endregion

            #region Управление клапаном
            if (_clsRrgStep.RrgConst.IsFlap)
            {
                curr.StateRrg = _clsRrgStep.Flap.FlapState;
                SetPortState(_clsRrgStep.Flap.FlapState);
                #endregion

                if (last != null)
                {
                    if (curr.StateRrg != last.StateRrg)
                    {
                        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                        conJumpArgs.NumDev = _clsRrgStep.Num;
                        conJumpArgs.NameDev = "РРГ";
                        conJumpArgs.CurrValue = Convert.ToInt32(curr.StateRrg);
                        conJumpArgs.TextError = "Состояние клапана РРГ изменено";
                        conJumpArgs.Conditional = 1;
                        conJumpArgs.TypeConditional = TypeConditional.Text;
                        AlarmError(this, conJumpArgs);
                    }
                }
            }

            if (AddData(curr))
            {
                if (_clsRrgStep.RrgConst.VacuumetrConst != null)
                    _clsRrgStep.RrgView.VacuumetrView.SeriesReadPress.PointsPrepare.Clr(Cnt);

                _clsRrgStep.RrgView.SeriesSetStream.PointsPrepare.Clr(Cnt);
                _clsRrgStep.RrgView.SeriesReadStream.PointsPrepare.Clr(Cnt);
                _clsRrgStep.RrgView.SeriesCalcStream.PointsPrepare.Clr(Cnt);
            }

            CheckStatus(curr);

            #region Визуализация

            #region Если есть вакуметр
            if (_clsRrgStep.RrgConst.VacuumetrConst != null)
            {
                _clsRrgStep.RrgView.VacuumetrView.CurrentPress = curr.CurrPress;
                _clsRrgStep.RrgView.VacuumetrView.SeriesReadPress.Add(new PointTime(curr.CurrDate, curr.CurrPress));
            }
            #endregion

            _clsRrgStep.RrgView.CurrentValue = curr.ReadRaise;
            if (Math.Abs(100 - curr.ReadRaise * 100 / curr.CurrSetRaise) > _clsRrgStep.RrgConst.FluctRaise && curr.StateRrg == false&& curr.CurrSetRaise!=0&& curr.ReadRaise>0)
            {
                _clsRrgStep.RrgView.Avalible = false;

                ConJumpArgs conJumpArgs1 = new ConJumpArgs(curr.TimeStep);
                conJumpArgs1.NumDev = _clsRrgStep.Num;
                conJumpArgs1.NameDev = "РРГ";
                conJumpArgs1.CurrValue = Convert.ToInt32(curr.StateRrg);
                conJumpArgs1.TextError = $" Расход имеет отклонение: Заданный расход {curr.CurrSetRaise}    Считаный расход {curr.ReadRaise}";
                conJumpArgs1.Conditional = 1;
                conJumpArgs1.TypeConditional = TypeConditional.LogWrite;
                AlarmError(this, conJumpArgs1);

            }
            else
                _clsRrgStep.RrgView.Avalible = true;


            //curr.CalcRaise = classDataRrg.CurrSetRaise;
            _clsRrgStep.RrgView.SeriesSetStream.Add(new PointTime(curr.CurrDate, Math.Round(_clsRrgStep.SetupValue, 1)));
            _clsRrgStep.RrgView.SeriesReadStream.Add(new PointTime(curr.CurrDate, Math.Round(curr.ReadRaise, 1)));
            _clsRrgStep.RrgView.SeriesCalcStream.Add(new PointTime(curr.CurrDate, curr.CurrSetRaise));
            _clsRrgStep.RrgView.DataRrg = curr;
            #endregion


        }

        /// <summary>
        /// Установка напряжения на ЦАП
        /// </summary>
        /// <param name="u">напряжение на ЦАП</param>
        public void SetCap(double u)
        {            
            var contr = LstContr[_clsRrgStep.RrgConst.MasCapConst[0].ContrNum] as ClassController87024;
            contr.SetCap(u, _clsRrgStep.RrgConst.MasCapConst[0].Port);
            //contr.MasCap[_clsRrgStep.RrgConst.MasCapConst[0].Port] = DataList.Last().Cap;

        }

        /// <summary>
        /// Чтение АЦП
        /// </summary>
        /// <returns>Значение прочитанное с АЦП</returns>
        public double GetAcp()
        {
            var contr = LstContr[_clsRrgStep.RrgConst.MasAcpConst[0].ContrNum] as ClassController87017;
            return contr.AcpValues[_clsRrgStep.RrgConst.MasAcpConst[0].Port];
        }

        /// <summary>
        /// Управление выходом вакуметра
        /// </summary>
        /// <param name="press">Показание вакуметра</param>
        public void SetVacuumetDoState(double press)
        {
            var con = _clsRrgStep.RrgConst.VacuumetrConst;
            if (con.DioConst == null)
                return;
            var contr = LstContr[con.DioConst.ContrNum] as ClassController87057;
            contr.MasPortsState[con.DioConst.Port] = con.CheckCondition(press);
            _clsRrgStep.RrgView.VacuumetrView.PressDio = con.CheckCondition(press);
        }
        /// <summary>
        /// Чтение датчика давления
        /// </summary>
        /// <returns>Значение прочитанное с АЦП</returns>
        public double GetPress()
        {
            var con = _clsRrgStep.RrgConst.VacuumetrConst;
            var contr = LstContr[con.AcpConst.ContrNum] as ClassController87017;
            var u = contr.AcpValues[con.AcpConst.Port];
            return con.AcpConst.GetValue(u);
        }

        public void SetPortState(bool portState)
        {
            if (!_clsRrgStep.RrgConst.IsFlap)
                return;
            var constRrgFlap = _clsRrgStep.Flap;

            var contr = LstContr[_clsRrgStep.Flap.FlapConst.MasDioConst[0].ContrNum] as ClassController87057;
            contr.MasPortsState[_clsRrgStep.Flap.FlapConst.MasDioConst[0].Port] = portState;

        }

        public DateTime timeErrorDataVacuum=DateTime.Now;
        public bool startTimerVacuum = false;

        public DateTime timeErrorDataRrg = DateTime.Now;
        public bool startTimerRrg = false;
        public void CheckStatus(ClassDataRRG curr)
        {

            var StatesVacuum = _states.Where(dat => dat.DevName == $"РРГ {_clsRrgStep.Num} Вакууметр").ToList();
            for (int i = 0; i < StatesVacuum.Count; i++)
            {
                if (curr.CurrPress < StatesVacuum[i].ValueBegin || curr.CurrPress > StatesVacuum[i].ValueEnd)
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

                if (((DateTime.Now- timeErrorDataVacuum).TotalSeconds >= StatesVacuum[i].Timer)&& startTimerVacuum)
                {
                    Console.WriteLine($"Переход по значению вакуметра РРГ {curr.TimeStep}");
                    FlagStop = true;
                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _clsRrgStep.Num;
                    conJumpArgs.NameDev = $"РРГ {_clsRrgStep.Num} Вакууметр 1";
                    conJumpArgs.CurrValue = curr.CurrPress;
                    conJumpArgs.TextError = "Значение давления за границей допустимого диапазона";
                    conJumpArgs.Conditional = StatesVacuum[i].NumStep;

                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError(this, conJumpArgs);
                }
            }


            var States = _states.Where(dat => dat.DevName == "РРГ").Where(dat1 => dat1.DevNum == _clsRrgStep.Num).ToList();

            for (int i = 0; i < States.Count; i++)
            {
                if (curr.ReadRaise < States[i].ValueBegin || curr.ReadRaise > States[i].ValueEnd)
                {
                    if (startTimerRrg == false)
                    {
                        timeErrorDataRrg = DateTime.Now;
                        startTimerRrg = true;
                    }
                }
                else
                {
                    startTimerRrg = false;
                }


                if ((DateTime.Now - timeErrorDataRrg).TotalSeconds >= States[i].Timer && (curr.ReadRaise < States[i].ValueBegin || curr.ReadRaise > States[i].ValueEnd))
                {
                    Console.WriteLine($"Переход по расходу РРГ {curr.TimeStep}");
                    FlagStop = true;                    
                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _clsRrgStep.Num;
                    conJumpArgs.NameDev = "РРГ";
                    conJumpArgs.CurrValue = curr.ReadRaise;
                    conJumpArgs.TextError = "Значение расхода за границей допустимого диапазона";
                    conJumpArgs.Conditional = States[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError(this, conJumpArgs);
                }
            }



        }
    }

}
