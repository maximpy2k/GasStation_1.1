using GasStation.xml.Script.XmlScript.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.xml.Script;

namespace GasStation.Status
{
    public class ClassStatusScript
    {
        private readonly XmlStateConditionScript[] _states;

        public ClassStatusScript(XmlStateConditionScript[] states)
        {
            _states = states;
        }
    }
}
