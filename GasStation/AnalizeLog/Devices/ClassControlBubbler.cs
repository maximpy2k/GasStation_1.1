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
            ClassDataBubler classData;

            var dateNow = DateTime.Now;
            var useBubler = _clsBublerStep.UseBubbler;
            SetPortState(_clsBublerStep.UseBubbler);


            if (_clsBublerStep.BubblerConst.Td != null)
            {
                var tdU = GetAcp();
                CurrTd.Add(tdU);

                classData = new ClassDataBubler(_classDataTime, useBubler, CurrTd.AverTd);
                _clsBublerStep.BubblerView.CurrentTd = classData.CurrTd;

                _clsBublerStep.BubblerView.SeriesReadTd.Add(new PointTime(dateNow, classData.CurrTd));

                if (AddData(classData))
                {
                    _clsBublerStep.BubblerView.SeriesReadTd.PointsPrepare.Clr(Cnt);
                }                    
            }
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
            
            //var States = _states.Where(dat => dat.DevName == "РРГ").Where(dat1 => dat1.DevNum == _clsRrgStep.Num).ToList();

            //var readRaise = DataList.Last().ReadRaise;

            //for (int i = 0; i < States.Count; i++)
            //{
            //    var time = DataList.Last().Time;

            //    if ((time > States[i].Timer) && (readRaise < States[i].ValueBegin || readRaise > States[i].ValueEnd) )
            //    {
            //        FlagStop = true;

            //        ConJumpArgs conJumpArgs=new ConJumpArgs();
            //        conJumpArgs.NumDev= _clsRrgStep.Num;
            //        conJumpArgs.NameDev = "РРГ";
            //        conJumpArgs.CurrValue = readRaise;
            //        conJumpArgs.TextError = "Значение расхода за границей допустимого диапазона";
            //        conJumpArgs.Conditional = States[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
            //        StateError(this, conJumpArgs);
            //    }
            //}

        }



    }
    
}
