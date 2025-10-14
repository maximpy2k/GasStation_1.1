using ChartApplication.points;
using GasStation.Controllers;
using GasStation.Elements.Data;
using GasStation.Status;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GasStation.Devices
{
    public class ClassControlVacuumetr : ClassBaseDevices
    {
        /// <summary>
        /// Параметры шага вакууметра
        /// </summary>
        private XmlClassVacuumetr _clsVacuumetrStep => (XmlClassVacuumetr)_clsDevStep;

        public ClassControlVacuumetr(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        public void DataStep(XmlClassVacuumetr classVacuumetrStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = classVacuumetrStep;
            _stepParams = stepParams;
            _states = states.ToArray();
            FlagStop = false;
            newStep = true;
            Init();

        }

        public override void Init()
        {           

        }


        /// <summary>
        /// Управление выходом вакуметра
        /// </summary>
        /// <param name="press">Показание вакуметра</param>
        public void SetVacuumetDoState(bool stateDo)
        {
            var con = _clsVacuumetrStep.VacuumetrConst;
            if (con.DioConst == null)
                return;
            var contr = LstContr[con.DioConst.ContrNum] as ClassController87057;

            contr.MasPortsState[con.DioConst.Port] = stateDo;
            _clsVacuumetrStep.VacuumetrView.PressDio = con.CheckCondition(Convert.ToInt32(stateDo));
        }

        /// <summary>
        /// Чтение датчика давления
        /// </summary>
        /// <returns>Значение прочитанное с АЦП</returns>
        public double GetPress()
        {
            var con = _clsVacuumetrStep.VacuumetrConst;
            var contr = LstContr[con.AcpConst.ContrNum] as ClassController87017;
            var u = contr.AcpValues[con.AcpConst.Port];
            return con.AcpConst.GetValue(u);
        }


        protected override void NextStepFunc()
        {
            var con = _clsVacuumetrStep.VacuumetrConst;
            var press = GetPress();

            var curr = new ClassDataVacuumetr(_classDataTime, press, con.CheckCondition(press));
            AddData(curr);
            SetVacuumetDoState(curr.StateDo);

            CheckStatus(curr);

            _clsVacuumetrStep.VacuumetrView.CurrentPress = curr.Press;
            _clsVacuumetrStep.VacuumetrView.SeriesReadPress.Add(new PointTime(curr.CurrDate, curr.Press));
        }

        

        public void CheckStatus(ClassDataVacuumetr curr)
        {
            //List<XmlStateConditionScript> CloseShutter = _states.Where(dat => dat.DevName == "Заслонка").ToList();
            
            //for (int i = 0; i < CloseShutter.Count; i++)
            //{
            //    if ((curr.TimeStep > CloseShutter[i].Timer) && (curr.CloseStatus != Convert.ToBoolean(CloseShutter[i].CurState)))
            //    {
            //        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
            //        conJumpArgs.NumDev = _classShutterStep.Num;
            //        conJumpArgs.NameDev = "Затвор закрыт";
            //        conJumpArgs.CurrValue = Convert.ToInt32(curr.CloseStatus);
            //        conJumpArgs.TextError = "";
            //        conJumpArgs.Conditional = CloseShutter[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
            //        StateError?.Invoke(this, conJumpArgs);
            //    }
            //}
                       

            //for (int i = 0; i < CloseShutter.Count; i++)
            //{
            //    if ((_classDataTime.TimeStep > CloseShutter[i].Timer) && (curr.OpenStatus != Convert.ToBoolean(CloseShutter[i].CurState)))
            //    {
            //        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
            //        conJumpArgs.NumDev = _classShutterStep.Num;
            //        conJumpArgs.NameDev = "Затвор открыт";
            //        conJumpArgs.CurrValue = Convert.ToInt32(curr.OpenStatus);
            //        conJumpArgs.TextError = "";
            //        conJumpArgs.Conditional = CloseShutter[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
            //        StateError?.Invoke(this, conJumpArgs);
            //    }
            //}
        }
    }
}
