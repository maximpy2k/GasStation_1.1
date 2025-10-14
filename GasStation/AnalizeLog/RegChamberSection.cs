using System;

namespace AnalizeLog
{
    public class RegChamberSection
    {
        private readonly string _str;
        private double? _time;
        private double? _setupTemp;
        private RegPidCoefs _pidIn;
        private RegPidCoefs _pidOut;

        public RegChamberSection(string str)
        {
            _str = str;
        }

        public double Time
        {
            get
            {
                if (_time==null)
                {
                    GetData(_str);
                }

                return Convert.ToDouble(_time);
            }
        }

        public double SetupTemp
        {
            get
            {
                if (_setupTemp == null)
                {
                    GetData(_str);
                }

                return Convert.ToDouble(_setupTemp);
            }
        }

        private void GetData(string str)
        {
            var datas = str.Split(new []{' '});

            if (datas[0]=="Time")
            {
                StrTitle = true;

                return;
            }

            _time = Convert.ToDouble(datas[0]);

            _setupTemp = Convert.ToDouble(datas[1]);

            _pidIn = new RegPidCoefs(datas,"in");

            _pidOut = new RegPidCoefs(datas,"out");
        }

        public RegPidCoefs PidOut
        {
            get
            {
                if (_pidOut==null)
                {
                    GetData(_str);
                }

                return _pidOut;
            }
        }

        public RegPidCoefs PidIn
        {
            get
            {
                if (_pidIn == null)
                {
                    GetData(_str);
                }

                return _pidIn;
            }
        }
        

        public bool StrTitle { get; private set; }
    }
}