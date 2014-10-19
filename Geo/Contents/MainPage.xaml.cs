using Geo.Common;
using Windows.UI.Xaml.Navigation;

namespace Geo.Contents
{
	public sealed partial class MainPage
    {
		public MainPage()
        {
            InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Required;
		}

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
			DataContext = e.Parameter;

			var navigationContext = e.Parameter as INavigationContext;

			if (navigationContext == null)
				return;

			navigationContext.OnNavigatedTo(this);			
        }

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);

			var navigationContext = e.Parameter as INavigationContext;

			if (navigationContext == null)
				return;

			navigationContext.OnNavigatedFrom(e.Parameter);
		}
	}
}
