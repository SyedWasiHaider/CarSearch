using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CarSearch
{
	public class CarMake : BaseViewModel
	{
		public CarMake()
		{
		}

		public string id { get; set; }
		public string name { get; set; }
		[JsonProperty("models")]
		public List<CarModel> modelList { get; set; }

		private string _imageUrl = "genericcar.png";
		public string imageUrl
		{
			get
			{
				return _imageUrl;
			}
			set
			{
				_imageUrl = value;
				RaisePropertyChanged();
			}
		}
	}
}

