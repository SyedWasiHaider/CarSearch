using System.Diagnostics;
using Xamarin.Forms;

namespace CarSearch
{
	public partial class CarSearchPage : ContentPage
	{
		async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var make = (CarMake)e.Item;
			await Navigation.PushAsync(new CarMakeDetailView() { BindingContext = make });
		}

		CarSearchViewModel vm;

		public CarSearchPage()
		{
			InitializeComponent();
			vm = new CarSearchViewModel();
			BindingContext = vm;
			vm.PopulateCarsByYear();
		}
	}
}

