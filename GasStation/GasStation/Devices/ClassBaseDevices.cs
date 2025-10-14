using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Elements.Data;
using GasStation.Controllers;
using GasStation.xml.Script;
using GasStation.xml.Script.XmlScript.Elements;

namespace GasStation.Devices
{
    /// <summary>
    /// Базовый класс устройства
    /// </summary>
    public abstract class ClassBaseDevices
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="classDataTime">Класс данных по времени</param>
        public ClassBaseDevices(ClassDataTime classDataTime)
        {
            _classDataTime = classDataTime;
        }

        /// <summary>
        /// Список контроллеров
        /// </summary>
        public List<BaseClassController> LstContr;

        /// <summary>
        /// Событие вывода данных 
        /// </summary>
        public EventHandler EventCommingData;

        /// <summary>
        /// Событие обработки условного перехода
        /// </summary>
        public EventHandler StateError;

        /// <summary>
        /// Событие обработки тревоги
        /// </summary>
        public EventHandler AlarmError;


        /// <summary>
        /// Максимальное колличество данных в списке
        /// </summary>
        public int Cnt = 3600;
        private int currCnt = 0;

        /// <summary>
        /// Последние запомненные данные
        /// </summary>
        public ClassDataBase LastData;

        /// <summary>
        /// Добавление данных в список
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        protected bool AddData(ClassDataBase data)
        {
            if (data == null)
                return false;
            LastData = data;
            currCnt++;


            #region Обнуление
            if (currCnt > Cnt && _stepParams.NumStep == 1)
            {
                currCnt = 0;
                return true;
            }
            //DataList1.Add(data);
            //if (DataList1.Count > Cnt && _stepParams.NumStep == 1)
            //{                
            //    DataList1 = DataList1.Where((dat, idx) => idx >= DataList1.Count - Cnt).ToList();
            //}
            #endregion
            return false;
        }


        /// <summary>
        /// Функция начальной инициализации
        /// </summary>
        public virtual void Init()
        {
            FlagStart = false;            
        }

        /// <summary>
        /// Переход на следующий шаг внутри одной команды
        /// </summary>
        /// <param name="writeLog">флаг записи в Log</param>
        public void NextStep(bool writeLog=true)
        {
            NextStepFunc();
            if(writeLog)
                WriteLog();
        }

        /// <summary>
        /// Переход на следующий шаг внутри одной команды
        /// </summary>
        protected abstract void NextStepFunc();
        public virtual void FailSave()
        {
            FlagStop = true;
        }

        /// <summary>
        /// Флаг остановки
        /// </summary>
        public bool FlagStop;

        public bool newStep=false;
        /// <summary>
        /// Флаг запуска
        /// </summary>
        public bool FlagStart;

        public override string ToString()
        {
            return base.ToString().Split(new[] { '.' }).Last();
        }

        /// <summary>
        /// Класс данных по времени
        /// </summary>
        protected readonly ClassDataTime _classDataTime;        
        /// <summary>
        /// Параметры шага устройства
        /// </summary>
        protected XmlBaseClassElementScript _clsDevStep;
        /// <summary>
        /// Условные переходы
        /// </summary>
        protected XmlStateConditionScript[] _states;
        /// <summary>
        /// Параметры шага скрипта
        /// </summary>
        protected XmlClassStepParams _stepParams;
        /// <summary>
        /// Путь к файлу лога
        /// </summary>
        private string _pathToLog
        {
            get
            {
                //2018.10.30  14 - 03 - 20 №321_[fdsfsdfdsfsd]
                var dir = _classDataTime.BeginStep.ToString("yyyy.MM.dd hh-mm-ss");
                var num = _stepParams.NumStep;
                var name = _stepParams.NameStep;
                return $"{_stepParams.PathToLog}\\{dir} №{num:000}[{name}]\\{_clsDevStep.Name}_{_clsDevStep.Num:00}.txt";
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public string path = "";


        public void WriteLog()
        {
            if (LastData != null&& _stepParams!=null)
                LastData.AppendToFile(_pathToLog);
        }
    }
}
