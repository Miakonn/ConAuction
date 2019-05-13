using System;
using System.Windows.Input;

namespace ConAuction3.Utilities {
    public class MyCommand : ICommand {
        public delegate void ExecuteMethod();
        private readonly Action _execute;
        private readonly Func<bool> _canExecute;

        public MyCommand(Action exec) {
            _execute = exec;
            _canExecute = null;
        }

        public MyCommand(Action exec, Func<bool> canExecute) {
            _execute = exec;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            if (_canExecute != null) {
                return _canExecute();
            }
            return true;
        }

        public void Execute(object parameter) {
            _execute();
        }

        public void RaiseExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged { 
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class ParameterCommand : ICommand {
        public delegate void ExecuteMethod();
        private Action<string> _execute;
        private readonly Func<bool> _canExecute;

        public ParameterCommand(Action<string> exec) {
            _execute = exec;
            _canExecute = null;
        }

        public ParameterCommand(Action<string> exec, Func<bool> canExecute) {
            _execute = exec;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            if (_canExecute != null) {
                return _canExecute();
            }
            return true;
        }

        public void Execute(object parameter) {
            _execute((string)parameter);
        }

        public void RaiseExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

    public class ObjectCommand : ICommand {
        public delegate void ExecuteMethod();
        private readonly Action<object> _execute;
        private readonly Func<bool> _canExecute;

        public ObjectCommand(Action<object> exec) {
            _execute = exec;
            _canExecute = null;
        }

        public ObjectCommand(Action<object> exec, Func<bool> canExecute) {
            _execute = exec;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            if (_canExecute != null) {
                return _canExecute();
            }
            return true;
        }

        public void Execute(object parameter) {
            _execute(parameter);
        }

        public void RaiseExecuteChanged() {
            CommandManager.InvalidateRequerySuggested();
        }

        public event EventHandler CanExecuteChanged {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
    }

}