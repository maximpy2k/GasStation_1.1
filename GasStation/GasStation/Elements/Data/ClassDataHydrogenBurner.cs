using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataHydrogenBurner:ClassDataBase
    {
        public ClassDataHydrogenBurner(ClassDataTime dataTime) : base(dataTime)
        {
        }


        public double TdFire;

        public double TdBurner;

        public bool StateFire;

        public bool StateWater;

        public bool StateRelay;

        public bool StateHeat;

        public override string HeaderStr =>
            $"{"Дата",-20}{"Время",-10}{"Температура",-10}{"Температура пламени",-10}{"Нагрев",-16}{"Датчик пламени",-16}{"Водяное охлаждение",-16}{"Реле нагрева",-16}";

        public override string ToString() =>
                    $"{CurrDate.ToString(),-20}{TimeStep,-10:0.00}{TdBurner,-10:0.00}{TdFire,-10:0.00}{StateHeat,-10}{StateFire,-10}{StateWater,-10}{StateRelay,-10}";
    }
}
