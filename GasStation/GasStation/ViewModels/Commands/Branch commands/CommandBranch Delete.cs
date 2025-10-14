//using System;
//using System.Windows.Input;

//namespace GasStation.ViewModels.Commands
//{
//    public class CommandBranch : ICommand
//    {
//        public bool CanExecute(object parameter)
//        {
//            return true;
//        }

//        public void Refrash()
//        {
//            CanExecuteChanged?.Invoke(this, null);
//        }

//        public void Execute(object parameter)
//        {
//            ConditionalWindow conditionalWindow = new ConditionalWindow();
//            conditionalWindow.DataContext = parameter;
//            conditionalWindow.ShowDialog();
//        }

//        public event EventHandler CanExecuteChanged;
//    }
//}