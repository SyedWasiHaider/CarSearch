using System;
using Realms;

namespace CarSearch
{
	public class CarImageUrl : RealmObject
	{
		public CarImageUrl()
		{
		}

		public string name { get; set;}
		public string url { get; set;}
		public DateTimeOffset expiry { get; set;}
	}
}

