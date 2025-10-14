using GasStation.xml.Script.Security;
using System.Linq;

namespace GasStation.ViewModels.Commands
{
    /// <summary>
    /// Шаг комманды удаления шага
    /// </summary>
    public class CommandRemoveStep:ClassCommandBase
    {
        public CommandRemoveStep(MainWindowViewModel viewMod):base(viewMod)
        {
        }
        
        public override bool CanExecute(object parameter)
        {
            return ViewMod.ClsScript.Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeScript);
        }

        public override void Execute(object parameter)
        {
            ViewMod.ClsScript.DeleteStep();
        }
    }
}
