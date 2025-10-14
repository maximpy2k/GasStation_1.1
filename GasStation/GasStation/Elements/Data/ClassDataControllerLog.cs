using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public enum Transfer
    {
        Фатальная_ошибка,
        Ошибка_сработал_Wdt_таймер,
        Фатальная_ошибка_работы_контроллера,
        Чтение,
        Запись
    }

    public class ClassDataControllerLog : ClassDataBase
    {
        
        public string cmd;
        public Transfer dest;

        public ClassDataControllerLog(DateTime dateTime, string cmd, Transfer dest)
        {
            //Message = message;            
            CurrDate = dateTime;
            this.cmd = cmd;
            this.dest = dest;
        }

        public override string ToString()
        {            
            return $"{CurrDate.ToString(),-23}{dest,-10}{cmd}";
        }

        public override string HeaderStr =>
            $"{"Дата",-23}{"Состояние",-10}{"Сообщение"}";
    }
}
