using System;
using System.Windows.Input;

namespace UserManager.Core.Common
{
    public class RelayCommand : ICommand
    {
        private Action action;
        private Func<bool> canExecuteMethod;
        public event EventHandler CanExecuteChanged;

        public RelayCommand(Action action, Func<bool> canExecuteMethod)
        {
            this.action = action;
            this.canExecuteMethod = canExecuteMethod;
        }

        #region ICommand
        public bool CanExecute(object parameter)
        {
            return canExecuteMethod();
        }

        public void Execute(object parameter)
        {
            action();
        }
        #endregion
    }
}
