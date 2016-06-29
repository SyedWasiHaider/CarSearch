using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Realms;
using Xamarin.Forms;

namespace CarSearch
{
	public class MakeDetailsPageViewModel : BaseViewModel
	{
		public MakeDetailsPageViewModel(MakeItemViewModel makeVM)
		{
			MakeViewModel = makeVM;
			var tempCars = new ObservableCollection<ModelItemViewModel>();
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
						var zipCode = await locationService.Value.getCurrentZipCode();
						Device.OpenUri(new Uri($"https://www.google.com/search?q={searchObj.ToString()} dealerships near {zipCode}"));
					}
					catch
					{
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
					var details = await carRestService.Value.getCarDetails(Name, Cars[i].Model.name, Cars[i].year);
					if (details != null && details["styles"].ToArray().Count() > 0)
					{
						Cars[i].engine = await JsonConvert.DeserializeObjectAsync<Engine>(details["styles"][0]["engine"].ToString());
						Cars[i].Mpg = await JsonConvert.DeserializeObjectAsync<Mileage>(details["styles"][0]["MPG"].ToString());
						Cars[i].MSRP =  await JsonConvert.DeserializeObjectAsync<int>(details["styles"][0]["price"]["baseMSRP"].ToString());
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
			var carNames = Cars.Select(car => car.DescriptiveName).ToArray();
			var carUrls = await imageSearchService.Value.getImageUrls(carNames, "car ");
			for (int i = 0; i < Cars.Count(); i++)
			{
				if (String.IsNullOrEmpty(carUrls[i])){
					Cars[i].imageUrl = Url;
				}
				else {
					Cars[i].imageUrl = carUrls[i];
				}
			}
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

  
		ObservableCollection<ModelItemViewModel> _cars = new ObservableCollection<ModelItemViewModel>();
		public ObservableCollection<ModelItemViewModel> Cars
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

