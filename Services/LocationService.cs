using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using Plugin.Geolocator;

namespace CarSearch
{
	public class LocationService : ILocationService
	{

		const string API_KEY = "AtM3qjQiWJBmaLPDrhGWGvNUGmGEmDkezwFcM8jlnlwX_LvfHMfdpYWV_2ZRwWSe";

		const string BASE_URL = "http://dev.virtualearth.net/REST/v1/Locations/{0},{1}?&key={2}";
		HttpClient client;

		public LocationService()
		{
			client = new HttpClient(new NativeMessageHandler());
		}

		public async Task<string> getCurrentZipCode()
		{
			var position = await CrossGeolocator.Current.GetPositionAsync(timeoutMilliseconds: 5000);
			var zipCodeRequest = String.Format(BASE_URL, position.Latitude, position.Longitude, API_KEY);
			var response = await client.GetAsync(zipCodeRequest);
			try
			{
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var temp = JToken.Parse(content);
					return temp["resourceSets"][0]["resources"][0]["address"]["postalCode"].ToString(); //What could possibly go wrong?
				}
			}
			catch(Exception e)
			{
				Debug.WriteLine(e.Message);
			}

			return "00000";

		}
	}
}

