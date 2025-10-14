using GasStation.xml.Script.Security;
using Microsoft.Win32;
using System.Linq;

namespace GasStation.ViewModels.Commands
{
    /// <summary>
    /// Сохранение констант в файл xml
    /// </summary>
    public class CommandSaveScript : ClassCommandBase
    {
        /// <summary>
        /// Класс комманды добавления нового шага
        /// </summary>
        public CommandSaveScript(MainWindowViewModel viewMod):base(viewMod)
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
            var path = "";

            SaveFileDialog saveDlg = new SaveFileDialog();
            saveDlg.Filter = "*.xml|*.xml";

            if (saveDlg.ShowDialog() == false)
                return;

            path = saveDlg.FileName;
            
            ViewMod.ClsScript.Save(path);
        }
    }
}