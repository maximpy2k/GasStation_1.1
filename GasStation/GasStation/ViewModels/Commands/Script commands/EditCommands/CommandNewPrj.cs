using GasStation.xml;
using System;
using System.Linq;
using GasStation.xml.Script.Security;
using GasStation.Elements.Data;

namespace GasStation.ViewModels.Commands
{
    /// <summary>
    /// Класс комманды создания нового проекта
    /// </summary>
    public class CommandNewPrj : ClassCommandBase
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="viewMod">Ссылка на ViewMod</param>
        public CommandNewPrj(MainWindowViewModel viewMod):base(viewMod)
        {            
        }

        /// <summary>
        /// Функцчия изменения состояния комманды
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns>Состояние коммманды</returns>
        public override bool CanExecute(object parameter)
        {
            return ViewMod.ClsScript.Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeScript);
        }
        /// <summary>
        /// Функция комманды
        /// </summary>
        /// <param name="parameter"></param>
        public override void Execute(object parameter)
        {
            if (ViewMod.ClsScript != null)
                ViewMod.MessageOut = ViewMod.ClsScript.ViewData.MessageOut;
            var currUser = ViewMod.ClsScript.Consts.SecuretyConst.CurrUser;

            ViewMod.ClsScript = new ClassScript(ClassScript.DefaultConstPath);

            ViewMod.ClsScript.Consts.SecuretyConst.CurrUserName = currUser.UserName;
            ViewMod.UpdateCanExecuteCommands();
            ViewMod.UpdateClsScript();

            ViewMod.ClsScript.CurrStep.StepParams.NameStep = "Режим ожидания";
            var message = $"Создание нового файла техпроцесса{Environment.NewLine}";

            var log = new ClassDataMainLog(DateTime.Now.ToLongTimeString(), message);

            ViewMod.MessageOut += log.ToString();
            ViewMod.ClsScript.ViewData.MessageOut = ViewMod.ClsScript.ViewData.MessageOut.Insert(ViewMod.ClsScript.ViewData.MessageOut.Count(), ViewMod.MessageOut);
            log.AppendToFile($"{ViewMod.ClsScript.Consts.ChannelConsts.LogPathFull}\\MainLog.txt");

            ViewMod.UpdateCanExecuteCommands();

        }
    }
}