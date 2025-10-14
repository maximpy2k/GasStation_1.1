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
    public class ClassControlShutter : ClassBaseDevices
    {
        public ClassControlFlap[] Flaps;

        private XmlClassShutter _classShutterStep=>(XmlClassShutter)_clsDevStep;

        public ClassControlShutter(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        public void DataStep(XmlClassShutter classShutterStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = classShutterStep;
            _stepParams = stepParams;
            _states = states.ToArray();
            FlagStop = false;
            newStep = true;
            Init();

        }

        public override void Init()
        {
            
            Flaps = new ClassControlFlap[_classShutterStep.Flaps.Length];
            

                for (int i = 0; i < Flaps.Length; i++)
                {
                    Flaps[i] = new ClassControlFlap(_classDataTime);
                    Flaps[i].AlarmError = AlarmError;
                    Flaps[i].LstContr = LstContr;
                    Flaps[i].DataStep(_classShutterStep.Flaps[i], null,_stepParams);
                }
            

        }

        private bool? lastOpenShutter = null;
        private bool? lastCloseShutter = null;
        protected override void NextStepFunc()
        {
            for (int i = 0; i < Flaps.Length; i++)
                Flaps[i].NextStep(false);


            var curr = new ClassDataShutter(_classDataTime, GetPortGateOpen(), GetPortGateClose());
            curr.DataFlaps = new ClassDataFlap[Flaps.Length];
            for (int i = 0; i < curr.DataFlaps.Length; i++)
                curr.DataFlaps[i] = (ClassDataFlap)Flaps[i].LastData;
            
            AddData(curr);

            _classShutterStep.ShutterView.OpenShutterStatus = curr.OpenStatus;

            if (lastOpenShutter == null || lastOpenShutter != curr.OpenStatus)
            {
                lastOpenShutter = curr.OpenStatus;
                if(lastOpenShutter==true)
                    GenerateEvent(true,"Открыт",TypeConditional.StatusText);

            }
            _classShutterStep.ShutterView.CloseShutterStatus = curr.CloseStatus;
            if (lastCloseShutter == null || lastCloseShutter != curr.CloseStatus)
            {
                lastCloseShutter = curr.CloseStatus;
                if (lastCloseShutter == true)
                    GenerateEvent(true, "Закрыт", TypeConditional.StatusText);

            }
            CheckStatus(curr);
        }

        public bool GetPortGateOpen()
        {
            if (_classShutterStep.ShutterConst.StatusConst.GateOpen == null)
                return false;
            var contr = LstContr[_classShutterStep.ShutterConst.StatusConst.GateOpen.ContrNum] as ClassController87053;
            return (contr.DioValues[_classShutterStep.ShutterConst.StatusConst.GateOpen.Port]);
        }
        public bool GetPortGateClose()
        {
            if (_classShutterStep.ShutterConst.StatusConst.GateClosed == null)
                return _classShutterStep.ShutterConst.StatusConst.GateOpen==null?false:!GetPortGateOpen();

            var contr = LstContr[_classShutterStep.ShutterConst.StatusConst.GateClosed.ContrNum] as ClassController87053;
            return (contr.DioValues[_classShutterStep.ShutterConst.StatusConst.GateClosed.Port]);
        }

        public void CheckStatus(ClassDataShutter curr)
        {
            List<XmlStateConditionScript> StatesShutter = _states.Where(dat => dat.DevName.Contains("Затвор")).ToList();
            
            for (int i = 0; i < StatesShutter.Count; i++)
            {
                //var val = _classShutterStep.ShutterConst.StatusConst.GateClosed.IsInverted
                //    ? !curr.CloseStatus
                //    : curr.CloseStatus;
                if ((curr.TimeStep > StatesShutter[i].Timer) && (curr.CloseStatus != Convert.ToBoolean(StatesShutter[i].CurState))&& StatesShutter[i].DevName.Contains("закрыт"))
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _classShutterStep.Num;
                    conJumpArgs.NameDev = "Затвор закрыт";
                    conJumpArgs.CurrValue = Convert.ToInt32(curr.CloseStatus);
                    conJumpArgs.TextError = "";
                    conJumpArgs.Conditional = StatesShutter[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);
                }
            }
                       

            for (int i = 0; i < StatesShutter.Count; i++)
            {
                var val = _classShutterStep.ShutterConst.StatusConst.GateOpen.IsInverted
                    ? !curr.OpenStatus
                    : curr.OpenStatus;
                if ((_classDataTime.TimeStep > StatesShutter[i].Timer) && (curr.OpenStatus != Convert.ToBoolean(StatesShutter[i].CurState)) && StatesShutter[i].DevName.Contains("открыт"))
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
                    conJumpArgs.NumDev = _classShutterStep.Num;
                    conJumpArgs.NameDev = "Затвор открыт";
                    conJumpArgs.CurrValue = Convert.ToInt32(curr.OpenStatus);
                    conJumpArgs.TextError = "";
                    conJumpArgs.Conditional = StatesShutter[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);
                }
            }
        }
        public void GenerateEvent(bool state, string text, TypeConditional typeError)
        {
            ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
            conJumpArgs.NumDev = _classShutterStep.Num;
            conJumpArgs.NameDev = "Затвор";
            conJumpArgs.CurrValue = Convert.ToInt32(state);

            conJumpArgs.TextError = text;


            conJumpArgs.Conditional = 1;
            conJumpArgs.TypeConditional = typeError;
            AlarmError(this, conJumpArgs);
        }
    }
}
