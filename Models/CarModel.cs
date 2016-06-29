using System;
using System.Linq;
using Newtonsoft.Json;

namespace CarSearch
{
	public class CarModel : BaseViewModel, ISelectable
	{
		public CarModel()
		{
		}


		//Automatically serialized properties
		public string id { get; set; }
		public string make { get; set; }
		public string name { get; set; }
		public CarYears [] years { get; set; }


		//Manual or calculated properties (mostly because of how the API is setup).

		public string DescriptiveName
		{
			get
			{
				return (id + " " + years.First().year).Replace("_", " ");
			}
		}

		public int year
		{
			get
			{
				return years.FirstOrDefault().year;
			}
		}


		private Engine _engine;
		public Engine engine
		{
			get
			{
				return _engine;
			}

			set
			{
				_engine = value;
				RaisePropertyChanged();
			}
		}

		private Mpg _mpg;
		public Mpg Mpg
		{
			get
			{
				return _mpg;
			}

			set
			{
				_mpg = value;
				RaisePropertyChanged();
			}
		}


		private int _msrp;
		public int MSRP
		{
			get
			{
				return _msrp;
			}
			set
			{
				_msrp = value;
				RaisePropertyChanged();
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

		public bool IsSelected
		{
			get;set;
		}

		public int index
		{
			get;set;
		}
	}
}

