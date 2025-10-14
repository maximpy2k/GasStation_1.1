using GasStation.xml.Const.Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GasStation.Elements.Data
{
    public class ClassDataPid:INotifyPropertyChanged
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="currValue">Измеренное значение величины</param>
        /// <param name="setupValue">Заданное значение величины</param>
        public ClassDataPid(double currValue, double setupValue,XmlClassPidConst con)
        {
            XmlConst = con;
            SetupValue = setupValue;
            CurrValue = currValue;            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
        private double setupValue;
        /// <summary>
        ///Заданное значение величины
        /// </summary>
        public double SetupValue
        {
            get { return setupValue; }
            set
            {
                setupValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SetupValue"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("E"));
            }
        }
        
        private double currValue;
        /// <summary>
        /// Измеренное значение величины
        /// </summary>
        public double CurrValue
        {
            get { return currValue; }
            set
            {
                currValue = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrValue"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("E"));
            }
        }
             
        /// <summary>
        /// Ошибка наблюдаемой величины
        /// </summary>
        public double E { get { return SetupValue - CurrValue; } }

        private double edif;
        /// <summary>
        /// Дифференциальная часть ПИД-регулятора
        /// </summary>
        public double Edif
        {
            get { return edif; }
            set
            {
                edif = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Edif"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SumErr"));
            }
        }

        private double eint;
        /// <summary>
        /// Интегральная часть ПИД-регулятора
        /// </summary>
        public double Eint
        {
            get { return eint; }
            set
            {
                //if (!XmlConst.EnabledChangedScript)
                //{
                //    return;
                //}
                eint = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Eint"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SumErr"));
            }
        }

        private double eprop;
        /// <summary>
        /// Пропорциональная часть ПИД-регулятора
        /// </summary>
        public double Eprop
        {
            get { return eprop; }
            set
            {
                //if (!XmlConst.EnabledChangedScript)
                //{
                //    return;
                //}
                eprop = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Eprop"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("SumErr"));
            }
        }

        /// <summary>
        /// Суммарная ошибка на текущем шаге
        /// </summary>
        public double SumErr
        {
            get
            {
                return E + Edif + Eint;
            }
        }

        
        
        public XmlClassPidConst XmlConst { get; set; }

        private double deltaValue;
        /// <summary>
        /// Воздействие на текущую величину
        /// </summary>
        public double DeltaValue
        {
            get { return deltaValue; }
            set
            {
                deltaValue = value;
                //Power = value*100;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DeltaPlus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DeltaMinus"));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("DeltaValue"));
                //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Power"));
            }
        }

        /// <summary>
        /// Значение для отображения положительной мощности
        /// </summary>
        public double DeltaPlus
        {
            get
            {
                if (DeltaValue >=0)
                {
                    return DeltaValue;
                }
                else
                {
                    return 0;
                }
            }
        }
        /// <summary>
        /// Значение для отображения отрицательной мощности
        /// </summary>
        public double DeltaMinus
        {
            get
            {
                if (DeltaValue < 0)
                {
                    return -DeltaValue;
                }
                else
                {
                    return 0;
                }
            }
        }

        //private double _power;
        ///// <summary>
        ///// Значение мощности в процентах
        ///// </summary>
        //public double Power
        //{
        //    get { return _power; }
        //    set
        //    {
        //        _power = value;
        //        //PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Power"));
        //    }
        //}

        public string Str
        {
            get
            { 
                return $"{SetupValue  ,-30:0.00}{CurrValue,  -18:0.00}{ E,-23:0.00000}{Edif,-26:0.00000}{Eint,-31:0.00000}{Eprop,-30:0.00000}{DeltaValue,-33:0.0000}{SumErr,-11:0.0000}";

            }
        }
        public  string HeaderStr =>
                                   $"{"Установленное значение"} {"Текущее значение"}    {"Линейная составляющая"}   {"Диференциальная составляющая"}    {"Интегральная составляющая"}   {"Пропорциональная составляющая"}   {"Разность установленной и текущей"}    {"Суммарная ошибка"}";
        public override string ToString()
        {
            return Str;
        }

        //public bool Gogo { get; set; } = false;

        public bool EnabledChangedScript => false;
    }
}
