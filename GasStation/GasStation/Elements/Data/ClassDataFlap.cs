namespace GasStation.Elements.Data
{
    public class ClassDataFlap:ClassDataBase
    {
        public ClassDataFlap(ClassDataTime dataTime, bool stateFlap):base(dataTime)
        {
            StateFlap = stateFlap;
        }

        public bool StateFlap;

        public override string HeaderStr => $"{"Дата",-20}{"Время",-8}{"Состояние",-10}";

        public override string ToString()
        {
            return $"{CurrDate.ToString(),-20}{TimeStep,-8:0.00}{((StateFlap == true) ? "Используется" : "Не используется"),-10}";
        }

    }
}
