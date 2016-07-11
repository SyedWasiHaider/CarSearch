using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarSearch
{
	public interface IImageSearchService
	{
		Task<string> getImageUrl(string query);
		Task<string[]> getImageUrls(string[] queries, List<BaseImageItemViewModel> imageViewModels, string extra = "");
	}
}

