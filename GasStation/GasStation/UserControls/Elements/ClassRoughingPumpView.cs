using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChartApplication.points;
using GasStation.xml.Const.Elements;

namespace GasStation.ViewModels.Elements
{
    public class ClassRoughingPumpView
    {
        private XmlClassPumpConst _const;

        public ClassRoughingPumpView(XmlClassPumpConst consts)
        {
            _const = consts;
            WorkUseRoughingPump = false;
        }

        private bool workUseRoughingPump;
        /// <summary>
        /// Реальная работа форвакуумного насоса
        /// </summary>
        public bool WorkUseRoughingPump
        {
            get { return workUseRoughingPump; }
            set
            {
                workUseRoughingPump = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("WorkUseRoughingPump"));
            }
        }

        public ClassFlapView FlapView { get; set; }

        public string BigName { get; set; }
        ///// <summary>
        ///// Класс отображения данных вакуметра
        ///// </summary>
        //public ClassvVacuumetrView VacuumetrView { get; set; }

        /// <summary>
        /// Возможность проверки привелегий
        /// </summary>
        public bool UsePriv { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
