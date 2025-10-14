using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataSensor : ClassDataBase
    {

        public ClassDataSensor(ClassDataTime dataTime, bool sensorValue):base(dataTime)
        {
            SensorValue = sensorValue;
        }

        /// <summary>
        /// Состояние датчика
        /// </summary>
        public bool SensorValue;

        /// <summary>
        /// Конвертация в массив
        /// </summary>
        public override byte[] ToByteMas
        {
            get
            {
                List<byte> lst=new List<byte>();
                lst.Add(FirstByte);
                lst.AddRange(BitConverter.GetBytes(TimeStep));
                lst.AddRange(BitConverter.GetBytes(SensorValue));
                return lst.ToArray();
            }
        }
        /// <summary>
        /// Конвертация в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            
            return $"{CurrDate.ToString(),-20}{TimeStep,-16:0.00}{ ((SensorValue == true) ? "Используется" : "Не используется"),-10}";
        }
        /// <summary>
        /// Заголовок текстового файла
        /// </summary>
        public override string HeaderStr => $"{"Дата",-20}{"Время",-16:0.00}{"Состояние датчика",-16}";
    }
}
