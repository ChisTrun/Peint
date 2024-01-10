using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Custom_Paint.Commands
{
    public class SimpleCommand : ICommand
    {
        private readonly Action<object?> _execute;

        public event EventHandler? CanExecuteChanged;

        public SimpleCommand(Action<object?> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            this._execute.Invoke(parameter);
        }
    }
}
