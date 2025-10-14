using System.ComponentModel;

namespace GasStation.xml.Script.EnumConst
{
    public enum Regims
    {
        [Description("Максимально быстро")]
        MaximumSpeed,
        [Description("За время интервала")]
        TimeInterval,
        [Description("Скорость заданная в константах")]
        DefaultSpeed
    }
}
