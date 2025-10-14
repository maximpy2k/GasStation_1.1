using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GasStation.Elements.ViewModels;
using GasStation.xml.Constant.XmlConst.Elements;

namespace StationImitation.Controllers
{
    public abstract class ClassBaseController
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="addr">Адрес контроллера</param>
        public ClassBaseController(int addr)
        {
            this.addr = addr;
            lastWdtReset = DateTime.Now;
        }
        public ClassBaseController(XmlClassControllerConst contrConst, BaseClassViewControllers viewController)
        {
            this.contrConst = contrConst;
            this.addr = contrConst.Pa;
        }
        /// <summary>
        /// Константы контроллера
        /// </summary>
        protected XmlClassControllerConst contrConst;

        /// <summary>
        /// Адресс контроллера
        /// </summary>
        public int addr;

        /// <summary>
        /// Время задержки ответа
        /// </summary>
        public int AnswerTimeout;

        


        /// <summary>
        /// Ответ на запрос
        /// </summary>
        public string Answer { get; set; }


        #region Wdt Таймер
        /// <summary>
        /// Последний сброс wdt
        /// </summary>
        DateTime lastWdtReset;

        private int wdtTime = 5000;
        public string OkWdt()
        {
            if ((DateTime.Now - lastWdtReset).TotalMilliseconds > wdtTime)
                return "";
            lastWdtReset = DateTime.Now;
            return "";
        }
        public string ResetWdt()
        {
            lastWdtReset = DateTime.Now;
            return $"!{addr:X2}";
        }
        public string SetWdtTime(string quest)
        {
            var strWdtTime = quest.Substring(5);
            var wdtTime = 0;
            if (int.TryParse(strWdtTime, out wdtTime))
                this.wdtTime = wdtTime * 100;
            return $"!{addr:X2}";
        }
        public string ChkWdt()
        {
            var time = (DateTime.Now - lastWdtReset).TotalMilliseconds;
            if (time < wdtTime)
                return $"!{addr:X2}80";
            else
                return $"!{addr:X2}40";
        }

        string TildaCmds(string quest)
        {
            if (quest == "~**")
            {
                OkWdt();
                return "";
            }

            var adr = quest.Substring(1, 2);
            var cmd = quest.Length==4? quest.Substring(3) : quest.Substring(3,2);
            
            switch (cmd)
            {

                case "1":
                    return ResetWdt();

                case "31":
                    return SetWdtTime(quest);

                case "0":
                    return ChkWdt();
                default:
                    return "";
            }
        }
        #endregion
        protected virtual string SharpCmds(string quest)
        {
            return "";
        }
        protected virtual string DogCmds(string quest)
        {
            return "";
        }

        /// <summary>
        /// Входящий запрос
        /// </summary>
        /// <param name="quest">строка запроса</param>
        public virtual void InputQuest(string quest)
        {
            Answer = "";
            Thread.Sleep(AnswerTimeout);

            switch (quest[0].ToString())
            {
                case "~":
                    Answer = TildaCmds(quest);
                    break;
                case "#":
                    Answer = SharpCmds(quest);
                    break;
                case "@":
                    Answer = DogCmds(quest);
                    break;       
            }
        }
    }

}
