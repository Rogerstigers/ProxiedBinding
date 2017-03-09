using System;
using System.Windows.Input;

namespace Demo
{
    public class DelegateCommand:ICommand
    {
        private readonly Action _executeMethod;

        public DelegateCommand(Action executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod.Invoke();
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public class DelegateCommand<T>:ICommand
    {
        private readonly Action<T> _executeMethod;

        public DelegateCommand(Action<T> executeMethod)
        {
            _executeMethod = executeMethod;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _executeMethod.Invoke((T)parameter);
        }

        public event EventHandler CanExecuteChanged;

        protected virtual void OnCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

    }
}
