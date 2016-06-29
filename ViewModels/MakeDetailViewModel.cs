﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Realms;

namespace CarSearch
{
	public class MakeDetailViewModel : BaseViewModel
	{
		public MakeDetailViewModel(CarMake make)
		{
			Cars = new ObservableCollection<CarModel>(make.modelList);
			Name = make.name;
			Url = make.imageUrl;
			PopulateCarImageUrls();
			PopulateCarDescriptions();
		}

		public async void PopulateCarDescriptions()
		{
			for (int i = 0; i < Cars.Count(); i++)
			{
				try
				{
					var details = await carRestService.Value.getCarDetails(Name, Cars[i].name, Cars[i].year);
					if (details != null && details["styles"].ToArray().Count() > 0)
					{
						Cars[i].engine = await JsonConvert.DeserializeObjectAsync<Engine>(details["styles"][0]["engine"].ToString());
						Cars[i].Mpg = await JsonConvert.DeserializeObjectAsync<Mpg>(details["styles"][0]["MPG"].ToString());
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
			var carNames = Cars.Select(car => (car.id + " " + car.years.First().year).Replace("_", " ")).ToArray();
			var carUrls = await imageSearchService.Value.getImageUrls(carNames, "cars ");
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

		ObservableCollection<CarModel> _cars = new ObservableCollection<CarModel>();
		public ObservableCollection<CarModel> Cars
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

