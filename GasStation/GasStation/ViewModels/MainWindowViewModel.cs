using GasStation.ViewModels.Commands;
using System;
using System.ComponentModel;
using GasStation.xml;

namespace GasStation.ViewModels
{
    public enum EnumGraph
    {
        [Description("Нет")]
        Текущая_уст_температуры,
        [Description("Нет1")]
        Внутренний_термодатчик,
        [Description("Нет2")]
        Внешний_термодатчик,
        [Description("Нет3")]
        Установленный_расход_газа,
        [Description("Нет4")]
        Реальный_расход_газа
    }
       
    public enum EnumDevice
    {
        Регулятор_расхода_газа,
        Термосекция,
        
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        #region Реализация интерфейса INotifyPropertyChanged
        /// <summary>
        /// Изменение Property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private ClassScript _clsScript;
        /// <summary>
        /// Класс данных технологического процесса
        /// </summary>
        public ClassScript ClsScript
        {
            get
            {
                return _clsScript;
            }
            set
            {
                _clsScript = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClsScript"));
            }
        }

        public string Version { get; set; }

        public string NameStend { get; set; }

        public event EventHandler EventUpdateCanExecute;

        public void UpdateCanExecuteCommands()
        {
            EventUpdateCanExecute?.Invoke(this, null);
        }
        public void UpdateClsScript()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ClsScript"));
            ClsScript.Update();
        }
        public string MessageOut;

        public ClassProcessingScript ClassProcessingScript;

        public MainWindowViewModel(string version = @"Версия 12.1 от 09.07.2018", string name = @"Оксид 3") : this()
        {
            Version = version;

            NameStend = name;
        }
        
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public MainWindowViewModel()
        {
            NameStend = @"Оксид 3";

            Version = @"Версия 12.1 от 09.07.2018";

            ClsScript = new ClassScript(ClassScript.DefaultConstPath);

            //_title = $"{NameStend} ПО ({Version}) Канал {ClsScript.Consts.NameChannel.RusName}";

            CmdAddStep = new CommandAddStep(this);
            CmdRemoveStep = new CommandRemoveStep(this);
            CmdGoToStep = new CommandGoToStep(this);
            CmdGoToFirstStep = new CommandGoToFirstStep(this);
            CmdOpenScript = new CommandOpenScript(this);
            CmdNewProject = new CommandNewPrj(this);
            CmdSaveScript = new CommandSaveScript(this);
            CmdSaveConst = new CommandSaveConst(this);
            CmdStartScript =new CommandStartScript(this);
            CmdStopScript = new CommandStopScript(this);
            CmdPauseScript = new CommandPauseScript(this);
            CmdRrgStepParams = new CommandStepParamsRrg (this);
            CmdNextStep = new CommandNextStep(this);

            CmdGoBranch = new CommandGoBranch(this);            
            CmdCopyStep = new CommandCopyStep(this);
            CmdChangeUser = new CommandChangeUser(this);

            //CmdBublerView = new CommandBublerView(this);

            CmdPumpView = new CommandPumpView(this);

            

            //CmdAddBranch = new CommandAddBranch(ConditionalView);

            //CmdRemoveBranch = new CommandRemoveBranch(ConditionalView);
        }


        #region Реализация комманд интерфейса

        public CommandPumpView CmdPumpView { get; set; }
        public CommandBublerView CmdBublerView { get; set; }
        public CommandChangeUser CmdChangeUser { get; set; }

        public CommandStepParamsFreqGenerator CmdStepParamsFreqGenerator { get; set; } = new CommandStepParamsFreqGenerator();
        public CommandStepParamsChamber ComStepParamsChamber { get; set; } = new CommandStepParamsChamber();
        public CommandStepParamsLoader CmdStepParamsLoader { get; set; } = new CommandStepParamsLoader();
        public CommandStepParamsFlap CmdStepParamsFlap { get; set; } = new CommandStepParamsFlap();
        public CommandStepParamsBubbler CmdStepParamsBubbler { get; set; } = new CommandStepParamsBubbler();


        public CommandAddStep CmdAddStep { get; set; }
        public CommandGoToStep CmdGoToStep { get; set; }
        public CommandGoToFirstStep CmdGoToFirstStep { get; set; }
        public CommandRemoveStep CmdRemoveStep { get; set; }
        public CommandOpenScript CmdOpenScript { get; set; }
        public CommandNewPrj CmdNewProject { get; set; }
        public CommandSaveScript CmdSaveScript { get; set; }
        public CommandSaveConst CmdSaveConst { get; set; }

        public CommandStartScript CmdStartScript { get; set; }
        public CommandStopScript CmdStopScript { get; set; }
        public CommandNextStep CmdNextStep { get; set; }
        public CommandPauseScript CmdPauseScript { get; set; }
        public CommandStepParamsRrg  CmdRrgStepParams { get; set; }
        public CommandGoBranch CmdGoBranch { get; set; }
        
        public CommandCopyStep CmdCopyStep { get; set; }

        
        #endregion

        #region Условные переходы

        private ConditionalWViewModel _conditionalView;

        public ConditionalWViewModel ConditionalView
        {
            get
            {
                if (_conditionalView == null)
                {
                    if (ClsScript.CurrStep !=null)
                    {
                        _conditionalView = new ConditionalWViewModel(this);
                    }
                }

                return _conditionalView;
            }
        }

        ///// <summary>
        ///// Добавить условный переход
        ///// </summary>
        //public CommandAddBranch CmdAddBranch { get; set; }
        ///// <summary>
        ///// Удалить условный пееход
        ///// </summary>
        //public CommandRemoveBranch CmdRemoveBranch { get; set; }
        #endregion

        private double _sliderValue = 0.6;
        public double SliderValue
        {
            get { return _sliderValue; }
            set
            {
                _sliderValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SliderValue"));
            }
        }

    }

}
