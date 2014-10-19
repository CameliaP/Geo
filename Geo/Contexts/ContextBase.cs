using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Geo.Utils;

namespace Geo.Contexts
{
	public abstract class ContextBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged(string propertyName)
		{
			PropertyChanged.SafeInvoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected bool SetProperty<T>(ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
		{
			if (EqualityComparer<T>.Default.Equals(oldValue, newValue))
				return false;

			oldValue = newValue;
			OnPropertyChanged(propertyName);

			return true;
		}
	}
}
