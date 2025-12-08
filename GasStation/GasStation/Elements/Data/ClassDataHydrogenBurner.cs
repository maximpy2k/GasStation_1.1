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
            $"{"Дата",-20}{"Время",-10}{"Температура",-10}{"Температура пламени",-10}{"Нагрев",-16}{"Датчик пламени",-16}{"Водяное охлаждение",-16}{"Реле нагрева",-16}";

        public override string ToString() =>
                    $"{CurrDate.ToString(),-20}{TimeStep,-10:0.00}{TdBurner,-10:0.00}{TdFire,-10:0.00}{StateHeat,-10}{StateFire,-10}{StateWater,-10}{StateRelay,-10}";
    }
}
