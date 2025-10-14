using GasStation.xml.Script.EnumConst;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data.RRG
{
    /// <summary>
    /// Класс данных для вычисления расхода
    /// </summary>
    public class ClassRaiseData
    {
        public ClassRaiseData()
        {

        }

        /// <summary>
        /// Текущий расход
        /// </summary>
        protected double CurrSetRaise { get; set; } = 0.0;

        /// <summary>
        /// Тип рабочего интервала
        /// </summary>
        public Regims CurrTypeReg { get; set; } = 0.0;

        /// <summary>
        /// Время шага РРГ
        /// </summary>
        public double StepTime { get; set; } = 0.0;

        /// <summary>
        /// Начальный расход
        /// </summary>
        protected double BegRaise { get; set; } = 0.0;

        /// <summary>
        /// Конечный расход
        /// </summary>        
        protected double EndRaise { get; set; } = 0.0;

        /// <summary>
        /// Начальное время
        /// </summary>
        protected double BegTime { get; set; } = 0.0;

        /// <summary>
        /// Конечное время
        /// </summary>
        protected double EndTime { get; set; } = 0.0;
    }
}
