using Geo.Common;
using Windows.Devices.Geolocation;

namespace Geo.Contexts
{
	public sealed class MainPageContext : ContextBase, INavigationContext
	{
		private IContent _content;
		private Geolocator _geo;
		private string _latitude;
		private string _longitude;
		private string _accuracy;
		private string _status;

		public void OnNavigatedFrom(object parameter)
		{
			if (_geo != null)
			{
				_geo.PositionChanged -= OnPositionChanged;
				_geo.StatusChanged -= OnStatusChanged;
				_geo = null;
			}

			_content = null;
			Latitude = null;
			Longitude = null;
			Accuracy = null;
		}

		public void OnNavigatedTo(object parameter)
		{
			_content = (IContent)parameter;

			if (_geo == null)
			{
				_geo = new Geolocator
				{
					MovementThreshold = 3.0
				};

				_geo.PositionChanged += OnPositionChanged;
				_geo.StatusChanged += OnStatusChanged;
			}
		}

		private async void OnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
		{
			await _content.InvokeAsync(() =>
			{
				Status = args.Status.ToString();
			});
		}

		private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
		{
			await _content.InvokeAsync(() =>
			{
				var position = args.Position;

				Latitude = position.Coordinate.Point.Position.Latitude.ToString();
				Longitude = position.Coordinate.Point.Position.Longitude.ToString();
				Accuracy = position.Coordinate.Accuracy.ToString();
			});
		}

		public string Accuracy
		{
			get { return _accuracy; }
			set { SetProperty(ref _accuracy, value); }
		}

		public string Latitude
		{
			get { return _latitude; }
			private set { SetProperty(ref _latitude, value); }
		}

		public string Longitude
		{
			get { return _longitude; }
			set { SetProperty(ref _longitude, value); }
		}

		public string Status
		{
			get { return _status; }
			set { SetProperty(ref _status, value); }
		}
	}
}
