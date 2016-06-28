﻿using System;
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
		string VEHICLE_BASE_URI = "https://api.edmunds.com/api/vehicle/v2/makes?api_key={0}&year={1}";
		string VEHICLE_DETAILS_URI = "https://api.edmunds.com/api/vehicle/v2/{0}/{1}/{2}/styles?view=full&api_key={3}";


		HttpClient client;
  		public CarRestService()
		{
			client = new HttpClient(new NativeMessageHandler());
  		}


		public async Task<JToken> getCarDetails(string make, string model, int year)
		{
			var requestUri = String.Format(VEHICLE_DETAILS_URI, make, model, year, API_KEY);
			var response = await client.GetAsync(requestUri);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				return JToken.Parse(content);
			}
			return null;
		}

		public async Task<List<CarMake>> getAllCarsByYear(int year)
		{

			var requestUri = String.Format(VEHICLE_BASE_URI, API_KEY, year);
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
