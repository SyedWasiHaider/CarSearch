using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Realms;
using Xamarin.Forms;
using XLabs.Ioc;

namespace CarSearch
{
	public class MakeDetailsPageViewModel : BaseViewModel
	{
		public MakeDetailsPageViewModel(MakeItemViewModel makeVM)
		{
			MakeViewModel = makeVM;
			var tempCars = new List<BaseImageItemViewModel>();
			foreach(var car in MakeViewModel.Make.modelList)
			{
				tempCars.Add(new ModelItemViewModel() { Model = car });
			}
			Cars = tempCars;

			Name = makeVM.Make.name;
			Url = makeVM.imageUrl;
			PopulateCarImageUrls();
			PopulateCarDescriptions();
			SearchCommand = new Command
				(async searchObj =>
				{
					try
					{
					var zipCode = await Resolver.Resolve<ILocationService>().getCurrentZipCode();
						Device.OpenUri(new Uri($"https://www.google.com/search?q={searchObj.ToString()} dealerships near {zipCode}"));
					}
					catch(Exception e)
					{
						Debug.WriteLine(e.Message);
					}
				});
		}

		public Command SearchCommand
		{
			get;set;
		}

		public async void PopulateCarDescriptions()
		{
			for (int i = 0; i < Cars.Count(); i++)
			{
				try
				{
					var details = await Resolver.Resolve<ICarRestService>().getCarDetails(Name, (Cars[i] as ModelItemViewModel).Model.name, (Cars[i] as ModelItemViewModel).year);
					if (details != null && details["styles"].ToArray().Count() > 0)
					{
						(Cars[i] as ModelItemViewModel).engine = await JsonConvert.DeserializeObjectAsync<Engine>(details["styles"][0]["engine"].ToString());
						(Cars[i] as ModelItemViewModel).Mpg = await JsonConvert.DeserializeObjectAsync<Mileage>(details["styles"][0]["MPG"].ToString());
						(Cars[i] as ModelItemViewModel).MSRP =  await JsonConvert.DeserializeObjectAsync<int>(details["styles"][0]["price"]["baseMSRP"].ToString());
					}
				}
				catch (Exception e)
				{

					Debug.WriteLine(e.Message);
				}

			}
		}

		public async void PopulateCarImageUrls()
		{
			var carNames = Cars.Select(car => (car as ModelItemViewModel).DescriptiveName).ToArray();
			await Resolver.Resolve<IImageSearchService>().getImageUrls(carNames, Cars, "car ");
		}

		private string _url;
		public string Url
		{
			get
			{
				return _url;
			}
			set
			{
				_url = value;
				RaisePropertyChanged();
			}

		}

		private string _name;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
				RaisePropertyChanged();
			}

		}

		private MakeItemViewModel _makeViewModel;
		public MakeItemViewModel MakeViewModel
		{
			get {
				return _makeViewModel;
			}
			set
			{
				_makeViewModel = value;
				RaisePropertyChanged();
			}
		}

  
		List<BaseImageItemViewModel> _cars = new List<BaseImageItemViewModel>();
		public List<BaseImageItemViewModel> Cars
		{
			get
			{
				return _cars;
			}
			set
			{
				_cars = value;
				RaisePropertyChanged();
			}
		}
	}
}

