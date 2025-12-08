using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;

namespace GasStation.Elements.Data
{
    public class ClassDataTime : INotifyPropertyChanged
    {
        #region Реализация интерфейса INotifyPropertyChanged

        /// <summary>
        /// Изменение Property
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Реализация интерфейса INotifyPropertyChanged

        /// <summary>
        /// Шаги скрипта
        /// </summary>
        public ObservableCollection<XmlClassStep> Steps;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        public ClassDataTime(ObservableCollection<XmlClassStep> steps)
        {
            Steps = steps;
        }

        /// <summary>
        /// Время начала скрипта
        /// </summary>
        public DateTime BeginScript { get; private set; }
        /// <summary>
        /// Время начала шага скрипта
        /// </summary>
        public DateTime BeginStep { get; private set; }
        /// <summary>
        /// Время начала циклического шага
        /// </summary>
        public DateTime BeginCycleStep { get; private set; }

        /// <summary>
        /// Время окончания циклического шага
        /// </summary>
        public DateTime EndCycleStep { get; set; }

        /// <summary>
        /// Запуск нового скрипта
        /// </summary>
        public void StartScript()
        {
            BeginScript = DateTime.Now;
            BeginStep = BeginScript;
            BeginCycleStep = BeginScript;
        }

        /// <summary>
        /// Начало нового шага скрипта
        /// </summary>
        public void StartScriptStep(int stepNum)
        {
            TimeStep = 0;
            BeginStep = DateTime.Now;


            BeginCycleStep = BeginStep;
            EndCycleStep = BeginStep;
            this.stepNum = stepNum;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RemainingTimeStep"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RemainingTimeScript"));
        }

        /// <summary>
        /// Начало нового циклического шага
        /// </summary>
        public void StartCycleStep()
        {
            EndCycleStep = DateTime.Now;
            if (!FlagPause)
            {
                TimeStep += (EndCycleStep - BeginCycleStep).TotalMilliseconds / 1000.0;
                TimeScript += (EndCycleStep - BeginCycleStep).TotalMilliseconds / 1000.0;
            }
            BeginCycleStep = EndCycleStep;

            Console.WriteLine($"{TimeStep} {RemainingTimeStep}");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RemainingTimeStep"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("RemainingTimeScript"));
        }

        /// <summary>
        /// Номер выполняемого шага
        /// </summary>
        private int stepNum = 0;

        /// <summary>
        /// Время от начала циклического шага
        /// </summary>
        public double TimeCycleStep
        {
            get
            {
                return (DateTime.Now - BeginCycleStep).TotalMilliseconds / 1000.0;
            }
        }
        public string Message = "";
        /// <summary>
        /// Отработанное время шага
        /// </summary>
        public double TimeStep { get; set; } = 0;

        /// <summary>
        /// Отработанное время скрипта
        /// </summary>
        public double TimeScript { get; set; }

        private bool flagPause = false;
        /// <summary>
        /// Флаг паузы
        /// </summary>
        public bool FlagPause
        {
            get
            {


                return flagPause;
            }
            set
            {
                if (flagPause != value)
                {
                    if(flagPause ==false) 
                        Message = "Техпроцесс поставлен на паузу";
                    else
                        Message = "Техпроцесс продолжен";
                    
                    
                }
                flagPause = value;
            }
        }
        /// <summary>
        /// Заданное время шага скрипта
        /// </summary>
        public double StepLengh
        {
            get { return Steps[stepNum].StepParams.TimeStep.TotalMilliseconds / 1000.0; }
        }

        /// <summary>
        /// Оставшееся время скрипта
        /// </summary>
        public double RemainingTimeScriptSec
        {
            get
            {
                var selSteps = Steps.Where(dat => dat.StepParams.NumStep >= stepNum + 1 && dat.StepParams.TypeStep == RegimsStep.Normal && dat.StepParams.TimeStep.TotalMilliseconds / 1000.0 != 0).ToArray();
                if (selSteps == null)
                    return 0;
                return selSteps.Sum(dat => dat.StepParams.TimeStep.TotalMilliseconds / 1000.0);
            }
        }


        /// <summary>
        /// Конвертация секунд в TimeSpawn
        /// </summary>
        /// <param name="sec">Колличество секунд</param>
        /// <returns>TimeSpawn</returns>
        public TimeSpan ConvSecToTime(double sec)
        {
            double m = (int)(sec / 60.0);
            double h = (int)(sec / 60.0 / 60);
            double s = sec - m * 60.0 - h * 60 * 60;
            return new TimeSpan(0, 0, 2000);
        }


        /// <summary>
        /// Время оставшееся до конца шага
        /// </summary>
        public string RemainingTimeStep
        {
            get
            {
                if (Steps[stepNum].StepParams.TimeStep.TotalMilliseconds / 1000.0 == 0)
                    return "Безвременной.";
                var sec = Steps[stepNum].StepParams.TimeStep.TotalMilliseconds / 1000.0 - TimeStep;
                return $"{new TimeSpan(0, 0, (int)sec)}";
            }
        }

        /// <summary>
        /// Время оставшееся до конца скрипта
        /// </summary>
        public string RemainingTimeScript
        {
            get
            {
                var timeStep = 0.0;
                if (Steps[stepNum].StepParams.TimeStep.TotalMilliseconds / 1000.0 != 0)
                    timeStep = TimeStep;
                var sec = RemainingTimeScriptSec - timeStep;
                return $"{new TimeSpan(0, 0, (int)sec)}";
            }
        }


    }
}