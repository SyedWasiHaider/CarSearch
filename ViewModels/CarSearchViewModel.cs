using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Realms;
using Xamarin.Forms;
using System.Linq;

namespace CarSearch
{
	public class CarSearchViewModel : BaseViewModel
	{

		CarRestService careRestService;
		ImageSearchService imageSearchService;

		List<CarMake> _cars = new List<CarMake>();
		public List<CarMake> Cars
		{
			get
			{
				if (!String.IsNullOrEmpty(ActiveSearchTerm))
				{
					return _cars.FindAll(car => car.name.ToUpper().StartsWith(ActiveSearchTerm.ToUpper()));
				}
				else {
					return _cars;
				}
			}
			set
			{
				_cars = value;
				RaisePropertyChanged();
			}
		}

		private string _activeSearchTerm = String.Empty;
		public string ActiveSearchTerm
		{
			get
			{
				return _activeSearchTerm;
			}
			set
			{
				_activeSearchTerm = value;
				RaisePropertyChanged();
				RaisePropertyChanged("Cars");
			}
		}

		public CarSearchViewModel()
		{
			careRestService = new CarRestService();
			imageSearchService = new ImageSearchService();
		}

		public async Task PopulateCarsByYear(int year = 2016)
		{
			try
			{
				IsBusy = true;
				var tempCars = await careRestService.getAllCarsByYear(year);
				Cars = tempCars;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			finally
			{
				IsBusy = false;
			}

			//Some caching logic for the urls
			//I'd cache the images themselves but
			//ain't not body got time for that right now.
			//At least this prevents me from using up all my Bing API calls (I get 5000 a day)
			var realm = Realm.GetInstance();
			foreach (var carMake in Cars)
			{
				var carImageUrl = realm.All<CarImageUrl>().First(urlObj => urlObj.name == carMake.name && urlObj.expiry < DateTimeOffset.UtcNow);
				if (carImageUrl == null)
				{
					carMake.imageUrl = carImageUrl.url;
				}
				else {
					var url = await imageSearchService.getImageUrl(carMake.name);
					carMake.imageUrl = url;
					realm.Write(() =>
					{
						var newCarImageUrl = realm.CreateObject<CarImageUrl>();
						newCarImageUrl.expiry = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromDays(30));
						newCarImageUrl.name = carMake.name;
						newCarImageUrl.url = url;
					});
				}
			}

		}


	}
}

