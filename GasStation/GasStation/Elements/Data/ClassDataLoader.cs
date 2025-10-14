using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataLoader:ClassDataBase
    {
        public ClassDataLoader(ClassDataTime dataTime, bool stateLoader):base(dataTime)
        {
            StateLoader = stateLoader;
        }
        public bool StateLoader;

        public override string HeaderStr =>
            $"{"Дата",-20}{"Время",-10}{"Состояние загрузчика",-10}";

        public override string ToString()
        {
            return $"{CurrDate.ToString(),-20}{TimeStep,-10:0.00}{((StateLoader == true) ? "Загружен" : "Не загружен"),-10}";
        }
    }
}
