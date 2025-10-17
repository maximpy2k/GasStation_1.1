using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Controllers;
using GasStation.Elements.Data;
using GasStation.Mathem.Chamber;
using GasStation.Status;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript;
using GasStation.xml.Script.EnumConst;

namespace GasStation.Devices
{
    public class ClassControlLoader : ClassBaseDevices
    {
        /// <summary>
        /// Класс параметров  шага устройства
        /// </summary>
        private XmlClassLoader _clsLoaderStep=>(XmlClassLoader)_clsDevStep;


        public ClassControlLoader(ClassDataTime classDataTime):base(classDataTime)
        {
        }
        
        
        public void DataStep(XmlClassLoader clsLoaderStep, IEnumerable<XmlStateConditionScript> states)
        {
            Init();
            _clsDevStep = clsLoaderStep;
            _states = states.ToArray();
        }

        public override void Init()
        {
            FlagStop = false;
            base.Init();
        }

        bool ErrorLoad ;
        bool ErrorUnload ;
        bool LoadComplete;
        bool UnloadComplete;
        bool RegWork;
        private bool gateOpen;
        private bool gateClose;
        private bool? lastload = null;
        private bool? lastunload = null;

        private bool? lastErrorload = null;
        private bool? lastErrorunload = null;

        protected override void NextStepFunc()
        {
            RegWork = GetPortRegWork();
            _clsLoaderStep.LoaderView.RegWork = RegWork;

            ErrorLoad =  GetPortLoadError();
            ErrorUnload =  GetPortUnloadError();
            

            LoadComplete = GetPortLoadComplete();
            UnloadComplete = GetPortUnloadComplete();


            gateOpen = GetPortGateOpen();
            _clsLoaderStep.LoaderView.IsGateOpen = gateOpen;

            gateClose = GetPortGateClose();            
            _clsLoaderStep.LoaderView.IsGateClose = gateClose;

            
            CheckStatus();

            if (!ErrorLoad || _clsLoaderStep.LoaderConst.StatusConst.ErrorLoad == null)
            {
                var value = (_clsLoaderStep.LoaderConst.MasCapConst == null) ? 0 : _clsLoaderStep.LoaderConst.MasCapConst[0].Table.MaxVal;
                if(!ErrorUnload|| _clsLoaderStep.LoaderConst.StatusConst.ErrorUnLoad == null)
                    SetCap(value * _clsLoaderStep.SetupSpeed / 100);
            }

            if (!LoadComplete && !ErrorLoad)
            {
                _clsLoaderStep.LoaderView.StatusLoad = null;
                SetPortLoad(_clsLoaderStep.DestLoad);
                lastload = LoadComplete;
                lastErrorload = ErrorLoad;
            }
            else
            {
                if (!(bool)_clsLoaderStep.DestUnload)
                    //_clsLoaderStep.Destination = null;

                if (LoadComplete)
                {
                    _clsLoaderStep.LoaderView.StatusLoad = LoadComplete;
                    SetPortLoad(_clsLoaderStep.DestLoad);
                    if (lastload!=LoadComplete)
                    {
                        lastload = LoadComplete;
                        GenerateEvent(LoadComplete, "Загружен",TypeConditional.StatusText);

                    }
                    
                }


                if (ErrorLoad)
                {
                    _clsLoaderStep.LoaderView.StatusLoad = !ErrorLoad;
                    SetPortLoad(_clsLoaderStep.DestLoad);

                    if (lastErrorload != ErrorLoad)
                    {
                        lastErrorload = ErrorLoad;
                        GenerateEvent(ErrorLoad, "Авария загрузки", TypeConditional.Alarm);
                    }
                }

            }


            if (!UnloadComplete && !ErrorUnload)
            {
                SetPortUnload(_clsLoaderStep.DestUnload);
                _clsLoaderStep.LoaderView.StatusUnload = null;
                lastunload = UnloadComplete;
                lastErrorunload = ErrorUnload;
            }
            else
            {
                if (!(bool)_clsLoaderStep.DestLoad)
                    //_clsLoaderStep.Destination = null;

                if (UnloadComplete)
                {
                    _clsLoaderStep.LoaderView.StatusUnload = UnloadComplete;
                    SetPortUnload(_clsLoaderStep.DestUnload);

                    if (lastunload != UnloadComplete)
                    {
                        lastunload = UnloadComplete;
                        GenerateEvent(UnloadComplete,"Выгружен",TypeConditional.StatusText);
                    }
                }

                if (ErrorUnload)
                {
                    _clsLoaderStep.LoaderView.StatusUnload = !ErrorUnload;
                    SetPortUnload(_clsLoaderStep.DestUnload);

                    if (lastErrorunload != ErrorUnload)
                    {
                        lastErrorunload = ErrorUnload;
                        GenerateEvent(ErrorUnload, "Авария выгрузки", TypeConditional.Alarm);
                    }
                }
            }
        }

        public void SetCap(double u)
        {
            if (_clsLoaderStep.LoaderConst.MasCapConst != null)
            {
                var contr = LstContr[_clsLoaderStep.LoaderConst.MasCapConst[0].ContrNum] as ClassController87024;
                contr.SetCap(u, _clsLoaderStep.LoaderConst.MasCapConst[0].Port);
            }
        }

        #region Чтение портов (проверка на статусы)
        public bool GetPortLoadComplete()
        {
            if (_clsLoaderStep.LoaderConst.StatusConst.LoadComplete == null)
                return false;
            var contr = LstContr[_clsLoaderStep.LoaderConst.StatusConst.LoadComplete.ContrNum] as ClassController87053;
            return (contr.DioValues[_clsLoaderStep.LoaderConst.StatusConst.LoadComplete.Port]);
        }
        public bool GetPortUnloadComplete()
        {
            if (_clsLoaderStep.LoaderConst.StatusConst.UnLoadComplete == null)
                return false;
            var contr = LstContr[_clsLoaderStep.LoaderConst.StatusConst.UnLoadComplete.ContrNum] as ClassController87053;
            return (contr.DioValues[_clsLoaderStep.LoaderConst.StatusConst.UnLoadComplete.Port]);
        }
        public bool GetPortLoadError()
        {
            if (_clsLoaderStep.LoaderConst.StatusConst.ErrorLoad == null)
                return false;
            var contr = LstContr[_clsLoaderStep.LoaderConst.StatusConst.ErrorLoad.ContrNum] as ClassController87053;
            return (contr.DioValues[_clsLoaderStep.LoaderConst.StatusConst.ErrorLoad.Port]);
        }
        public bool GetPortUnloadError()
        {
            if (_clsLoaderStep.LoaderConst.StatusConst.ErrorUnLoad == null)
                return  false;
            var contr = LstContr[_clsLoaderStep.LoaderConst.StatusConst.ErrorUnLoad.ContrNum] as ClassController87053;
            return (contr.DioValues[_clsLoaderStep.LoaderConst.StatusConst.ErrorUnLoad.Port]);
        }
        public bool GetPortGateOpen()
        {
            if (_clsLoaderStep.LoaderConst.StatusConst.DumperOpen == null)
                return false;
            var contr = LstContr[_clsLoaderStep.LoaderConst.StatusConst.DumperOpen.ContrNum] as ClassController87053;
            var val = contr.DioValues[_clsLoaderStep.LoaderConst.StatusConst.DumperOpen.Port];
                
            return val;
        }
        public bool GetPortGateClose()
        {
            if (_clsLoaderStep.LoaderConst.StatusConst.DumperClosed == null)
                return false;
            var contr = LstContr[_clsLoaderStep.LoaderConst.StatusConst.DumperClosed.ContrNum] as ClassController87053;
            var val = contr.DioValues[_clsLoaderStep.LoaderConst.StatusConst.DumperClosed.Port];
              
            return val;
        }

        public bool GetPortRegWork()
        {
            if (_clsLoaderStep.LoaderConst.StatusConst.RegWork == null)
                return true;
            var contr = LstContr[_clsLoaderStep.LoaderConst.StatusConst.RegWork.ContrNum] as ClassController87053;
            var val = contr.DioValues[_clsLoaderStep.LoaderConst.StatusConst.RegWork.Port];

            return val;
        }
        #endregion

        public void SetPortLoad(bool? portState)
        {
            if (_clsLoaderStep.LoaderConst.Load == null)
                return;
            var contr = LstContr[_clsLoaderStep.LoaderConst.Load.ContrNum] as ClassController87057;

            if (portState!=null)
                contr.MasPortsState[_clsLoaderStep.LoaderConst.Load.Port] = (bool)portState;
        }

        public void SetPortUnload(bool? portState)
        {
            if (_clsLoaderStep.LoaderConst.UnLoad == null)
                return;
            var contr = LstContr[_clsLoaderStep.LoaderConst.UnLoad.ContrNum] as ClassController87057;
            if (portState != null)
                contr.MasPortsState[_clsLoaderStep.LoaderConst.UnLoad.Port] = (bool)portState;
        }

        public void CheckStatus()
        {
            #region Проверка загрузки

            List<XmlStateConditionScript> statesDLoad = _states.Where(dat => dat.DevName.Contains("Загружен")).ToList();

            for (int i = 0; i < statesDLoad.Count; i++)
            {
                var curValDLoad = Convert.ToInt32(LoadComplete);

                if ((_classDataTime.TimeStep > statesDLoad[i].Timer) && (curValDLoad != statesDLoad[i].CurState))
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _clsLoaderStep.Num;
                    conJumpArgs.NameDev = "Загрузчик";
                    conJumpArgs.CurrValue = curValDLoad;
                    conJumpArgs.TextError = "Загрузка не завершена";
                    conJumpArgs.Conditional = statesDLoad[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);
                }



            }
            #endregion

            #region Проверка Выгрузки

            List<XmlStateConditionScript> statesDUnload = _states.Where(dat => dat.DevName.Contains("Выгружен")).ToList();

            for (int i = 0; i < statesDUnload.Count; i++)
            {
                var curValDUnload = Convert.ToInt32(UnloadComplete);
                if ((_classDataTime.TimeStep > statesDUnload[i].Timer) && (curValDUnload != statesDUnload[i].CurState))
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _clsLoaderStep.Num;
                    conJumpArgs.NameDev = "Загрузчик";
                    conJumpArgs.CurrValue = curValDUnload;
                    conJumpArgs.TextError = "Выгрузка не завершена";
                    conJumpArgs.Conditional = statesDUnload[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);

                }
            }
            #endregion

            #region Проверка Заслонки Открыта

            List<XmlStateConditionScript> statesDGateOpen = _states.Where(dat => dat.Device == "Заслонка открыта").ToList();

            for (int i = 0; i < statesDGateOpen.Count; i++)
            {
                var curValDGateOpen = Convert.ToInt32(GetPortGateOpen());
                if ((_classDataTime.TimeStep > statesDGateOpen[i].Timer) && (curValDGateOpen != statesDGateOpen[i].CurState))
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _clsLoaderStep.Num;
                    conJumpArgs.NameDev = "Загрузчик";
                    conJumpArgs.CurrValue = curValDGateOpen;
                    conJumpArgs.TextError = "Заслонка не открыта";
                    conJumpArgs.Conditional = statesDGateOpen[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);

                }
            }
            #endregion

            #region Проверка Заслонки Закрыта

            List<XmlStateConditionScript> statesDGateClose = _states.Where(dat => dat.Device == "Заслонка закрыта").ToList();

            for (int i = 0; i < statesDGateClose.Count; i++)
            {
                var curValDGateClose = Convert.ToInt32(GetPortGateClose());
                if ((_classDataTime.TimeStep > statesDGateClose[i].Timer) && (curValDGateClose != statesDGateClose[i].CurState))
                {
                    ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                    conJumpArgs.NumDev = _clsLoaderStep.Num;
                    conJumpArgs.NameDev = "Загрузчик";
                    conJumpArgs.CurrValue = curValDGateClose;
                    conJumpArgs.TextError = "Заслонка не закрыта";
                    conJumpArgs.Conditional = statesDGateClose[i].NumStep;
                    conJumpArgs.TypeConditional = TypeConditional.Error;
                    StateError?.Invoke(this, conJumpArgs);

                }
            }
            #endregion


        }

        public void GenerateEvent(bool state,string text, TypeConditional typeError)
        {
                ConJumpArgs conJumpArgs = new ConJumpArgs(_classDataTime.TimeStep);
                conJumpArgs.NumDev = _clsLoaderStep.Num;
                conJumpArgs.NameDev = "Загрузчик";
                conJumpArgs.CurrValue = Convert.ToInt32(state);
                
                conJumpArgs.TextError = text;


                conJumpArgs.Conditional = 1;
                conJumpArgs.TypeConditional = typeError;
                AlarmError(this, conJumpArgs);
        }
    }
}
