using GasStation.xml.Script.EnumConst;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GasStation.Status
{
    public class ConJumpArgs:EventArgs
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="timeStep">Время от начала шага</param>
        public ConJumpArgs(double timeStep)
        {
            dateTime = DateTime.Now;
            TimeStep = timeStep;
        }
        //static SpeechSynthesizer  Voise = new SpeechSynthesizer();

        public DateTime dateTime;
        /// <summary>
        /// Время от начала шага
        /// </summary>
        public double TimeStep;

        public int NumDev;

        public String NameDev;

        public String TextError;

        public double CurrValue;

        /// <summary>
        /// Интервал на который переходим
        /// </summary>
        public int Conditional;

        public TypeConditional TypeConditional;

        public String Message(RegimsStep typeFinishInterval = RegimsStep.Normal)
        {
            switch (TypeConditional)
            {
                case TypeConditional.Error:
                    {
                        if (typeFinishInterval == RegimsStep.Emergency)
                        {
                            var message = $"Aварийный переход на {Conditional} шаг. Устройство: {NameDev} {NumDev} Причина: {TextError} Значение: {CurrValue}";
                            Speak(message);
                            return message;
                        }
                        return $"Условный переход на {Conditional} шаг. Устройство: {NameDev} {NumDev} Значение: {CurrValue}";
                    }
                case TypeConditional.Manual:
                    return $"Ручной переход на {Conditional} Шаг";

                case TypeConditional.Alarm:
                    {
                        var message = $" Тревога! Причина: {TextError} Устройство: {NameDev}_{NumDev} Значение: {CurrValue}";
                        Speak(message);
                        return message;
                    }

                case TypeConditional.FatalError:
                    {
                        var message = $"Фатальная ошибка! Причина: {TextError} Устройство: {NameDev} Значение: {CurrValue}";
                        Speak(message);
                        return message;
                    }

                case TypeConditional.Text:
                    return $"Состояние было изменено {TextError} Устройство: {NameDev}_{NumDev} Значение: {CurrValue}";
                case TypeConditional.StatusText:
                    return $"Устройство: {NameDev}_{NumDev} Статус: {TextError}";

                case TypeConditional.LogWrite:
                    return $"{TextError} Устройство: {NameDev}_{NumDev}";

                case TypeConditional.ErrorReadWrite:
                    return $"{TextError} Устройство: {NameDev}_{NumDev} Значение: {CurrValue}";

                default:
                    return "";
            }
        }

        private void Speak(string message)
        {
            //StaticClassVoice.Speak(message);
        }
        //private static ConcurrentQueue<string> queue = new ConcurrentQueue<string>();

        //private static bool start=true;
        //public void Speak(String message)
        //{
        //    var cnt = StaticClassVoice.queue.Count;
        //    Thread thread = new Thread(StaticClassVoice.SpeakVoise);
        //    StaticClassVoice.queue.Enqueue(message);
        //    if (StaticClassVoice.start)
        //        thread.Start();
        //    //SpeakVoise();
        //  //  Thread thread = new Thread(SpeakVoise);
        //  //  thread.Start(message);

        //}

        //private static void SpeakVoise()
        //{
        //    try
        //    {
        //        start = false;
        //        while (queue.Count != 0)
        //        {
        //            var message = "";
        //            if(!queue.TryDequeue(out message))
        //                continue;
        //            Voise.Speak(message);
        //        }

        //        start = true;
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e.Message);
        //    }

        //}
    }
}
