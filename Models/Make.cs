using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CarSearch
{
	public class Make
	{
		public Make()
		{
		}

		public string id { get; set; }
		public string name { get; set; }
		[JsonProperty("models")]
		public List<Model> modelList { get; set; }
	}
}

