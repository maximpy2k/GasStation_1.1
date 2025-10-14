using System;

namespace GasStation.Elements.Data
{
    public class ClassDataShutter:ClassDataBase
    {

        public ClassDataShutter(ClassDataTime dataTime, bool openStatus, bool closeStatus):base(dataTime)
        {
            OpenStatus = openStatus;
            CloseStatus = closeStatus;
        }

        /// <summary>
        /// Данные по клапанам
        /// </summary>
        public ClassDataFlap[] DataFlaps;

        /// <summary>
        /// Статус открытого затвора
        /// </summary>
        public bool OpenStatus;

        /// <summary>
        /// Статус закрытого затвора
        /// </summary>
        public bool CloseStatus;

        /// <summary>
        /// Конвертация в массив
        /// </summary>
        public override byte[] ToByteMas
        {
            get
            {
                byte[] val = new byte[12];
                val[0] = FirstByte;
                BitConverter.GetBytes(TimeStep).CopyTo(val, 1);
                BitConverter.GetBytes(OpenStatus).CopyTo(val, 9);
                BitConverter.GetBytes(CloseStatus).CopyTo(val, 10);
                return val;
            }
        }
        /// <summary>
        /// Конвертация в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var s= $"{CurrDate.ToString(),-20}{TimeStep,-10:0.00}{ ((OpenStatus == true) ? "Используется" : "Не используется"),-12}{ ((CloseStatus == true) ? "Используется" : "Не используется"),-12}";
            for (int i = 0; i < DataFlaps.Length; i++)
                s += $"{ DataFlaps[i]}";
            return s;
            
        }

        /// <summary>
        /// Заголовок текстового файла
        /// </summary>
        public override string HeaderStr
        {
            get
            {
                var s = $"{"Дата",-20}{"Время",-10}{"Затвор открыт",-12}{"Затвор закрыт",-12}";
                for (int i = 0; i < DataFlaps.Length; i++)
                    s += $"{ DataFlaps[i].HeaderStr}";
                return s;
            }
        }
    }
}
