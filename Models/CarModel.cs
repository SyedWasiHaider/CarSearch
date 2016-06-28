using System;
using Newtonsoft.Json;

namespace CarSearch
{
	public class CarModel
	{
		public CarModel()
		{
		}

		public string id { get; set; }
		public string name { get; set; }
		public CarYears [] years { get; set; }
	}
}

