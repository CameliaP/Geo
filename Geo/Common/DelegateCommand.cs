using System;
using System.Windows.Input;

namespace Geo.Common
{
	public class DelegateCommand : ICommand
	{
		private readonly Action<object> _executeAction;
		private readonly Func<object, bool> _canExecuteFunc;

		public event EventHandler CanExecuteChanged;

		public DelegateCommand(Action<object> executeAction, Func<object, bool> canExcecuteFunc = null)
		{
			if (executeAction == null)
				throw new ArgumentNullException(nameof(executeAction));

			_executeAction = executeAction;
			_canExecuteFunc = canExcecuteFunc;
		}

		public bool CanExecute(object parameter)
		{
			if (_canExecuteFunc == null)
				return true;

			return _canExecuteFunc(parameter);
		}

		public void Execute(object parameter)
		{
			_executeAction(parameter);
		}

		public void RaiseExecuteChanged()
		{
			if (CanExecuteChanged != null)
				CanExecuteChanged(this, new EventArgs());
		}
	}
}
