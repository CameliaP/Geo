using System;
using System.Threading.Tasks;
using Geo.Common;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace Geo.Contents
{
	public abstract class ContentBase : Page, IContent
	{
		private readonly CoreDispatcher _dispatcher;

		public ContentBase()
		{
			_dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
		}

		public async Task InvokeAsync(Action action)
		{
			await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => action());
		}

		public async Task InvokeAsync(Func<Task> action)
		{
			await _dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => await action());
		}
	}
}
