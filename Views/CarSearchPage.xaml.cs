using System.Diagnostics;
using Xamarin.Forms;
using XLabs.Ioc;

namespace CarSearch
{
	public partial class CarSearchPage : ContentPage
	{
		async void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
		{
			var listView = (ListView)sender;
			await Navigation.PushAsync(new CarMakeDetailView()
			{
				BindingContext = new MakeDetailsPageViewModel((MakeItemViewModel)listView.SelectedItem)
			});
			listView.SelectedItem = null;
		}

		SearchPageViewModel vm;

		public CarSearchPage()
		{
			InitializeComponent();
			vm = Resolver.Resolve<SearchPageViewModel>();
			BindingContext = vm;
		}

		protected async  override void OnAppearing()
		{
			base.OnAppearing();
			await vm.PopulateCarsByYear();
		}
	}
}

