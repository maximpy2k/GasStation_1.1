using System.Collections.Generic;

namespace AnalizeLog
{
    internal class TeorVer
    {
        private readonly List<RegChamberSection> _file;
        private List<double> _dts;

        public TeorVer(List<RegChamberSection> file)
        {
            _file = file;
        }

        public List<double> Dts
        {
            get
            {
                if (_dts == null)
                {
                    CreateCoefs(_file);
                }

                return _dts;
            }
        }

        private void CreateCoefs(List<RegChamberSection> file)
        {
            _dts = new List<double> {file[0].Time};

            for (int index = 1; index < file.Count; index++)
            {
                _dts.Add(file[index].Time - file[index-1].Time);
            }
        }
    }
}