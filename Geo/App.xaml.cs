using System;
using Geo.Contents;
using Geo.Contexts;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Geo
{
	public sealed partial class App : Application
    {
        private TransitionCollection transitions;

        public App()
        {
            InitializeComponent();
            Suspending += OnSuspending;
			UnhandledException += App_UnhandledException;
        }

		private async void App_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			e.Handled = true;

			var loader = new Windows.ApplicationModel.Resources.ResourceLoader();
			var title = loader.GetString("UnhandledExceptionTitle");
			var dialog = new MessageDialog(e.Message, title);

			await dialog.ShowAsync();
		}

		protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            var rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                rootFrame.CacheSize = 1;
                rootFrame.Language = Windows.Globalization.ApplicationLanguages.Languages[0];
				Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (rootFrame.ContentTransitions != null)
                {
                    transitions = new TransitionCollection();

					foreach (var c in rootFrame.ContentTransitions)
                        transitions.Add(c);
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;

				var context = new MainPageContext();

				rootFrame.Navigate(typeof(MainPage), context);
            }

            Window.Current.Activate();
        }

        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e)
        {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = transitions ?? new TransitionCollection() { new NavigationThemeTransition() };
            rootFrame.Navigated -= RootFrame_FirstNavigated;
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }
    }
}