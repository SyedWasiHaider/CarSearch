using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace CarSearch
{
	public interface ICarRestService
	{
		Task<List<Make>> getAllCarMakesByYear(int year);
		Task<JToken> getCarDetails(string make, string model, int year);
	}
}

