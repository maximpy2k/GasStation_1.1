using GasStation.Status;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataMainLog:ClassDataBase
    {
        public string Message;
        private string _localTime;

        public ClassDataMainLog(string time,string message)
        {
            Message = message;
            _localTime = time;
        }

        public override string ToString()
        {
            return $"{_localTime,-10}{Message}";
        }

        public override string HeaderStr =>
            $"{"Дата",-11}{"Время",-10}{"Сообщение"}";
    }
}
