using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataVacuumetr : ClassDataBase
    {

        public ClassDataVacuumetr(ClassDataTime dataTime, double press, bool stateDo):base(dataTime)
        {
            StateDo = stateDo;
            Press = press;
        }

        /// <summary>
        /// Состояние выхода
        /// </summary>
        public bool StateDo;

        /// <summary>
        /// Давление
        /// </summary>
        public double Press;
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
                lst.AddRange(BitConverter.GetBytes(StateDo));
                lst.AddRange(BitConverter.GetBytes(Press));
                return lst.ToArray();
            }
        }
        /// <summary>
        /// Конвертация в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{CurrDate.ToString(),-20}{TimeStep,-16:0.00}{Press,-16:0.00}{StateDo,-16}";
        }
        /// <summary>
        /// Заголовок текстового файла
        /// </summary>
        public override string HeaderStr => $"{"Дата",-20}{"Время",-16:0.00}{"Давление",-16}{"Состояние выхода",-16:0.00}";
    }
}
