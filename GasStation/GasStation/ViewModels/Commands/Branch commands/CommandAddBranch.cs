using System;
using System.Linq;
using System.Windows.Input;
using GasStation.xml.Script.Security;

namespace GasStation.ViewModels.Commands
{
    public class CommandAddBranch : ClassCommandBase
    {
        private readonly MainWindowViewModel _model;

        public CommandAddBranch(MainWindowViewModel conditionalModel):base(conditionalModel)
        {
            this._model = conditionalModel;
        }

        public override bool CanExecute(object parameter)
        {
            return ViewMod.ClsScript.Consts.SecuretyConst.CurrUser.Privs.Contains(EnumPriv.ChangeConditionalScript); 
        }

        //public void Refrash()
        //{
        //    CanExecuteChanged?.Invoke(this, null);
        //}

        public override void Execute(object parameter)
        {
            _model.ConditionalView.AddConditionalState();
        }

        //public event EventHandler CanExecuteChanged;
    }
}