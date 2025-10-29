using GasStation.xml.Const.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GasStation.Elements.Data
{
    public class ClassDataBubler:ClassDataBase,INotifyPropertyChanged
    {

        public ClassDataBubler(ClassDataTime dataTime) : base(dataTime)
        {            
        }
        public ClassDataBubler(ClassDataTime dataTime, XmlClassBubblerConst con) : base(dataTime)
        {
            DataTime = dataTime;
            CurrDate = dataTime.BeginCycleStep;
            _con = con;
            ClassPidOut = new ClassDataPid(0, 0, con.Pid);
        }
        /// <summary>
        /// Класс данных времени
        /// </summary>
        public ClassDataTime DataTime { get; set; }
        private static XmlClassBubblerConst _con;
        public bool StateHeat;
        /// <summary>
        /// Время измерения
        /// </summary>

        private double setupTemp;

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
                    classPidOut = new ClassDataPid(0, 0, _con.Pid);

                classPidOut.CurrValue = value.CurrValue;
                classPidOut.DeltaValue = value.DeltaValue;
                classPidOut.Edif = value.Edif;
                classPidOut.Eint = value.Eint;
                classPidOut.Eprop = value.Eprop;
                classPidOut.SetupValue = value.SetupValue;
            }
        }
        /// <summary>
        /// Состояние борботера
        /// </summary>
        public bool Relay;

        /// <summary>
        /// Состояние борботера
        /// </summary>
        public bool UseBubbler;
        /// <summary>
        /// Текущая температура
        /// </summary>
        public double CurrTd;

        /// <summary>
        /// Заданная температура
        /// </summary>
        public double SetupValue;

        public double CurrSetTemp;

        /// <summary>
        /// Данные ПИД регулятора
        /// </summary>
        public ClassDataPid DataPid { get; set; }

        public bool UsePid =>DataPid != null?true:false;
        


        /// <summary>
        /// Конвертация в массив
        /// </summary>
        public override byte[] ToByteMas
        {
            get
            {
                byte[] val = new byte[25];
                val[0] = FirstByte;
                BitConverter.GetBytes(TimeStep).CopyTo(val, 1);
                BitConverter.GetBytes(Relay).CopyTo(val, 9);
                BitConverter.GetBytes(CurrTd).CopyTo(val, 17);
                BitConverter.GetBytes(SetupValue).CopyTo(val, 25);
                return val;
            }
        }
        /// <summary>
        /// Конвертация в строку
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{CurrDate.ToString(),-20}{TimeStep,-16:0.00}{((UseBubbler == true) ? "Используется" : "Не используется"),-16}{((Relay == true) ? "Реле вкл" : "Реле выкл"),-16}{CurrTd,-16:0.00}{SetupValue,-16:0.00}";
        }
        /// <summary>
        /// Заголовок текстового файла
        /// </summary>
        public override string HeaderStr => $"{"Дата",-20}{"Время",-16:0.00}{"Состояние борботера",-16}{"Состояние реле",-16}{"Текущая температура",-16:0.00}{"Заданная температура",-16:0.00}";
        
        
        public event PropertyChangedEventHandler PropertyChanged;
    }

}
