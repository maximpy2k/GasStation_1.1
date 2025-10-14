using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataPumpSysyem:ClassDataBase
    {
    
        public ClassDataPumpSysyem(ClassDataTime dataTime) :base(dataTime)
        {            
        }

     
        /// <summary>
        /// Текущее давление измеренное вакуметром
        /// </summary>
        public double? CurrPress { get; set; }


        /// <summary>
        /// Использование вакуумного насоса
        /// </summary>
        public bool UseVacuumPump { get; set; }
        /// <summary>
        /// Использование форвакуумного насоса
        /// </summary>
        public bool UseRoughingPump { get; set; }

        /// <summary>
        /// Реальное состояние вакуумного насоса
        /// </summary>
        public bool RealStateVaccuumPump;
        /// <summary>
        /// Реальное состояние форвакуумного насоса
        /// </summary>
        public bool RealStateRoughingPump;

        public bool LampVaccuumPump { get; set; }

        public bool LampRoughingPump { get; set; }

        /// <summary>
        /// Массив с данными
        /// </summary>
        public override byte[] ToByteMas
        {
            get
            {
                byte[] val = new byte[25];
                val[0] = FirstByte;
                BitConverter.GetBytes(TimeStep).CopyTo(val, 1);
                BitConverter.GetBytes(Convert.ToDouble(CurrPress)).CopyTo(val, 9);
                BitConverter.GetBytes(UseVacuumPump).CopyTo(val, 17);
                BitConverter.GetBytes(UseRoughingPump).CopyTo(val, 17);
                BitConverter.GetBytes(RealStateVaccuumPump).CopyTo(val, 17);
                BitConverter.GetBytes(RealStateRoughingPump).CopyTo(val, 17);
                return val;
            }
        }
        /// <summary>
        /// Строка с данными
        /// </summary>
        /// <returns>Строка с данными</returns>
        public override string ToString()
        {
            return CurrPress != null
                ? $"{CurrDate.ToString(),-20}{TimeStep,-16:0.00}{UseVacuumPump,-16}{UseRoughingPump,-16}{RealStateVaccuumPump,-16}{RealStateRoughingPump,-16}{CurrPress,-16}"
                : $"{CurrDate.ToString(),-20}{TimeStep,-16:0.00}{UseVacuumPump,-16}{UseRoughingPump,-16}{RealStateVaccuumPump,-16}{RealStateRoughingPump,-16}";

        }

        /// <summary>
        /// Заголовок для текстового файла
        /// </summary>
        public override string HeaderStr => CurrPress != null
            ? $"{"Дата",-20}{"Время",-16:0.00}{"Использование ФорВакуумного насоса",-16:0.00}{"Использование Вакуумного насоса",-16}{"Текущее состояние ФорВакуумного насоса",-16}{"Текущее состояние Вакуумного насоса",-16}{"Текущее воздействие",-16}"
            : $"{"Дата",-20}{"Время",-16:0.00}{"Использование ФорВакуумного насоса",-16:0.00}{"Использование Вакуумного насоса",-16}{"Текущее состояние ФорВакуумного насоса",-16}{"Текущее состояние Вакуумного насоса",-16}"
            ;
    }
}
