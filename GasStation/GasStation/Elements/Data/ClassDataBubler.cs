using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataBubler:ClassDataBase
    {

        public ClassDataBubler(ClassDataTime dataTime) : base(dataTime)
        {            
        }     

        /// <summary>
        /// Состояние борботера
        /// </summary>
        public bool Relay;

        /// <summary>
        /// Состояние борботера
        /// </summary>
        public bool UseBubbler;
        /// <summary>
        /// Текущая температура
        /// </summary>
        public double CurrTd;
        /// <summary>
        /// Конвертация в массив
        /// </summary>
        public override byte[] ToByteMas
        {
            get
            {
                byte[] val = new byte[25];
                val[0] = FirstByte;
                BitConverter.GetBytes(TimeStep).CopyTo(val, 1);
                BitConverter.GetBytes(Relay).CopyTo(val, 9);
                BitConverter.GetBytes(CurrTd).CopyTo(val, 17);
                return val;
            }
        }
        /// <summary>
        /// Конвертация в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{CurrDate.ToString(),-20}{TimeStep,-16:0.00}{((UseBubbler == true) ? "Используется" : "Не используется"),-16}{((Relay == true) ? "Реле вкл" : "Реле выкл"),-16}{CurrTd,-16:0.00}";
        }
        /// <summary>
        /// Заголовок текстового файла
        /// </summary>
        public override string HeaderStr => $"{"Дата",-20}{"Время",-16:0.00}{"Состояние борботера",-16}{"Состояние реле",-16}{"Текущая температура",-16:0.00}";
    }
}
