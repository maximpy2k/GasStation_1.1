using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Controllers;
using GasStation.Controllers.DO;
using GasStation.Elements.Data;
using GasStation.Mathem.Chamber;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.xml.Script.XmlScript;

namespace GasStation.Devices
{
    public class ClassControlGenFrequency : ClassBaseDevices
    {
        public ClassControlGenFrequency(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        public override void Init()
        {
            FlagStop = false;
            base.Init();
        }

        /// <summary>
        /// Функция заполнения даннных скрипта для клапана на текущем шаге скрипта
        /// </summary>
        /// <param name="classFlapStep">Устанавливаемые значения дял клапана из скрипта</param>
        public void DataStep(XmlClassFreqGenerator classShimGenerator, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = classShimGenerator;
            FlagStop = false;
            newStep = true;
            _stepParams = stepParams;
            Init();
        }

        protected override void NextStepFunc()
        {
            var step = _clsDevStep as XmlClassFreqGenerator;
            SetImpuls(0, step.ImpulsDuration1, step.ImpulsDelay1);
            SetImpuls(1, step.ImpulsDuration2, step.ImpulsDelay2);
        }

        /// <summary>
        /// Установка параметров импульса
        /// </summary>
        /// <param name="num">Номер канала</param>
        /// <param name="impulsDuration">Длительность импульса</param>
        /// <param name="impulsDelay">Задержка между импульсами</param>
        public void SetImpuls(int num,  double impulsDuration, double impulsDelay)
        {
            var step = _clsDevStep as XmlClassFreqGenerator;
            var con= step.ShimGeneratorConst.ImpulsConsts[num];
            
            var contrImpDuration = LstContr[con.ImpulsDuration.ContrNum] as ClassControllerShim;
            var contrImpDelay = LstContr[con.ImpulsDelay.ContrNum] as ClassControllerShim;

            contrImpDuration.SetValue(con.ImpulsDuration.GetValue(impulsDuration));
            contrImpDelay.SetValue(con.ImpulsDuration.GetValue(impulsDelay));

            SetStartupSwitch(step.StartupSwitch);
        }

        /// <summary>
        /// Включение отключение генератора
        /// </summary>
        /// <param name="state">состояние генератора</param>
        public void SetStartupSwitch(bool state)
        {
            var step = _clsDevStep as XmlClassFreqGenerator;
            var ctrNum = step.ShimGeneratorConst.StartupSwitch.ContrNum;
            var port = step.ShimGeneratorConst.StartupSwitch.Port;

            var contr = LstContr[ctrNum] as ClassController87057;
            contr.MasPortsState[port] = state;
        }

    }
}
