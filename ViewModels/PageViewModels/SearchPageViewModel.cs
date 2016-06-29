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
	public class SearchPageViewModel : BaseViewModel
	{
		
		List<MakeItemViewModel> _itemsViewModel = new List<MakeItemViewModel>();
		public List<MakeItemViewModel> ItemsViewModel
		{
			get
			{
				if (!String.IsNullOrEmpty(ActiveSearchTerm))
				{
					return _itemsViewModel.FindAll(item => item.Make.name.ToUpper().StartsWith(ActiveSearchTerm.ToUpper()) ||

										  item.Make.modelList.Any(car => car.name.ToUpper().StartsWith(ActiveSearchTerm.ToUpper())));
				}
				else {
					return _itemsViewModel;
				}
			}
			set
			{
				_itemsViewModel = value;
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
				RaisePropertyChanged("ItemsViewModel");
			}
		}

		public SearchPageViewModel()
		{
		}

		public async Task PopulateCarImageUrls()
		{
			var carNames = ItemsViewModel.Select(item => item.Make.name).ToArray();
			var carUrls = await imageSearchService.Value.getImageUrls(carNames, "cars ");
			for (int i = 0; i < ItemsViewModel.Count(); i++)
			{
				ItemsViewModel[i].imageUrl = carUrls[i];
			}
		}

		public async Task PopulateCarsByYear(int year = 2016)
		{
			try
			{
				IsBusy = true;
				var tempMakes = await carRestService.Value.getAllCarMakesByYear(year);
				var items = new List<MakeItemViewModel>();
				foreach (var make in tempMakes)
				{
					items.Add(new MakeItemViewModel() { Make = make });
				}
				ItemsViewModel = items;
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

