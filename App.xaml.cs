using System.Diagnostics;
using Xamarin.Forms;
using XLabs.Ioc;
using System.Reflection;

namespace CarSearch
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			var resolverContainer = new SimpleContainer();

			resolverContainer.Register<ILocationService, LocationService>();
			resolverContainer.Register<IImageSearchService, ImageSearchService>();
			resolverContainer.Register<ICarRestService, CarRestService>();
			resolverContainer.Register<SearchPageViewModel>(r => new SearchPageViewModel(r.Resolve<IImageSearchService>(), r.Resolve<ICarRestService>()));

			Resolver.SetResolver(resolverContainer.GetResolver());


			LoadStyles();
			var navPage = new NavigationPage(new CarSearchPage() { Title = "Auto Pocket" } ) { BarTextColor=AppColors.AccentTextColor, BarBackgroundColor = AppColors.AccentColor };
			//navPage.Tint = 
			MainPage = navPage;
		}

		private void LoadStyles()
		{
			StyleLoader.Load(typeof(AppColors));
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}

