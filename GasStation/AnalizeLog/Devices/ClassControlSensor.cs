using ChartApplication.points;
using GasStation.Controllers;
using GasStation.Controllers.DI;
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
    public class ClassControlSensor : ClassBaseDevices
    {
        /// <summary>
        /// Параметры шага вакууметра
        /// </summary>
        private XmlClassSensor _clsSensorStep => (XmlClassSensor)_clsDevStep;

        public ClassControlSensor(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        public void DataStep(XmlClassSensor classSensorStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = classSensorStep;
            _stepParams = stepParams;
            _states = states.ToArray();
            FlagStop = false;
            newStep = true;
            Init();

        }

        public override void Init()
        {           

        }


        public bool GetPortState()
        {
            var contr = LstContr[_clsSensorStep.SensorConst.ContrNum] as BaseDIController;
            return (contr.DioValues[_clsSensorStep.SensorConst.Port]);
        }


        protected override void NextStepFunc()
        {
            var sensorValue = GetPortState();
            var curr = new ClassDataSensor(_classDataTime, sensorValue);
            AddData(curr);
            
            _clsSensorStep.SensorView.SensorValue = sensorValue;
        }

        

        public void CheckStatus(ClassDataVacuumetr curr)
        {
        }
    }
}
