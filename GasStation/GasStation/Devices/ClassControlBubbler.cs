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
    public class ClassControlBubbler : ClassBaseDevices
    {
        /// <summary>
        /// Класс параметров  шага устройства
        /// </summary>
        XmlClassBubbler _clsBublerStep => (XmlClassBubbler)_clsDevStep;
        
        public ClassControlBubbler(ClassDataTime classDataTime) : base(classDataTime)
        {
        }  

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
                _currTd = new ClassBaseTd(_clsBublerStep.BubblerConst.Td);
                return _currTd;
            }
        }
        public void DataStep(XmlClassBubbler clsBublerStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = clsBublerStep;
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
            CurrTd.Add(GetAcp());

            var last = (ClassDataBubler)LastData;
            var curr = new ClassDataBubler(_classDataTime);

            curr.CurrTd = CurrTd.AverTd;
            curr.UseBubbler = _clsBublerStep.UseBubbler;

            #region Определение вкл/выкл нагрева
            if (curr.CurrTd > _clsBublerStep.SetupValue && _clsBublerStep.UseBubbler)
                curr.Relay = true;
            else
                curr.Relay = false;
            #endregion

            SetPortState(_clsBublerStep.BubblerView.Relay);

            ClassDataBubler classData;

            var dateNow = DateTime.Now;
            //var useBubler = _clsBublerStep.UseBubbler;
            //SetPortState(_clsBublerStep.UseBubbler);

            //var relay= true;
            //SetPortState(true);

            if (last != null)
            {
                if (curr.UseBubbler != last.UseBubbler)
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _clsBublerStep.Num;
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
                _clsBublerStep.BubblerView.SeriesReadTd.PointsPrepare.Clr(Cnt);
            }

            _clsBublerStep.BubblerView.SeriesReadTd.Add(new PointTime(_classDataTime.BeginCycleStep, curr.CurrTd));

            _clsBublerStep.BubblerView.CurrentTd = curr.CurrTd;
            _clsBublerStep.BubblerView.Relay = curr.Relay;





            //SetPortState(_clsBublerStep.BubblerView.Relay);

            //if (_clsBublerStep.BubblerConst.Td != null)
            //{
            //    var tdU = GetAcp();
            //    CurrTd.Add(tdU);

            //    classData = new ClassDataBubler(_classDataTime, true, CurrTd.AverTd);
            //    //classData = new ClassDataBubler(_classDataTime, relay, CurrTd.AverTd);
            //    _clsBublerStep.BubblerView.CurrentTd = classData.CurrTd;

            //    _clsBublerStep.BubblerView.SeriesReadTd.Add(new PointTime(dateNow, classData.CurrTd));

            //    if (AddData(classData))
            //    {
            //        _clsBublerStep.BubblerView.SeriesReadTd.PointsPrepare.Clr(Cnt);
            //    }

            //    _clsBublerStep.BubblerView.Relay = true;
            //}
        }
                
        /// <summary>
        /// Чтение АЦП
        /// </summary>
        /// <returns>Значение прочитанное с АЦП</returns>
        public double GetAcp()
        {
            var contr7018 = LstContr[_clsBublerStep.BubblerConst.Td.MasAcpConst[0].ContrNum] as ClassController7018;
            if(contr7018!=null)
                return contr7018.AcpValues[_clsBublerStep.BubblerConst.Td.MasAcpConst[0].Port];

            var contr87017 = LstContr[_clsBublerStep.BubblerConst.Td.MasAcpConst[0].ContrNum] as ClassController87017;
            if(contr87017!=null)
                return contr87017.AcpValues[_clsBublerStep.BubblerConst.Td.MasAcpConst[0].Port];

            return 0.0;
        }

        public void SetPortState(bool portState)
        {
            var constBubler = _clsBublerStep.BubblerConst;
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
