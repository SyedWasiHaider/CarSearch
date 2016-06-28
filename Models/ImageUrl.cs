using System;
using Realms;

namespace CarSearch
{
	public class ImageUrl : RealmObject
	{
		public ImageUrl()
		{
		}
		public string name { get; set;}
		public string url { get; set;}
		public DateTimeOffset expiry { get; set;}
	}
}

