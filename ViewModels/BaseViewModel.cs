using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CarSearch
{
	public class BaseViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;
		protected Lazy<CarRestService> carRestService;
		protected Lazy<ImageSearchService> imageSearchService;
		protected Lazy<LocationService> locationService;


		private bool _isBusy = false;
		public bool IsBusy
		{
			get
			{
				return _isBusy;
			}
			set
			{
				_isBusy = value;
				RaisePropertyChanged();
			}

		}


		public void RaisePropertyChanged([CallerMemberName] string caller = "")
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this, new PropertyChangedEventArgs(caller));
			}
		}

		public BaseViewModel()
		{
			carRestService = new Lazy<CarRestService>(() => new CarRestService());
			imageSearchService = new Lazy<ImageSearchService>(() => new ImageSearchService());
			locationService = new Lazy<LocationService>(() => new LocationService());
		}

	}
}

