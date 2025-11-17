using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GasStation.xml.Const.Elements;

namespace GasStation.Elements.Data
{
    public class ClassDataChamber : ClassDataBase,INotifyPropertyChanged
    {

        public ClassDataChamber(ClassDataTime dataTime, XmlClassThermoSectionConst con):base(dataTime)
        {
            DataTime = dataTime;
            CurrDate = dataTime.BeginCycleStep;
            _con = con;
            ClassPidIn = new ClassDataPid(0, 0, con.Pid[1]);
            ClassPidOut = new ClassDataPid(0, 0, con.Pid[0]);
        }

        public ClassDataChamber()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;


        /// <summary>
        /// Класс данных времени
        /// </summary>
        public ClassDataTime DataTime { get; set; }

        public bool StateHeat;
        /// <summary>
        /// Время измерения
        /// </summary>

        private double setupTemp;

        /// <summary>
        /// Температура, для установки
        /// </summary>
        public double SetupTemp {
            get
            {
                return setupTemp;
            }
            set
            {
                setupTemp = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SetupTemp"));
            }
        }

        /// <summary>
        /// Установленная мощность
        /// </summary>
        public double SetPower { get; set; }


        ClassDataPid classPidOut;
        /// <summary>
        /// Данные по внешнему ПИД регулятору
        /// </summary>
        public ClassDataPid ClassPidOut
        {
            get
            {
                return classPidOut;
            }
            set
            {
                if (classPidOut == null)
                    classPidOut = new ClassDataPid(0, 0, _con.Pid[0]);

                classPidOut.CurrValue = value.CurrValue;
                classPidOut.DeltaValue = value.DeltaValue;
                classPidOut.Edif = value.Edif;
                classPidOut.Eint = value.Eint;
                classPidOut.Eprop = value.Eprop;
                classPidOut.SetupValue = value.SetupValue;               
            }
        }

        ClassDataPid classPidIn;
        private static XmlClassThermoSectionConst _con;

        /// <summary>
        /// Данные по внутреннему ПИД регулятору
        /// </summary>
        public ClassDataPid ClassPidIn
        {
            get
            {
                return classPidIn;
            }
            set
            {
                if(classPidIn == null)
                    classPidIn = new ClassDataPid(0, 0, _con.Pid[1]);

                classPidIn.CurrValue = value.CurrValue;
                classPidIn.DeltaValue = value.DeltaValue;
                classPidIn.Edif = value.Edif;
                classPidIn.Eint = value.Eint;
                classPidIn.Eprop = value.Eprop;
                classPidIn.SetupValue = value.SetupValue;
            }
        }

        public override string ToString()
        {
            var strIn = ClassPidIn != null ? ClassPidIn.Str : "";
            return $"{CurrDate.ToString(),-20}{TimeStep,-21:0.00}{SetupTemp}{"",-27}{ClassPidOut.Str}{"",-27}{strIn}";
        }

        public override string HeaderStr => $"{"Дата"}                {"Время"}      {"Заданная температура"}    {"ПИД регулятор внешнего контура   "}  {ClassPidOut.HeaderStr}      {"ПИД регулятор внутреннего контура"}  {ClassPidIn.HeaderStr}";
               
        public override byte[] ToByteMas
        {
            get
            {
                byte[] val = new byte[69];
                val[0] = FirstByte;
                BitConverter.GetBytes(DataTime.TimeScript).CopyTo(val, 1);
                BitConverter.GetBytes(SetupTemp).CopyTo(val, 9);
                //BitConverter.GetBytes(TdOut).CopyTo(val, 17);
                //BitConverter.GetBytes(TdIn).CopyTo(val, 25);
                BitConverter.GetBytes(SetPower).CopyTo(val, 33);

                BitConverter.GetBytes(ClassPidOut.E).CopyTo(val,37);
                BitConverter.GetBytes(ClassPidOut.Edif).CopyTo(val, 45);
                BitConverter.GetBytes(ClassPidOut.Eint).CopyTo(val, 53);
                BitConverter.GetBytes(ClassPidOut.Eprop).CopyTo(val, 61);

                return val;
            }
        }

        public bool StateWater;

    }
}
