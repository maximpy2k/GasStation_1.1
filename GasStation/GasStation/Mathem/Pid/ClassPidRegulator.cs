using GasStation.Elements.Data;
using GasStation.xml.Const.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GasStation.Mathem.Pid
{
    public class ClassPidRegulator
    {
        /// <summary>
        /// Конструктро класса
        /// </summary>
        /// <param name="pidConst">Константы ПИД регулятора</param>
        public ClassPidRegulator(XmlClassPidConst pidConst)
        {
            _pidConst = pidConst;
        }

        /// <summary>
        /// Константы ПИД регулятора
        /// </summary>
        XmlClassPidConst _pidConst;

        /// <summary>
        /// Данные ПИД регулятора
        /// </summary>
        //List<ClassDataPid> ListPidData=new List<ClassDataPid>();

        /// <summary>
        /// Данные последнего шага
        /// </summary>
        private ClassDataPid _lastPidData;
        //{
        //    get { return ListPidData.Count != 0 ? ListPidData.Last() : null; }
        //}

        /// <summary>
        /// Новый шаг скрипта
        /// </summary>
        public void DataStep()
        {
            //if (ListPidData.Count == 0)
            //    return;
            //var last = _lastPidData;
            //ListPidData = new List<ClassDataPid>();
            //ListPidData.Add(last);
        }

        /// <summary>
        /// Новый шаг работы Пид регулятора
        /// </summary>
        /// <param name="currValue">Измеренное значение</param>
        /// <param name="setupValue">Установленное значение</param>
        /// <returns>Данные по шагу работы ПИД регулятора</returns>
        public ClassDataPid NextStep(double currValue, double setupValue)
        {
            if (_lastPidData == null)
                _lastPidData = new ClassDataPid(currValue, setupValue, _pidConst);

            var pidData = new ClassDataPid(currValue, setupValue, _pidConst);

            pidData.Edif = _pidConst.Td * (pidData.E - _lastPidData.E);

            #region Интегральная составляющая
            if (Math.Abs(_lastPidData.SumErr) < _pidConst.Di)
                pidData.Eint = Math.Abs(_pidConst.Ti) > 0.0000001 ? _lastPidData.Eint + pidData.E / _pidConst.Ti : 0.0;
            else
                pidData.Eint = _lastPidData.Eint;
            #endregion

            pidData.Eprop = pidData.E;

            //if (Math.Abs(pidData.E) > _pidConst.Di)
            //  pidData.Eint = 0;

            if (Math.Abs(pidData.SumErr) > _pidConst.Di)
                pidData.DeltaValue = Math.Sign(pidData.SumErr) * _pidConst.MaxImpact;
            else
                pidData.DeltaValue = pidData.SumErr * _pidConst.Kp;

            //ListPidData.Add(pidData);
            _lastPidData = pidData;
            return pidData;
        }
    }
}
