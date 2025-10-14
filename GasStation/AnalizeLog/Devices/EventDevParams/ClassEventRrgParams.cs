using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.Elements.Data;

namespace GasStation.Devices.EventDevParams
{
    public class ClassEventRrgParams:EventArgs
    {
        public ClassDataRRG _classDataRrg;

        public ClassEventRrgParams(ClassDataRRG classDataRrg)
        {
            _classDataRrg = classDataRrg;
        }

    }
}
