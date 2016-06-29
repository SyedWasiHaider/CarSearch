using System;
using System.Linq;
using Newtonsoft.Json;

namespace CarSearch
{
	public class Model
	{
		public Model()
		{
		}


		//Automatically serialized properties
		public string id { get; set; }
		public string make { get; set; }
		public string name { get; set; }
		public CarYears [] years { get; set; }

	}
}

