using System;
using System.Net.Http;
using System.Threading.Tasks;
using ModernHttpClient;
using Newtonsoft.Json.Linq;

namespace CarSearch
{
	//Poor man's image search because Edmund's Media API is too much of a hassle to get access to.
	public class ImageSearchService
	{

		const string API_KEY = "BAulUgxQChsXfI4xdE5bOMCD+8Ff/ihKokiL6rasLHw";
		string IMAGE_SEARCH_URI = "https://api.datamarket.azure.com/Bing/Search/v1/Image?$format=JSON&$top=1";
		HttpClient client;

		public ImageSearchService()
		{
			client = new HttpClient(new NativeMessageHandler());
			var base64Str = (API_KEY + ":" + API_KEY).Base64Encode();
			client.DefaultRequestHeaders.Add("Authorization", $"Basic {base64Str}");
		}

	
		public async Task<string> getImageUrl(string query)
		{
			var test = $"{IMAGE_SEARCH_URI}&Query='{"car " + query}'";
			var response = await client.GetAsync(test);
			if (response.IsSuccessStatusCode)
			{
				var content = await response.Content.ReadAsStringAsync();
				var temp = JToken.Parse(content);
				return temp["d"]["results"][0]["MediaUrl"].ToString();
			}
			return String.Empty;
		}
	}
}

