using System.Collections.Generic;
using System.Linq;
using GasStation.Elements.Data;
using GasStation.ViewModels.Elements;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.Controllers;
using GasStation.Status;
using GasStation.xml.Script;
using System;
using GasStation.xml.Script.EnumConst;

namespace GasStation.Devices
{
    public class ClassControlFlap: ClassBaseDevices
    {        
        private bool newStep=false;

        /// <summary>
        /// Задание для текущего шага
        /// </summary>
        private XmlClassFlap _classFlapStep=>(XmlClassFlap)_clsDevStep;

        /// <summary>
        /// Конструктор класса для управления одним клапаном
        /// </summary>        
        public ClassControlFlap(ClassDataTime classDataTime) : base(classDataTime)
        {
            Cnt = 2;        
        }

        /// <summary>
        /// Функция заполнения даннных скрипта для клапана на текущем шаге скрипта
        /// </summary>
        /// <param name="classFlapStep">Устанавливаемые значения дял клапана из скрипта</param>
        public void DataStep(XmlClassFlap classFlapStep, IEnumerable<XmlStateConditionScript> states, XmlClassStepParams stepParams)
        {
            _clsDevStep = classFlapStep;
            FlagStop = false;
            newStep = true;
            _stepParams = stepParams;
            Init();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void NextStepFunc()
        {
            SetPortState(_classFlapStep.FlapState);

            if (_classFlapStep.Num == 3)
            {
                Console.WriteLine(_classFlapStep.FlapState);
            }
            var last = LastData != null ? (ClassDataFlap)LastData : null;

            var curr = new ClassDataFlap(_classDataTime,_classFlapStep.FlapState );
            AddData(curr);            

            if (last!=null)
            {
                if (newStep)
                {
                    newStep = false;
                    return;
                }
                if(curr.StateFlap!=last.StateFlap)
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _classFlapStep.Num;
                    conJumpArgs.NameDev = "Клапан";
                    conJumpArgs.CurrValue = Convert.ToInt32(curr.StateFlap);
                    conJumpArgs.TextError = "Состояние клапана изменено";
                    conJumpArgs.Conditional = 1;
                    conJumpArgs.TypeConditional = TypeConditional.Text;
                    AlarmError(this, conJumpArgs);
                }
            }
        }

        private bool _portState;
        public void SetPortState(bool portState)
        {
            var contr = LstContr[_classFlapStep.FlapConst.MasDioConst[0].ContrNum] as ClassController87057;
            contr.MasPortsState[_classFlapStep.FlapConst.MasDioConst[0].Port] = portState;
            _portState=portState;
        }

        public void CheckStatus()
        {

            var States = _states.Where(dat => dat.DevName == "Клапан").Where(dat1 => dat1.DevNum == _classFlapStep.Num).ToList();

            for (int i = 0; i < States.Count; i++)
            {
                if ((_classDataTime.TimeStep > States[i].Timer) && (Convert.ToInt32(_portState) != States[i].CurState))
                {
                    FlagStop = true;

                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _classFlapStep.Num;
                    conJumpArgs.NameDev = "Клапан";
                    conJumpArgs.CurrValue = Convert.ToInt32(_portState);
                    conJumpArgs.TextError = "Состояние клапана не верное";
                    conJumpArgs.Conditional = States[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError(this, conJumpArgs);
                }
            }

        }
    }
}
