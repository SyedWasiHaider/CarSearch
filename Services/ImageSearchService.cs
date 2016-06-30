using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json.Linq;
using Realms;

namespace CarSearch
{
	//Poor man's image search because Edmund's Media API is too much of a hassle to get access to.
	public class ImageSearchService
	{

		const string API_KEY = "BAulUgxQChsXfI4xdE5bOMCD+8Ff/ihKokiL6rasLHw";
		string IMAGE_SEARCH_URI = "https://api.datamarket.azure.com/Bing/Search/v1/Image?$format=JSON&$top=1&ImageFilters='Size:Medium'";
		HttpClient client;

		public ImageSearchService()
		{
			client = new HttpClient(new NativeMessageHandler());
			var base64Str = (API_KEY + ":" + API_KEY).Base64Encode();
			client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Str}");
		}

	
		public async Task<string> getImageUrl(string query)
		{
			var test = $"{IMAGE_SEARCH_URI}&Query='{query}'";
			var response = await client.GetAsync(test);
			try
			{
				if (response.IsSuccessStatusCode)
				{
					var content = await response.Content.ReadAsStringAsync();
					var temp = JToken.Parse(content);
					return temp["d"]["results"][0]["MediaUrl"].ToString();
				}
			}
			catch (Exception e)
			{
				Debug.WriteLine(e.Message);
				return String.Empty;
			}
			
			return String.Empty;
		}

		public async Task<string[]> getImageUrls(string[] queries, List<BaseImageItemViewModel> imageViewModels, string extra = "")
		{
			var realm = Realm.GetInstance();
			var results = new string[queries.Length];
			for (int i = 0; i < queries.Length; i++)
			{
				var sillyDateOffset = DateTimeOffset.UtcNow;
				var sillyName = queries[i];
				var carImageUrl = realm.All<ImageUrl>().Where(urlObj => urlObj.name == sillyName && urlObj.expiry > sillyDateOffset);

				if (carImageUrl.Count() > 0)
				{
					results[i] = carImageUrl.First().url;
					imageViewModels[i].imageUrl = results[i];
				}
				else {
					results[i] = await getImageUrl(extra + queries[i]);
					imageViewModels[i].imageUrl = results[i];
					realm.Write(() =>
					{
						var newImageUrl = realm.CreateObject<ImageUrl>();
						newImageUrl.expiry = DateTimeOffset.UtcNow.AddDays(30);
						newImageUrl.name = queries[i];
						newImageUrl.url = results[i];
					});
				}
			}

			return results;
		}
	}
}

