namespace AnalizeLog
{
    public class RegPidCoefs
    {
        private string[] _datas;
        private string _type;

        public RegPidCoefs(string[] datas, string type)
        {
            this._datas = datas;
            this._type = type;
        }

        private void GetData(string[] datas, string type)
        {
            
        }

        /// <summary>
        ///Заданное значение величины
        /// </summary>
        public double SetupValue { get; set; }

        /// <summary>
        /// Измеренное значение величины
        /// </summary>
        public double CurrValue { get; set; }

        /// <summary>
        /// Ошибка наблюдаемой величины
        /// </summary>
        public double E { get; set; }

        /// <summary>
        /// Дифференциальная часть ПИД-регулятора
        /// </summary>
        public double Edif { get; set; }

        /// <summary>
        /// Интегральная часть ПИД-регулятора
        /// </summary>
        public double Eint { get; set; }

        /// <summary>
        /// Пропорциональная часть ПИД-регулятора
        /// </summary>
        public double Eprop { get; set; }

        /// <summary>
        /// Воздействие на текущую величину
        /// </summary>
        public double DeltaValue { get; set; }

        /// <summary>
        /// Суммарная ошибка на текущем шаге
        /// </summary>
        public double SumErr { get; set; }
    }
}