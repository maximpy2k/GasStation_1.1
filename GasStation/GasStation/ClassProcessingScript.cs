using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using GasStation.Devices;
using GasStation.xml;
using GasStation.Elements.Data;
using GasStation.xml.Script.EnumConst;
using GasStation.Controllers;
using GasStation.Status;
using GasStation.xml.Script.XmlScript;
using System.Speech.Synthesis;
using System.Windows.Forms;
using GasStation.ModalWindows.View;
using GasStation.xml.Script;
using MessageBox = System.Windows.MessageBox;
using GasStation.Elements.ViewModels;
using System.IO;
using GasStation.Controllers.DO;

namespace GasStation
{
    public class ClassProcessingScript
    {

        SpeechSynthesizer Voise = new SpeechSynthesizer();
        /// <summary>
        /// Параметы Rs485
        /// </summary>
        public ClassSerialParams SerialParams;
    
        /// <summary>
        /// Класс скрипта
        /// </summary>
        private ClassScript _script;
        

        /// <summary>
        /// Событие обработки условного перехода
        /// </summary>
        public EventHandler StateError;

        /// <summary>
        /// Событие обработки тревоги
        /// </summary>
        public EventHandler AlarmError;


        private bool ErrorState;
        private bool fatalError=false;
        private ConJumpArgs conJumpArgs;


        private readonly ClassDevices _classDevices;
        /// <summary>
        /// Список для классов устройств
        /// </summary>
        private List<ClassBaseDevices> ListBaseDeviceses
        {
            get { return _classDevices.Base; }
        }

        /// <summary>
        /// Список контроллеров
        /// </summary>
        List<BaseClassController> listControllers;

        public ClassProcessingScript(ClassScript clsScript, ClassSerialParams serialParams)
        {
           _script = clsScript;
            _script.RefreshData();
            _script.CurrentStepIdx = 0;

            SerialParams = serialParams;
            _script.Consts.ChannelConsts.FindStartupLogDir();
            InitControllers(clsScript.ViewData.ViewControllers);

            _classDevices = new ClassDevices(listControllers, clsScript);
            Voise = new SpeechSynthesizer();

            StateError = _classDevices.CheckState;            
            _classDevices.StateError += CheckState;
            _classDevices.AlarmError += AlarmState;


        }

        public bool AbortScript;

        AutoResetEvent waitStart = new AutoResetEvent(false);

        private Thread t;
        /// <summary>
        /// Функция запуска среды для выполнения скрипта
        /// </summary>
        public void Start()
        {
            //CheckDevices();            

            t = new Thread(ProcessingScript);
            t.Start();

            waitStart.WaitOne();
        }

        /// <summary>
        /// Проверка доступности устройств
        /// </summary>
        /// <returns></returns>
        private bool CheckDevices()
        {
            if (SerialParams.SP.IsOpen)
                return false;

            try
            {
                SerialParams.SP.Open();
                SerialParams.SP.Close();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        AutoResetEvent StopEvent=new AutoResetEvent(false);
        public void Stop()
        {
            _script.IsViewScriptStop = false;

            if (WaitPause != null)
                WaitPause.Set();

            AbortScript = true;
            _script.IsViewScriptStart = true;
            _script.IsViewScriptStop = false;

            //ActProcessingScript.EndInvoke(resultProcScript);
        }

        AutoResetEvent WaitPause;
        public void Pause()
        {
            if(WaitPause!=null)
            {
                WaitPause.Set();
                return;
            }
            WaitPause = new AutoResetEvent(false);
        }

        /// <summary>
        /// Функция перемещения между строками скрипта
        /// </summary>
        private void ProcessingScript()
        {
            if (_script.ViewData.MessageOut != "")
            {
                var log = new ClassDataMainLog($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}  ", _script.ViewData.MessageOut.Remove(_script.ViewData.MessageOut.Length-($"{Environment.NewLine}").Length, ($"{Environment.NewLine}").Length));
                log.AppendToFile($"{_script.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");
            }
            SerialParams.Connect();
            if (SerialParams.SP.IsOpen==false)
            {
                ConJumpArgs ComArgs = new ConJumpArgs(0.0);
                ComArgs.NumDev = 0;
                ComArgs.NameDev = $"{SerialParams.ComPort}";
                ComArgs.CurrValue = 0;
                ComArgs.TextError = "Невозможно открыть";
                ComArgs.Conditional = 0;
                ComArgs.TypeConditional = TypeConditional.Error;
                AlarmState(this, ComArgs);
                waitStart.Set();
                return;
            }
            #region Начало работы скрипта
            _script.IsViewScriptStart = false;
            _script.IsViewScriptStop = true;
            _script.EnableStartTechProcess = true;            
            waitStart.Set();
            #endregion

            _script.ClsDataTime.StartScript();

            while (!AbortScript)
            {
                if (_script.CurrentStepIdx >= _script.Steps.Count)
                    _script.CurrentStepIdx = 0;

                if (ErrorState == false && _script.Steps[_script.CurrentStepIdx].StepParams.TypeStep == RegimsStep.Emergency)
                {
                    _script.CurrentStepIdx++;
                    ErrorState = false;
                    continue;
                }
                _script.GoToStep(_script.CurrentStepIdx);
               
                FlagAbort = false;
               
                ProcessStepNew(_script.Steps[_script.CurrentStepIdx]);

                if (ErrorState == false)
                {
                    _script.CurrentStepIdx++;
                }
                else
                {
                    _script.CurrentStepIdx = conJumpArgs.Conditional-1;
                    if (conJumpArgs.Conditional == 0)
                        conJumpArgs.Conditional = 1;

                    if (_script.Steps[_script.CurrentStepIdx].StepParams.TypeStep!=RegimsStep.Emergency)
                        ErrorState = false;
                }
                if (_script.CurrentStepIdx == _script.Steps.Select(dat => dat.StepParams.NumStep).Max())
                    _script.CurrentStepIdx = _script.Steps.Select(dat => dat.StepParams.NumStep).Min()-1;
            }
            SerialParams.Disconnect();


            if (fatalError)
            {
                MessageBox.Show("Технология закончена из за фатальной ошибки");
                fatalError = false;
                return;
            }
            MessageBox.Show("Управление установкой закончено");
            
            EndThread(null);
        }

        //WindowEndTechnology windowEndTechnology = new WindowEndTechnology();

        /// <summary>
        /// Функция остановки выполнения скрипта
        /// </summary>
        /// <param name="result"></param>
        private void EndThread(IAsyncResult result)
        {
            _script.IsViewScriptStart = true;
           // _script.IsViewScriptStart = false;
        }

        /// <summary>
        /// Флаг остановки режима
        /// </summary>
        public bool FlagAbort;
        

        /// <summary>
        /// Инициализация контроллеров
        /// </summary>
        private void InitControllers(BaseClassViewControllers[] viewControllers)
        {
            listControllers = new List<BaseClassController>();
            BaseClassController controller;
            for (int i = 0; i < _script.Consts.ConstControllers.Length; i++)
            {
                switch (_script.Consts.ConstControllers[i].NameController)
                {
                    case "IDAS 7018":
                        controller = new ClassController7018(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP,viewControllers[i]);
                        break;
                    case "TM 7042":
                    case "TM 7042P":
                        controller = new ClassController7042(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP,viewControllers[i]);
                        break;
                    case "TM 7041":
                        controller = new ClassController7041(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP,viewControllers[i]);
                        break;
                    case "IDAS 87017":
                        controller = new ClassController87017(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP, viewControllers[i]);
                        break;
                    case "IDAS 87024":
                        controller = new ClassController87024(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP, viewControllers[i]);
                        break;                    
                    case "IDAS 87057":
                        controller = new ClassController87057(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP, viewControllers[i]);
                        break;
                    case "IDAS 87053":
                        controller = new ClassController87053(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP, viewControllers[i]);
                        break;
                    case "TM SHIM":
                        controller = new ClassControllerShim(_script.Consts.ChannelConsts.LogPathFull, _script.Consts.ConstControllers[i], SerialParams.SP, viewControllers[i]);
                        break;
                    default:
                        MessageBox.Show("Нет выбранных контроллеров");
                        return;
                }
                controller.AlarmError += AlarmState;
                listControllers.Add(controller);
            }
        }

        /// <summary>
        /// Основной цикл работы
        /// </summary>
        public void ProcessStepNew(XmlClassStep classScriptStep)
        {
            Console.WriteLine("NextStep");

            var aval = true;
            //foreach (var contr in listControllers)
            //{
            //    
            //}

            foreach (var contr in listControllers)
                contr.ResetWdt();

            foreach (var contr in listControllers)
            {
               // contr.CheckAvalible();
                contr.PrevExecute();
            }
            
            //_classDevices.StepScript(_script.Steps[_script.CurrentStepIdx]);
            _classDevices.StepScript(classScriptStep);

            var message = $"Шаг {classScriptStep.StepParams.NumStep} {classScriptStep.StepParams.NameStep} запущен";
            var log = new ClassDataMainLog($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}  ", message);

            _script.ViewData.MessageOut = _script.ViewData.MessageOut.Insert(_script.ViewData.MessageOut.Count(), log.ToString() + $"{Environment.NewLine}");

            log.AppendToFile($"{_script.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");

            _script.ClsDataTime.StartScriptStep(classScriptStep.StepParams.NumStep - 1);

            Console.WriteLine();
            while (!FlagAbort && !AbortScript)
            {

                if (ListBaseDeviceses.Select(flagAbort => flagAbort.FlagStop).Where(flagAbort1 => flagAbort1 == false).ToList().Count == 0)
                {
                    FlagAbort = true;
                    break;
                }
                if (_script.ClsDataTime.Message != "")
                {

                    var message1 = $"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}   { _script.ClsDataTime.Message}";
                    _script.ViewData.MessageOut = _script.ViewData.MessageOut.Insert(_script.ViewData.MessageOut.Count(), message1.ToString() + $"{Environment.NewLine}");
                    _script.ClsDataTime.Message = "";
                }
                
                if (_script.ClsDataTime.TimeCycleStep <= 0.4)                   
                    continue;

                //Console.WriteLine($"{"BeginStep",-30}{DateTime.Now,-20}");

                #region Проверка времени шага

                if (_script.ClsDataTime.TimeStep >= classScriptStep.StepParams.TimeStep.TotalSeconds && classScriptStep.StepParams.TimeStep.TotalSeconds > 0)
                {
                    FlagAbort = true;
                    break;
                }
                _script.ClsDataTime.StartCycleStep();

                #endregion



                foreach (var contr in listControllers)
                    contr.PrevExecute();

                foreach (ClassBaseDevices classBase in ListBaseDeviceses)
                {
                    if (classBase.FlagStop)
                        continue;
                    //Console.WriteLine($"{classBase.ToString(),-30}{DateTime.Now,-20}");
                    classBase.NextStep();
                }

                foreach (var contr in listControllers)
                    contr.PostExecute();

                
                //Console.WriteLine($"{"EndStep",-30}{DateTime.Now,-20}\n");
            }
            
        }

        public void CheckState(object a, EventArgs o)
        {

            conJumpArgs = (ConJumpArgs)o;
            String message = "";

            switch (conJumpArgs.TypeConditional)
            {
                case TypeConditional.Error:

                    if (conJumpArgs.Conditional == 0)
                        conJumpArgs.Conditional = 1;

                    message = conJumpArgs.Message(_script.Steps[conJumpArgs.Conditional - 1].StepParams.TypeStep);
                    break;

                case TypeConditional.Manual:
                    message = conJumpArgs.Message();
                    break;
            }
                
                
                
            if (conJumpArgs.Conditional > 1 && _script.Steps.Count < 2)
                conJumpArgs.Conditional = 1;

            var log= new ClassDataMainLog($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}  ", message);
            _script.ViewData.MessageOut = _script.ViewData.MessageOut.Insert(_script.ViewData.MessageOut.Count(), log.ToString() + $"{Environment.NewLine}");
            log.AppendToFile($"{_script.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");

            ErrorState = true;
        }

        public void AlarmState(object a, EventArgs o)
        {
            var conArgs = (ConJumpArgs)o;
            var message = conArgs.Message();

            if (conArgs.TypeConditional == TypeConditional.FatalError)
            {
                fatalError = true;
                Stop();
            }

            var log = new ClassDataMainLog($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToLongTimeString()}  ", message);
            _script.ViewData.MessageOut = _script.ViewData.MessageOut.Insert(_script.ViewData.MessageOut.Count(), log.ToString() + $"{Environment.NewLine}");
            log.AppendToFile($"{_script.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");


        }

        private void CalcProgressBar()
        {
            int fullTimeScript = 0;
            for (int i = 0; i < _script.Steps.Count; i++)
            {
                fullTimeScript = fullTimeScript + Convert.ToInt32(_script.Steps[i].StepParams.TimeStep);
            }

            _script.ViewData.MaxValueProgress = fullTimeScript;
        }

        private void InitializeVoise()
        {
            Voise.SelectVoice("ScanSoft Katerina_Full_22kHz");
            Voise.Volume = 100;
            Voise.Rate = +10;
        }






    }
}
