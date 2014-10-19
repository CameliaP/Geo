using System.ComponentModel;

namespace Geo.Utils
{
	public static class Extensions
	{
		public static void SafeInvoke(this PropertyChangedEventHandler action, object sender, PropertyChangedEventArgs e)
		{
			if (action != null)
				action(sender, e);
		}
	}
}
