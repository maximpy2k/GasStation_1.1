using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnalizeLog
{
    public class ReaderLog
    {
        public static List<RegChamberSection> ReadChamberSection(string path,bool title = false)
        {
            var strList = File.ReadAllLines(path);


            var result = new List<RegChamberSection>();

            if (title)
            {
                result = strList.Select(str => new RegChamberSection(str)).ToList();
            }
            else
            {
                result.AddRange(strList.Select(str => new RegChamberSection(str)).Where(reg => !reg.StrTitle));
            }

            return result;
        }
    }
}