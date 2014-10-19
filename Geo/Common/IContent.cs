using System;
using System.Threading.Tasks;

namespace Geo.Common
{
	interface IContent
	{
		Task InvokeAsync(Action action);
		Task InvokeAsync(Func<Task> action);
	}
}
