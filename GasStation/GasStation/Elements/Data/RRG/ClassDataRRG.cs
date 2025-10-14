using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataRRG : ClassDataBase
    {

        public ClassDataRRG(ClassDataTime dataTime) : base(dataTime)
        {
        }

        public double Cap;

        public double Acp;

        public bool StateRrg;

        public double SetupValue;

        public double CurrSetRaise;

        public double ReadRaise;

        /// <summary>
        /// Показание вакуметра
        /// </summary>
        public double CurrPress;


        /// <summary>
        /// Данные ПИД регулятора
        /// </summary>
        public ClassDataPid DataPid { get; set; }

        public override string ToString()
        {
            var s = $"{CurrDate.ToUniversalTime(),-20}{TimeStep,-25:0.00}{Cap,-30:0.00}{Acp,-30:0.00}{SetupValue,-30:0.00}{CurrSetRaise,-30:0.00}{ReadRaise,-30:0.00}{CurrPress,-30:0.00}";
            return s;
        }

        public override string HeaderStr =>
            $"{"Дата",-20}{"Время",-16:0.00}{"Значение ЦАП",-30:0.00}{"Значение АЦП",-30:0.00}{"Установленный расход",-30:0.00}{"Текущий установленный расход",-30:0.00}{"Считанный расход",-30:0.00}{"Текущее воздействие",-30:0.00}";    
    }
}
