using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{

    public class ClassDataBase
    {
        public ClassDataBase()
        {

        }
        public ClassDataBase(ClassDataTime dataTime)
        {
            TimeStep = dataTime.TimeStep;
            CurrDate = dataTime.BeginCycleStep;
        }

        public byte FirstByte = 0x11;
        public virtual byte[] ToByteMas { get; set; }
        
        /// <summary>
        /// Текущая дата
        /// </summary>
        public DateTime CurrDate { get; set; }

        /// <summary>
        /// Время измерения
        /// </summary>
        public double TimeStep;

        /// <summary>
        /// Шапка для общего лога
        /// </summary>
        public virtual string HeaderStr { get; }

        public void AppendToFile(string path)
        {
            var dirPath = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            if(!File.Exists(path))
                File.AppendAllLines(path, new[] { HeaderStr });
            File.AppendAllLines(path, new[] { ToString() });
        }
       


    }
}
