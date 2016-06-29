using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Realms;
using Xamarin.Forms;
using System.Linq;
using System.Collections;

namespace CarSearch
{
	public class SearchViewModel : BaseViewModel
	{
		
		List<CarMake> _makes = new List<CarMake>();
		public List<CarMake> Makes
		{
			get
			{
				if (!String.IsNullOrEmpty(ActiveSearchTerm))
				{
					return _makes.FindAll(make => make.name.ToUpper().StartsWith(ActiveSearchTerm.ToUpper()) ||

										  make.modelList.Any(car => car.name.ToUpper().StartsWith(ActiveSearchTerm.ToUpper())));
				}
				else {
					return _makes;
				}
			}
			set
			{
				_makes = value;
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
				RaisePropertyChanged("Makes");
			}
		}

		public SearchViewModel()
		{
		}

		public async Task PopulateCarImageUrls()
		{
			var carNames = Makes.Select(car => car.name).ToArray();
			var carUrls = await imageSearchService.Value.getImageUrls(carNames, "cars ");
			for (int i = 0; i < Makes.Count(); i++)
			{
				Makes[i].imageUrl = carUrls[i];
			}
		}

		public async Task PopulateCarsByYear(int year = 2016)
		{
			try
			{
				IsBusy = true;
				var tempCars = await carRestService.Value.getAllCarsByYear(year);
				Makes = tempCars;
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
			}
			finally
			{
				IsBusy = false;
			}

			await PopulateCarImageUrls();
				
		}
	}
}

