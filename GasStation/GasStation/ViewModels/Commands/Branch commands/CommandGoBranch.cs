using System;
using System.Windows.Input;
using GasStation.xml.Script;

namespace GasStation.ViewModels.Commands
{
    /// <summary>
    ///Комманда для открытия окна условного перехода
    /// </summary>
    public class CommandGoBranch : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public CommandGoBranch(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object parameter)
        {

            return true;
        }

        /// <summary>
        /// Выполнение команды
        /// </summary>
        /// <param name="parameter">Узел XML</param>
        public void Execute(object parameter)
        {
            var xmlStepParams = (XmlClassStep)parameter;

            //ConditionalWViewModel conViewModel = new ConditionalWViewModel(_viewModel);

            if (_viewModel.ConditionalView != null)
            {
                _viewModel.ConditionalView.Update(_viewModel, xmlStepParams);

                ConditionalWindow conditionalWindow = new ConditionalWindow {DataContext = _viewModel.ConditionalView };

                conditionalWindow.ShowDialog();
            }


        }

        public event EventHandler CanExecuteChanged;
    }
}