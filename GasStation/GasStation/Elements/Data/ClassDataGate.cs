using GasStation.UserControls.Elements;
using GasStation.xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataGate:ClassDataBase
    {
        private readonly XmlClassGate xmlСlassGate;

        public ClassDataGate(ClassDataTime dataTime, XmlClassGate xmlСlassGate) : base(dataTime)
        {
            this.xmlСlassGate = xmlСlassGate;
        }
        public bool StatusGateOpen { get; set; }
        public bool StatusGateClose { get; set; }

        public override string HeaderStr => $"{"Дата",-20}{"Время",-10}{"Кнопка открытия заслонки",-20}{"Кнопка закрытия заслонки",-20}{"Датчик открытой заслонки",-30}{"Датчик закрытой заслонки",-30}";

        public override string ToString()
        {
            return $"{CurrDate.ToString(),-20}{TimeStep,-8:0.00}{xmlСlassGate.GateOpen,-30}{xmlСlassGate.GateClose,-30}{xmlСlassGate.GateView.OpenGateStatus,-30}{xmlСlassGate.GateView.CloseGateStatus,-30}";
        }
    }
}
