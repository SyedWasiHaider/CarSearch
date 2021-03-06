﻿using System;
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
		
		List<BaseImageItemViewModel> _itemsViewModel = new List<BaseImageItemViewModel>();
		public List<BaseImageItemViewModel> ItemsViewModel
		{
			get
			{
				if (!String.IsNullOrEmpty(ActiveSearchTerm))
				{
					return _itemsViewModel.FindAll(item => (item as MakeItemViewModel).Make.name.ToUpper().StartsWith(ActiveSearchTerm.ToUpper()) ||

										  (item as MakeItemViewModel).Make.modelList.Any(car => car.name.ToUpper().StartsWith(ActiveSearchTerm.ToUpper())));
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

		private IImageSearchService imageSearchService;
		private ICarRestService carRestService;

		public SearchPageViewModel(IImageSearchService imgService, ICarRestService carService)
		{
			imageSearchService = imgService;
			carRestService = carService;
		}

		public async Task PopulateCarImageUrls()
		{
			var carNames = ItemsViewModel.Select(item => (item as MakeItemViewModel).Make.name).ToArray();
			await imageSearchService.getImageUrls(carNames, ItemsViewModel, "cars ");

		}

		public async Task PopulateCarsByYear(int year = 2016)
		{
			try
			{
				IsBusy = true;
				var tempMakes = await carRestService.getAllCarMakesByYear(year);
				var items = new List<BaseImageItemViewModel>();
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

