using Xamarin.Forms;

namespace CarSearch
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			var navPage = new NavigationPage(new CarSearchPage() { Title = "Awesome Possum Car Search" } ) { BarBackgroundColor = Color.FromRgb(53, 183, 104) };
			//navPage.Tint = 
			MainPage = navPage;
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

