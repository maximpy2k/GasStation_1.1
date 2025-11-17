using GasStation.xml.Constant;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using GasStation.xml.Constant.XmlConst.Elements;
using GasStation.Status;
using GasStation.xml.Script.EnumConst;
using System.Linq;
using GasStation.Elements.Data;

namespace GasStation.Controllers
{
    public class BaseClassController
    {
        /// <summary>
        /// Константы контроллера
        /// </summary>
        protected XmlClassControllerConst contrConst;
        /// <summary>
        /// Последовательный порт
        /// </summary>
        private readonly SerialPort _sp;
        /// <summary>
        /// Путь к логу программы
        /// </summary>
        private readonly string _pathToLogFolder;



        /// <summary>
        /// Статус работы контроллера
        /// </summary>
        public bool ControllerStatus { get; protected set; }

        #region Ошибки работы контроллера
        /// <summary>
        /// Колличество неправильно принятых строк
        /// </summary>
        public int ErrAnswerString;

        /// <summary>
        /// Колличество ошибок чтения
        /// </summary>
        public int ErrRead = 0;
        /// <summary>
        /// Колличество ошибок записи
        /// </summary>
        public int ErrWrite = 0;

        public int ErrWdt = 0;

        public int CountResetWdt = 0;
        /// <summary>
        /// Добавление ошибки чтения
        /// </summary>
        protected void AddErrRead()
        {
            ErrRead++;
            if (ErrRead > contrConst.CountReads)
                ControllerStatus = false;


        }
        /// <summary>
        /// Добавление ошибки записи
        /// </summary>
        protected void AddErrWrite()
        {
            ErrWrite++;
            if (ErrWrite > contrConst.CountReads)
                ControllerStatus = false;
        }

        /// <summary>
        /// Добавление ошибки записи
        /// </summary>
        protected void AddErrWdt()
        {
            ErrWdt++;
        }
        /// <summary>
        /// Очистка ошибок чтения
        /// </summary>
        protected void ClrErrRead()
        {
            ErrRead = 0;
        }

        protected void ClrErrWdt()
        {
            ErrRead = 0;
        }
        /// <summary>
        /// Очистка ошибок записи
        /// </summary>
        protected void ClrErrWrite()
        {
            ErrWrite = 0;
        }
        #endregion        

        /// <summary>
        /// Событие обработки тревоги
        /// </summary>
        public EventHandler AlarmError;

        public BaseClassController(String pathToLogFolder, XmlClassControllerConst contrConst, SerialPort sp)
        {
            this.contrConst = contrConst;
            _sp = sp;
            _pathToLogFolder = pathToLogFolder;
            ControllerStatus = true;
        }

        public string SendCmd(string s)
        {
            try
            {
                #region Очистка буферов чтения и записи
                if (_sp.BytesToRead != 0)
                {
                    _sp.ReadExisting();
                    _sp.DiscardInBuffer();
                    _sp.DiscardOutBuffer();
                }
                #endregion

                AddLog(DateTime.Now, s, Transfer.Запись);
                _sp.WriteLine(s);


                Thread.Sleep(contrConst.DelayRead);

                if (_sp.BytesToWrite != 0)
                {
                    AddLog(DateTime.Now, $"Ошибка {ErrWrite} Буфер отправки не пуст.", Transfer.Запись);
                    AddErrWrite();
                    GenerateAlarm(Transfer.Запись, ErrWrite, TypeConditional.Alarm);
                    return null;
                }
                //ControllerStatus = true;
                ClrErrWrite();
            }
            catch (Exception e)
            {
                AddLog(DateTime.Now, $"Ошибка {ErrWrite} {e.Message}", Transfer.Запись);
                AddErrWrite();
                GenerateAlarm(Transfer.Запись, ErrWrite, TypeConditional.Alarm);
                return null;
            }

            return s;
        }

        public string SendCmdWithAnswer(string s)
        {
            if (SendCmd(s) == null)
                return null;

            var answer = "";

            try
            {
                if (_sp.BytesToRead == 0)
                {
                    answer = null;
                    AddLog(DateTime.Now, $"Ошибка {ErrRead} Нет данных для чтения", Transfer.Чтение);
                    AddErrRead();
                    GenerateAlarm(Transfer.Чтение, ErrRead, TypeConditional.Alarm);
                    return null;
                }
                answer = _sp.ReadLine();
                AddLog(DateTime.Now, answer, Transfer.Чтение);
                ClrErrRead();
                ControllerStatus = true;
                return answer;
            }
            catch (Exception e)
            {
                AddLog(DateTime.Now, $"Ошибка {ErrWrite} {e.Message}", Transfer.Чтение);
                AddErrRead();
                GenerateAlarm(Transfer.Чтение, ErrRead, TypeConditional.Alarm);
                return null;
            }
        }

        private void GenerateAlarm(Transfer dest, int errNum, TypeConditional condType)
        {
            ConJumpArgs conJumpArgs = new ConJumpArgs(0.0);
            conJumpArgs.NumDev = contrConst.DevNum;
            conJumpArgs.NameDev = $"Контроллер{contrConst.NameController}";
            conJumpArgs.CurrValue = errNum;

            var errTxt = "";
            switch (dest)
            {
                case Transfer.Чтение:
                    errTxt = "Ошибка чтения";
                    break;
                case Transfer.Запись:
                    errTxt = "Ошибка записи";
                    break;
                default:
                    errTxt = dest.ToString();
                    break;
            }
            conJumpArgs.TextError = errTxt;
            conJumpArgs.Conditional = 0;

            if (errNum == 1)
                conJumpArgs.TypeConditional = condType;
            else
                conJumpArgs.TypeConditional = TypeConditional.ErrorReadWrite;

            AlarmError?.Invoke(this, conJumpArgs);
        }

        #region Wdt таймер
        /// <summary>
        /// Время последнего подтверждения статуса wdt таймера
        /// </summary>
        private static DateTime lastWdtOk = DateTime.Now;

        /// <summary>
        /// Проверка таймера Wdt
        /// </summary>
        public virtual void CheckWdt()
        {
            string cmd1 = $"~{contrConst.Pa:X2}0";
            var ans1 = SendCmdWithAnswer(cmd1);

            if (string.IsNullOrEmpty(ans1))
            {
                ErrAnswerString++;
                return;
            }
            if (ans1.Length != 5)
            {
                ErrAnswerString++;
                return;
            }
            if (ans1.Substring(0, 1) != "!")
            {
                ErrAnswerString++;
                return;
            }

            var adr = ans1.Substring(1, 2);
            if (adr != $"{contrConst.Pa:X2}")
            {
                ErrAnswerString++;
                return;
            }
            var answCmd = ans1.Substring(3, 2);

            switch (answCmd)
            {
                //WdtStatusOk
                case "80":
                    ControllerStatus = true;
                    ClrErrWdt();
                    CountResetWdt = 0;
                    break;
                //WdtStatusErr
                case "04":
                case "40":
                default:
                    AddErrWdt();
                    GenerateAlarm(Transfer.Ошибка_сработал_Wdt_таймер, ErrWdt, TypeConditional.Alarm);
                    if (CountResetWdt < ErrWdt)
                        ResetWdt();
                    ControllerStatus = false;
                    CountResetWdt++;
                    break;
            }
        }
        /// <summary>
        /// Проверка доступности контроллера
        /// </summary>
        public virtual void CheckAvalible()
        {
            string cmd1 = $"${contrConst.Pa:X2}M";
            var ans1 = SendCmdWithAnswer(cmd1);
        }
        /// <summary>
        /// Сброс Wdt таймера и установка времени срабатывания
        /// </summary>
        public virtual void ResetWdt()
        {
            //Сброс
            string cmd1 = $"~{contrConst.Pa:X2}1";
            var ans1 = SendCmdWithAnswer(cmd1);
            //Установка времени срабатывания
            string cmd2 = $"~{contrConst.Pa:X2}31{contrConst.WDT}";
            var ans2 = SendCmdWithAnswer(cmd2);
        }
        #endregion

        public void PrevExecute()
        {
            #region Управление WDT таймером
            if ((DateTime.Now - lastWdtOk).TotalSeconds > 0.7)
            {
                lastWdtOk = DateTime.Now;
                SendCmd("~**");
            }
            CheckWdt();
            #endregion

            PrevExecuteFunc();
        }

        /// <summary>
        /// Получение информации перед запуском шага
        /// </summary>
        protected virtual void PrevExecuteFunc()
        {

        }
        /// <summary>
        /// Получение информации после запуска шага
        /// </summary>
        public virtual void PostExecute()
        {
        }


        /// <summary>
        /// Добавление строки в лог 
        /// </summary>
        /// <param name="cmd"> команда</param>
        /// <param name="status">true - запись, false - чтение</param>
        public void AddLog(DateTime dateTime, String cmd, Transfer dest)
        {
            ClassDataControllerLog log = new ClassDataControllerLog(dateTime, cmd, dest);
            log.AppendToFile($"{_pathToLogFolder}\\Контроллеры\\{contrConst.NameController}_{contrConst.DevNum}.txt");
        }

    }
}
