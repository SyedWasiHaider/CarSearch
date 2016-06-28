﻿using System.Diagnostics;
using Xamarin.Forms;

namespace CarSearch
{
	public partial class CarSearchPage : ContentPage
	{
		async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var make = (CarMake)e.Item;
			((ListView)(sender)).SelectedItem = null;
			await Navigation.PushAsync(new CarMakeDetailView() { BindingContext = new MakeDetailViewModel(make) });
		}

		SearchViewModel vm;

		public CarSearchPage()
		{
			InitializeComponent();
			vm = new SearchViewModel();
			BindingContext = vm;
		}

		protected async  override void OnAppearing()
		{
			base.OnAppearing();
			await vm.PopulateCarsByYear();
		}
	}
}

