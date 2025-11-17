using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
//using ChartApplication.points;
using GasStation.Elements.Data;
using GasStation.ViewModels.Elements;
using GasStation.Mathem.Chamber;
using GasStation.xml.Script.XmlScript.Elements;
using GasStation.Controllers;
using static GasStation.Controllers.ClassController7042;
using GasStation.xml.Const.Elements;
using GasStation.xml.Script.XmlScript;
using GasStation.Status;
using GasStation.xml.Script;
using GasStation.xml.Script.EnumConst;
using ChartApplication.points;
using GasStation.Controllers.DO;
using GasStation.Controllers.ACP;

namespace GasStation.Devices.ControlChamber
{
    public class ClassControlThermoSection : ClassBaseDevices
    {

        /// <summary>
        /// Параметры термосекции
        /// </summary>

        public int NumSec;

        //public bool EmergencyTemp;


        /// <summary>
        /// Данные шага скрипта
        /// </summary>        
        public XmlClassChamberSection ClassChamberSectionStep => (XmlClassChamberSection)_clsDevStep;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="classChamberConst">Класс констант камеры вызывается при запуске скрипта один раз</param>
        /// <param name="classChamberSectionView">Класс вывода данных на форму</param>
        public ClassControlThermoSection(ClassDataTime classDataTime) : base(classDataTime)
        {
        }

        public override void Init()
        {
            FlagStop = false;
            _chamberSetThermo = null;
            base.Init();
        }
        /// <summary>
        /// Вызывается на каждом шаге скрипта
        /// </summary>
        /// <param name="classChamberStep">Параметры шага</param>
        public void DataStep(XmlClassChamberSection classChamberSectionStep, XmlStateConditionScript[] states, XmlClassStepParams stepParams)
        {
            Init();

            _clsDevStep = classChamberSectionStep;
            _states = states;
            _stepParams = stepParams;

            ChamPidRegulation.DataStep(ClassChamberSectionStep.ThermoSectionConst, ClassChamberSectionStep);

            ReadTd();
            LastSetTempIn = LastSetTempIn == -25 ? _tdIn.AverTd : LastSetTempIn;
            LastSetTempOut = LastSetTempOut == -25 ? _tdOut.AverTd : LastSetTempOut;
            _chamberSetThermo = new ClassChamberSetThermo(ClassChamberSectionStep, LastSetTempOut, LastSetTempIn);

            if (File.Exists($"{path}\\Chammber_{ClassChamberSectionStep.Num}.txt"))
                File.Delete($"{path}\\Chammber_{ClassChamberSectionStep.Num}.txt");


        }

        private void ReadTd()
        {
            #region Расчет температуры по внешнему термодатчику
            var uTdOut = GetAcpTdOut();
            TdOut.Add(uTdOut);
            #endregion

            #region Расчет температуры по внутреннему термодатчику, если он есть
            var uTdIn = GetAcpTdIn();
            TdIn.Add(uTdIn);
            #endregion           
        }

        /// <summary>
        /// Переход на новый шаг цикла работы внутри комманды скрипта
        /// </summary>
        protected override void NextStepFunc()
        {
            ClassDataChamber classDataChamber = new ClassDataChamber(_classDataTime, ClassChamberSectionStep.ThermoSectionConst);

            ReadTd();

            #region Расчет заданой температуры
            double setTemp = _chamberSetThermo.NextStep(_classDataTime);
            #endregion

            LastSetTempIn = setTemp;
            LastSetTempOut = setTemp;


            var curr = _chamPidRegulation.NextStep(_classDataTime, _tdOut.AverTd, _tdIn.AverTd, setTemp);
            curr.SetupTemp = ClassChamberSectionStep.SetupTemp;
            CheckStatus(curr);

            //var val = curr.SetPower << 1;
            //SetValue(val);
            
            SetValue(curr.SetPower);


            if (AddData(curr))
            {
                ClassChamberSectionStep.ThermoSectionView.SeriesTdOut.PointsPrepare.Clr(Cnt);
                ClassChamberSectionStep.ThermoSectionView.SeriesCurrSetTemp.PointsPrepare.Clr(Cnt);
                if (curr.ClassPidIn != null)
                    ClassChamberSectionStep.ThermoSectionView.SeriesTdIn.PointsPrepare.Clr(Cnt);
            }


            #region Вывод на форму  

            ClassChamberSectionStep.ThermoSectionView.DataChamber = curr;
            ClassChamberSectionStep.ThermoSectionView.SeriesTdOut.Add(new PointTime(curr.CurrDate, curr.ClassPidOut.CurrValue));

            if (curr.ClassPidIn != null)
                ClassChamberSectionStep.ThermoSectionView.SeriesTdIn.Add(new PointTime(curr.CurrDate, curr.ClassPidIn.CurrValue));
            if (ClassChamberSectionStep.UsePid)
                ClassChamberSectionStep.ThermoSectionView.SeriesCurrSetTemp.Add(new PointTime(curr.CurrDate, curr.ClassPidIn.SetupValue));
            else
                ClassChamberSectionStep.ThermoSectionView.SeriesCurrSetTemp.Add(new PointTime(curr.CurrDate, curr.ClassPidOut.SetupValue));
            #endregion
        }

        private ClassBaseTd _tdOut;
        /// <summary>
        /// Данные по внешнему термодатчику
        /// </summary>
        protected ClassBaseTd TdOut
        {
            get
            {
                if (_tdOut != null)
                    return _tdOut;
                _tdOut = new ClassBaseTd(ClassChamberSectionStep.ThermoSectionConst.Td[0]);
                return _tdOut;
            }
        }

        private ClassBaseTd _tdIn;
        /// <summary>
        /// Данные по внутреннему термодатчику
        /// </summary>
        protected ClassBaseTd TdIn
        {
            get
            {
                if (_tdIn != null)
                    return _tdIn;
                _tdIn = new ClassBaseTd(ClassChamberSectionStep.ThermoSectionConst.Td[1]);
                return _tdIn;
            }
        }


        private ChamberPidRegulation _chamPidRegulation;
        /// <summary>
        /// Класс пид регулятора
        /// </summary>
        private ChamberPidRegulation ChamPidRegulation
        {
            get
            {
                if (_chamPidRegulation != null)
                    return _chamPidRegulation;
                _chamPidRegulation = new ChamberPidRegulation();
                return _chamPidRegulation;
            }
        }

        private double LastSetTempIn = -25;
        private double LastSetTempOut = -25;

        /// <summary>
        /// Класс задания температуры
        /// </summary>
        private ClassChamberSetThermo _chamberSetThermo;


        /// <summary>
        /// Чтение АЦП внешнего термодатчика
        /// </summary>
        /// <returns>Температуа внешнего термодатчика</returns>
        public double GetAcpTdOut()
        {
            var contr = LstContr[ClassChamberSectionStep.ThermoSectionConst.Td[0].MasAcpConst[0].ContrNum] as BaseAcpController;
            return contr.AcpValues[ClassChamberSectionStep.ThermoSectionConst.Td[0].MasAcpConst[0].Port];
        }
        /// <summary>
        /// Чтение АЦП внутреннего термодатчика
        /// </summary>
        /// <returns>температура внутреннего термодатчика</returns>
        public double GetAcpTdIn()
        {
            var contr = LstContr[ClassChamberSectionStep.ThermoSectionConst.Td[1].MasAcpConst[0].ContrNum] as BaseAcpController;
            return contr.AcpValues[ClassChamberSectionStep.ThermoSectionConst.Td[1].MasAcpConst[0].Port];
        }

        public void SetValue(double setValue)
        {
            if (setValue < 0)
                setValue = 0;

            var cap = ClassChamberSectionStep.ThermoSectionConst.CapConst;
            BaseDOController doController;

            if (cap == null)
            {
                var k = (ClassChamberSectionStep.ThermoSectionConst.MinKey - ClassChamberSectionStep.ThermoSectionConst.MaxKey) / (0.0 - 100.0);
                var b = ClassChamberSectionStep.ThermoSectionConst.MinKey - k * 0.0;
                //var setVal= ((int)(setValue * 100 * k + b)) << 1;
                var setVal = ((int)(setValue * 100 * k + b)) > ClassChamberSectionStep.ThermoSectionConst.MaxKey ? ClassChamberSectionStep.ThermoSectionConst.MaxKey : ((int)(setValue * 100 * k + b));
                doController = LstContr[ClassChamberSectionStep.ThermoSectionConst.ContrNum] as BaseDOController;
                doController.SetValue((int)setVal);
                return;
            }
            var capController = LstContr[cap.ContrNum] as ClassController87024;

            
            var setVal1 = cap.GetValue(setValue * 100);

            capController.SetCap(setVal1, cap.Port);

            //var contr = LstContr[ClassChamberSectionStep.ThermoSectionConst.ContrNum] as BaseDOController;
        }

        public void CheckStatus(ClassDataChamber curr)
        {
            return;
            //#region Проверка температуры (газ)
            //if (NumSec != 1)
            //    return;
            //var States = _states.Where(dat => dat.DevName == "Термосекция(Газ)").ToList();
            //var currValue = ClassChamberSectionStep.UsePid ? curr.ClassPidIn.CurrValue : curr.ClassPidOut.CurrValue;

            //if (ClassChamberSectionStep.UsePid)
            //    currValue = curr.ClassPidIn.CurrValue;
            //if (!ClassChamberSectionStep.UsePid)
            //    currValue = curr.ClassPidOut.CurrValue;

            //for (int i = 0; i < States.Count; i++)
            //{
            //    if ((curr.DataTime.TimeStep > States[i].Timer) && ((currValue < States[i].ValueBegin) || (currValue > States[i].ValueEnd)))
            //    {
            //        EmergencyTemp = true;
            //        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.DataTime.TimeStep);
            //        conJumpArgs.NumDev = ClassChamberSectionStep.Num;
            //        conJumpArgs.NameDev = "Термосекция(Газ)";
            //        conJumpArgs.CurrValue = currValue;
            //        conJumpArgs.TextError = "Значение температуры за границей допустимого диапазона";
            //        conJumpArgs.Conditional = States[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
                    
            //        StateError?.Invoke(this, conJumpArgs);
            //    }
            //}
            //#endregion

            //#region Проверка температуры (Центр)
            //if (NumSec != 1)
            //    return;
            //List<XmlStateConditionScript> States_SecCent = _states.Where(dat => dat.DevName == "Термосекция(Центр)").ToList();
            //double currValue_States_SecCent = 0;
            //if (ClassChamberSectionStep.UsePid)
            //    currValue_States_SecCent = curr.ClassPidIn.CurrValue;
            //if (!ClassChamberSectionStep.UsePid)
            //    currValue_States_SecCent = curr.ClassPidOut.CurrValue;

            //for (int i = 0; i < States_SecCent.Count; i++)
            //{
            //    if ((curr.DataTime.TimeScript > States_SecCent[i].Timer) && ((currValue_States_SecCent < States_SecCent[i].ValueBegin) || (currValue_States_SecCent > States_SecCent[i].ValueEnd)))
            //    {
            //        EmergencyTemp = true;
            //        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
            //        conJumpArgs.NumDev = ClassChamberSectionStep.Num;
            //        conJumpArgs.NameDev = "Термосекция(Центр)";
            //        conJumpArgs.CurrValue = currValue_States_SecCent;
            //        conJumpArgs.TextError = "Значение температуры за границей допустимого диапазона";
            //        conJumpArgs.Conditional = States_SecCent[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
            //        Console.WriteLine(DateTime.Now);
            //        StateError?.Invoke(this, conJumpArgs);
            //    }
            //}
            //#endregion

            //#region Проверка температуры (Загрузчик)
            //if (NumSec != 1)
            //    return;
            //List<XmlStateConditionScript> States_SecLoad = _states.Where(dat => dat.DevName == "Термосекция(Загр.)").ToList();
            //double currValue_States_SecLoad = 0;
            //if (ClassChamberSectionStep.UsePid)
            //    currValue_States_SecLoad = curr.ClassPidIn.CurrValue;
            //if (!ClassChamberSectionStep.UsePid)
            //    currValue_States_SecLoad = curr.ClassPidOut.CurrValue;

            //for (int i = 0; i < States_SecLoad.Count; i++)
            //{
            //    if ((curr.DataTime.TimeScript > States_SecLoad[i].Timer) && ((currValue_States_SecLoad < States_SecLoad[i].ValueBegin) || (currValue_States_SecLoad > States_SecLoad[i].ValueEnd)))
            //    {
            //        EmergencyTemp = true;
            //        ConJumpArgs conJumpArgs = new ConJumpArgs(curr.TimeStep);
            //        conJumpArgs.NumDev = ClassChamberSectionStep.Num;
            //        conJumpArgs.NameDev = "Термосекция(Загр.)";
            //        conJumpArgs.CurrValue = currValue_States_SecLoad;
            //        conJumpArgs.TextError = "Значение температуры за границей допустимого диапазона";
            //        conJumpArgs.Conditional = States_SecLoad[i].NumStep;
            //        conJumpArgs.TypeConditional = TypeConditional.Error;
            //        Console.WriteLine(DateTime.Now);
            //        StateError?.Invoke(this, conJumpArgs);
            //    }
            //}
            //#endregion
        }
    }
}
