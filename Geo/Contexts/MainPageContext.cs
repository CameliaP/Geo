using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Geo.Common;
using Windows.Devices.Geolocation;

namespace Geo.Contexts
{
	public sealed class MainPageContext : ContextBase, INavigationContext
	{
		private readonly DelegateCommand _commandRefresh;

		private IContent _content;
		private Geolocator _geo;
		private string _latitude;
		private string _longitude;
		private string _accuracy;
		private string _status;
		private bool _isRefreshing;

		public MainPageContext()
		{
			_commandRefresh = new DelegateCommand(CommandExecuteRefresh, CommandPredicateRefresh);
		}

		private bool CommandPredicateRefresh(object arg)
		{
			return (_geo != null) && (_geo.LocationStatus == PositionStatus.Ready) && !_isRefreshing;
		}

		private async void CommandExecuteRefresh(object obj)
		{
			await PopulateData();
		}

		private async Task PopulateData()
		{
			IsRefreshing = true;

			var cts = new CancellationTokenSource();

			try
			{
				var position = await _geo.GetGeopositionAsync().AsTask(cts.Token);

				Latitude = position.Coordinate.Point.Position.Latitude.ToString();
				Longitude = position.Coordinate.Point.Position.Longitude.ToString();
				Accuracy = position.Coordinate.Accuracy.ToString();
			}
			catch (TaskCanceledException)
			{
			}
			finally
			{
				cts = null;
				IsRefreshing = false;
			}
		}

		public void OnNavigatedFrom(object parameter)
		{
			if (_geo != null)
			{
				//_geo.PositionChanged -= OnPositionChanged;
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
					DesiredAccuracyInMeters = 10,
					ReportInterval = 1,
					MovementThreshold = 1d
				};

				//_geo.PositionChanged += OnPositionChanged;
				_geo.StatusChanged += OnStatusChanged;
			}
		}

		private async void OnStatusChanged(Geolocator sender, StatusChangedEventArgs args)
		{
			await _content.InvokeAsync(() =>
			{
				Status = GetStatusString(args.Status);
			});
		}

		private string GetStatusString(PositionStatus status)
		{
			var loader = new Windows.ApplicationModel.Resources.ResourceLoader();

			switch (status)
			{
				case PositionStatus.Ready:
					return loader.GetString("PositionStatusReady");
				case PositionStatus.Initializing:
					return loader.GetString("PositionStatusInitializing");
				case PositionStatus.NoData:
					return loader.GetString("PositionStatusNoData");
				case PositionStatus.Disabled:
					return loader.GetString("PositionStatusDisabled");
				case PositionStatus.NotInitialized:
					return loader.GetString("PositionStatusNotInitialized");
				case PositionStatus.NotAvailable:
					return loader.GetString("PositionStatusNotAvailable");
				default:
					throw new NotSupportedException("Value: " + status);
			}
		}

		//private async void OnPositionChanged(Geolocator sender, PositionChangedEventArgs args)
		//{
		//	await _content.InvokeAsync(() =>
		//	{
		//		var position = args.Position;

		//		Latitude = position.Coordinate.Point.Position.Latitude.ToString();
		//		Longitude = position.Coordinate.Point.Position.Longitude.ToString();
		//		Accuracy = position.Coordinate.Accuracy.ToString();
		//	});
		//}

		public string Accuracy
		{
			get { return _accuracy; }
			private set { SetProperty(ref _accuracy, value); }
		}

		public ICommand CommandRefresh
		{
			get { return _commandRefresh; }
		}

		public bool IsRefreshing
		{
			get { return _isRefreshing; }
			private set
			{
				if (SetProperty(ref _isRefreshing, value))
					_commandRefresh.RaiseCanExecuteChanged();
			}
		}

		public string Latitude
		{
			get { return _latitude; }
			private set { SetProperty(ref _latitude, value); }
		}

		public string Longitude
		{
			get { return _longitude; }
			private set { SetProperty(ref _longitude, value); }
		}

		public string Status
		{
			get { return _status; }
			private set
			{
				if (SetProperty(ref _status, value))
					_commandRefresh.RaiseCanExecuteChanged();
			}
		}
	}
}
