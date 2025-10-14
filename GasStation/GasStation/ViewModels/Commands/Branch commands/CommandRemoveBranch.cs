using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GasStation.xml.Script.Security;

namespace GasStation.ViewModels.Commands
{
    public class CommandRemoveBranch : ClassCommandBase
    {
        private MainWindowViewModel model;

        public CommandRemoveBranch(MainWindowViewModel conditionalModel):base(conditionalModel)
        {
            this.model = conditionalModel;
        }

        public override bool CanExecute(object parameter)
        {
            return ViewMod.ClsScript.Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeConditionalScript);
        }

        public override void Execute(object parameter)
        {
            model.ConditionalView.RemoveConditionalState();
        }

        //public event EventHandler CanExecuteChanged;
    }
}
