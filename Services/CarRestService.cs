using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CarSearch
{
	public class CarRestService
	{
		const string API_KEY = "sbga8ynjh5bxvmubk2ab2rer";
		string VEHICLE_BASE_URI = $"https://api.edmunds.com/api/vehicle/v2/makes?api_key={API_KEY}";

		HttpClient client;
  		public CarRestService()
		{
			client = new HttpClient(new NativeMessageHandler());
  		}


		public async Task<List<CarMake>> getAllCarsByYear(int year)
		{

			var requestUri = $"{VEHICLE_BASE_URI}&year={year}";
			var response = await client.GetAsync(requestUri);
			List<CarMake> result = null;
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var temp = JToken.Parse(content);
				result = JsonConvert.DeserializeObject<List<CarMake>>(temp["makes"].ToString());
			}
			return result;
		}

	}
}

