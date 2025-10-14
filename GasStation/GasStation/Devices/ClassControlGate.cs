using GasStation.Controllers;
using GasStation.Elements.Data;
using GasStation.xml;
using GasStation.xml.Constant;
using GasStation.xml.Script;
using GasStation.xml.Script.Constant;
using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Status;
using GasStation.xml.Script.EnumConst;

namespace GasStation.Devices
{
    public class ClassControlGate: ClassBaseDevices
    {
        public ClassControlGate(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        private XmlClassGate _clsGateStep => (XmlClassGate)_clsDevStep;
        public void DataStep(XmlClassGate clsGateStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = clsGateStep;
            _stepParams = stepParams;
            FlagStop = false;
            newStep = true;
            _states = states.ToArray();
            
            Init();

        }
        public override void Init()
        {

        }
        private bool currStateOpen;
        private bool currStateClose;
        protected override void NextStepFunc()
        {
            _clsGateStep.GateView.OpenGateStatus = GetPortGateStatus(_clsGateStep.GateConst.DioGateOpenConst);
            _clsGateStep.GateView.CloseGateStatus = GetPortGateStatus(_clsGateStep.GateConst.DioGateCloseConst);
            if (_clsGateStep.GateOpen != currStateOpen)
            {
                SetPortGate(_clsGateStep.GateOpen, _clsGateStep.GateConst.OpenGate);
                currStateOpen = (bool)_clsGateStep.GateOpen;
            }
            if (_clsGateStep.GateClose != currStateClose)
            {
                SetPortGate(_clsGateStep.GateClose, _clsGateStep.GateConst.CloseGate);
                currStateClose = (bool)_clsGateStep.GateClose;
            }

            var curr = new ClassDataGate(_classDataTime, _clsGateStep);
            AddData(curr);
        }

        public void SetPortGate(bool? portState, XmlClassDioPortConst xmlClassDioPortConst)
        {
            if (_clsGateStep.GateConst.OpenGate == null)
                return;
            var contr = LstContr[xmlClassDioPortConst.ContrNum] as ClassController87057;
            if (portState != null)
                contr.MasPortsState[xmlClassDioPortConst.Port] = (bool)portState;
        }

        public bool GetPortGateStatus(XmlClassSensorConst xmlClassSensorConst)
        {
            if (_clsGateStep.GateConst.DioGateOpenConst == null)
                return false;
            var contr = LstContr[xmlClassSensorConst.ContrNum] as ClassController87053;
            return (contr.DioValues[xmlClassSensorConst.Port]);
        }






    }
}
