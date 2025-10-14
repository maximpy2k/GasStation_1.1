using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using GasStation.Annotations;
using GasStation.ViewModels.Commands;
using GasStation.xml;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript.Elements;

namespace GasStation.ViewModels
{
    public class ConditionalWViewModel : INotifyPropertyChanged
    {
        public string Title { get; set; } 

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Добавить условный переход
        /// </summary>
        public CommandAddBranch CmdAddBranch { get; set; }
        /// <summary>
        /// Удалить условный пееход
        /// </summary>
        public CommandRemoveBranch CmdRemoveBranch { get; set; }

        //public ConditionalWViewModel(ClassScript clrScript,XmlClassStep currStep, CommandAddBranch cmdAddbranch, CommandRemoveBranch cmdRemoveBranch)
        //{
        //    CmdAddBranch = cmdAddbranch;

        //    CmdRemoveBranch = cmdRemoveBranch;
            
        //    Title = $"Условные переходы Шаг №{currStep.StepParams.NumStep}";

        //    ListDevice = clrScript.Consts.AvalibleDevices;

        //    //_сountStep = clrScript.Steps.Count;

        //    Steps = clrScript.Steps;

        //    Conditional = currStep.ConditionalScript;
        //}

        public ConditionalWViewModel(MainWindowViewModel viewModel)
        {
            Update(viewModel,viewModel.ClsScript.CurrStep);
        }

        /// <summary>
        /// Добавить условный переход
        /// </summary>
        public void AddConditionalState()
        {

            //Conditional.AddState(new XmlStateConditionScript());
            var ss = new XmlStateConditionScript(_viewModel.ClsScript.Consts);
            Conditional.AddNode(ss);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Conditional"));
        }

        /// <summary>
        /// Удалить условный переход
        /// </summary>
        public void RemoveConditionalState()
        {
            if (CurrState == null)
                return;
            Conditional.RemoveNodeIdx(CurrentStateIdx);
            //Conditional.RemoveNode(CurrState);

            //Conditional.StateScripts.Remove(CurrState);

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Conditional"));
        }

        /// <summary>
        /// Лежит список условных переходов
        /// </summary>
        public XmlConditionalScript Conditional { get; set; }
        
        /// <summary>
        /// Список устройств из констант
        /// </summary>
        public List<string> ListDevice { get; set; }


        /// <summary>
        /// Имена шагов списком
        /// </summary>
        public List<string> NameSteps
        {
            get
            {
                var result = new List<string>();

                foreach (var xmlClassStep in Steps)
                {
                    result.Add($"{xmlClassStep.StepParams.NumStep} {xmlClassStep.StepParams.NameStep}");
                }

                return result;
            }
        }

        private XmlStateConditionScript _currState;
        private ObservableCollection<XmlClassStep> Steps;
        private MainWindowViewModel _viewModel;

        public int _currentStateIdx;
        /// <summary>
        /// Текущий рабочий шаг скрипта
        /// </summary>
        public int CurrentStateIdx
        {
            get
            {
                return _currentStateIdx;
            }
            set
            {
                _currentStateIdx = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentStateIdx"));
            }
        }
        /// <summary>
        /// Текущее условие
        /// </summary>
        public XmlStateConditionScript CurrState
        {
            get
            {
                return _currState;
            }
            set
            {
                _currState = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrState"));
            }
        }

        public void Update(MainWindowViewModel viewModel, XmlClassStep step)
        {
            _viewModel = viewModel;

            CmdAddBranch = new CommandAddBranch(viewModel);

            CmdRemoveBranch = new CommandRemoveBranch(viewModel);



            Title = $"Условные переходы на интервале №{step.StepParams.NumStep}";

            ListDevice = viewModel.ClsScript.Consts.AvalibleDevices;

            //_сountStep = clrScript.Steps.Count;

            Steps = viewModel.ClsScript.Steps;

            Conditional = step.ConditionalScript;
        }
    }
}
