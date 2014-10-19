namespace Geo.Common
{
	public interface INavigationContext
	{
		void OnNavigatedTo(object parameter);
		void OnNavigatedFrom(object parameter);
	}
}
