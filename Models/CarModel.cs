using System;
using System.Linq;
using Newtonsoft.Json;

namespace CarSearch
{
	public class CarModel : BaseViewModel
	{
		public CarModel()
		{
		}

		public string id { get; set; }
		public string make { get; set; }
		public string name { get; set; }
		public CarYears [] years { get; set; }
		public int year {
			get
			{
				return years.FirstOrDefault().year;
			}
		}

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

