using GasStation.xml.Const.Elements;
using GasStation.xml.Constant.XmlConst.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataHydrogenBurner:ClassDataBase, INotifyPropertyChanged
    {
        public ClassDataHydrogenBurner(ClassDataTime dataTime) : base(dataTime)
        {
        }
        public ClassDataHydrogenBurner(ClassDataTime dataTime, XmlClassHydrogenBurnerConst con) : base(dataTime)
        {
            DataTime = dataTime;
            CurrDate = dataTime.BeginCycleStep;
            _con = con;
            ClassPidOut = new ClassDataPid(0, 0, con.Pid);
        }

        public ClassDataTime DataTime { get; set; }
        private static XmlClassHydrogenBurnerConst _con;

        ClassDataPid classPidOut;
        public ClassDataPid ClassPidOut
        {
            get
            {
                return classPidOut;
            }
            set
            {
                if (classPidOut == null)
                    classPidOut = new ClassDataPid(0, 0, _con.Pid);

                classPidOut.CurrValue = value.CurrValue;
                classPidOut.DeltaValue = value.DeltaValue;
                classPidOut.Edif = value.Edif;
                classPidOut.Eint = value.Eint;
                classPidOut.Eprop = value.Eprop;
                classPidOut.SetupValue = value.SetupValue;
            }
        }

        public ClassDataPid DataPid { get; set; }

        public bool UsePid => DataPid != null ? true : false;
        public double SetPower { get; set; }


        public double TdFire;
        /// <summary>
        /// Состояние борботера
        /// </summary>
        public bool UseHydrogen;

        public double TdBurner;

        public bool StateFire;

        public bool StateWater;

        public bool StateRelay;

        public bool StateHeat;

        private double setupTemp;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Температура, для установки
        /// </summary>
        public double SetupTemp
        {
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
        public override string HeaderStr =>
            $"{"Дата",-20}{"Время",-20}{"Температура заданная",-20}{"Температура сичтанная",20}{"Нагрев",-20}{"Реле нагрева",-20}{"Датчик пламени",-20}{"Водяное охлаждение",-20}";

        public override string ToString() =>
                    $"{CurrDate.ToString(),-20}{TimeStep,-20:0.00}{SetupTemp,-20:0.00}{classPidOut.CurrValue,-20:0.00}{UseHydrogen,-20}{StateRelay,-20}{StateFire,-20}{StateWater,-20}";
    }
}
