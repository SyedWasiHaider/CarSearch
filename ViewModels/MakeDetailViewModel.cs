using System;
using System.Collections.Generic;
using System.Linq;
using Realms;

namespace CarSearch
{
	public class MakeDetailViewModel : BaseViewModel
	{
		public MakeDetailViewModel(CarMake make)
		{
			Cars = make.modelList;
			Name = make.name;
			Url = make.imageUrl;
			PopulateCarImageUrls();
		}


		public async void PopulateCarImageUrls()
		{
			var carNames = Cars.Select(car => (car.id + " " + car.years.First().year).Replace("_", " ")).ToArray();
			var carUrls = await imageSearchService.Value.getImageUrls(carNames, "cars ");
			for (int i = 0; i < Cars.Count(); i++)
			{
				Cars[i].imageUrl = carUrls[i];
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

		List<CarModel> _cars = new List<CarModel>();
		public List<CarModel> Cars
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

